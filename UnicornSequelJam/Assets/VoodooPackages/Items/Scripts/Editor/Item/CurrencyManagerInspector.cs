using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace VoodooPackages.Tech.Items
{
    [CustomEditor(typeof(CurrencyManager))]
	public class CurrencyManagerInspector : Editor
	{
		private CurrencyManager currencyManager;
		private const string dataPath = "Assets/VoodooPackages/Items/Resources/Data";
		private Rect foldoutRect;
		private Texture2D sprite;
		private Dictionary<Currency,bool> displayAdvancedSettings;

		private void OnEnable()
		{
			Init();
		}

		private void Init()
		{
			currencyManager = (CurrencyManager)target;
			sprite = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/VoodooPackages/Items/Editor Default Resources/Cross.png",
				typeof(Texture2D));

			SerializedProperty currencies = serializedObject.FindProperty("currencies");
			displayAdvancedSettings = new Dictionary<Currency, bool>();
			for (int i = 0; i < currencies.arraySize; i++)
			{
				Currency currency = currencies.GetArrayElementAtIndex(i).objectReferenceValue as Currency;

				if (currency != null && !displayAdvancedSettings.ContainsKey(currency))
					displayAdvancedSettings.Add(currency, false);
			}
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			Undo.RecordObject(currencyManager, "Undo currencyManager modifications");

			EditorGUILayout.Space();

			SerializedProperty currencies = serializedObject.FindProperty("currencies");

			for (int i = 0; i < currencies.arraySize; i++)
			{
				if (currencies.GetArrayElementAtIndex(i).objectReferenceValue != null)
					Undo.RecordObject(currencies.GetArrayElementAtIndex(i).objectReferenceValue, "Undo currency modifications");
			}

			AddNewCurrency(currencies);
			DisplayCurrencyList(currencies);

			serializedObject.ApplyModifiedProperties();
		}

		private void AddNewCurrency(SerializedProperty _list)
		{
			bool addElement = GUILayout.Button("Add new Currency");

			if (addElement)
			{
				if (!Directory.Exists(dataPath))
					Directory.CreateDirectory(dataPath);

				Currency newCreatedCurrency = CreateInstance<Currency>();

				string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(dataPath, "Currency.asset"));

				AssetDatabase.CreateAsset(newCreatedCurrency, assetPathAndName);
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();

				bool isNullElementOnArray = false;
				for (int i = 0; i < _list.arraySize; i++)
				{
					SerializedProperty sp = _list.GetArrayElementAtIndex(i);
					if (sp.objectReferenceValue == null)
					{
						isNullElementOnArray = true;
						sp.objectReferenceValue = newCreatedCurrency;
						break;
					}
				}

				if (!isNullElementOnArray)
				{
					_list.InsertArrayElementAtIndex(_list.arraySize);
					_list.GetArrayElementAtIndex(_list.arraySize - 1).objectReferenceValue = newCreatedCurrency;
				}

				displayAdvancedSettings.Add(newCreatedCurrency, false);
			}
		}

		private void DisplayCurrencyList(SerializedProperty _list)
		{
			EditorGUI.BeginChangeCheck();

			EditorGUILayout.PropertyField(_list);

			bool hasChanged = EditorGUI.EndChangeCheck();

			if (hasChanged)
			{
				if (Event.current.type == EventType.DragPerform)
					return;
			}

			if (_list.isExpanded)
			{
				DisplayCurrencies(_list);
			}
		}

		public void DisplayCurrencies(SerializedProperty _list)
		{
			EditorGUI.indentLevel++;
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(_list.FindPropertyRelative("Array.size"));
			bool hasChanged = EditorGUI.EndChangeCheck();
			if (hasChanged)
				EditorUtility.SetDirty(currencyManager);
			EditorGUI.indentLevel--;

			for (int i = 0; i < _list.arraySize; i++)
			{
				SerializedProperty currencySerializedProperty = _list.GetArrayElementAtIndex(i);
				Currency currency = currencySerializedProperty.objectReferenceValue as Currency;
				
				if (currency != null && !displayAdvancedSettings.ContainsKey(currency))
					displayAdvancedSettings.Add(currency, false);

				DisplayCurrency(currency, i);

				EditorGUILayout.Space();
			}

			displayAdvancedSettings = displayAdvancedSettings.Where(kvp => kvp.Key != null).ToDictionary(key => key.Key, value => value.Value);
		}

		public void DisplayCurrency(Currency _currency, int _currencyIndex)
		{
			EditorGUILayout.BeginHorizontal();
			{
				GUILayout.Space(16);
				EditorGUILayout.BeginVertical("Box");
				{
					EditorGUILayout.BeginHorizontal();
					{
						if (_currency == null)
						{
							GUI.enabled = false;
							EditorGUILayout.ObjectField(null, typeof(Sprite), false, GUILayout.Height(EditorGUIUtility.singleLineHeight * 4), GUILayout.Width(EditorGUIUtility.singleLineHeight * 4));
							GUI.enabled = true;
						}
						else
							_currency.icon = (Sprite)EditorGUILayout.ObjectField(_currency.icon, typeof(Sprite), false, GUILayout.Height(EditorGUIUtility.singleLineHeight * 4), GUILayout.Width(EditorGUIUtility.singleLineHeight * 4));

						EditorGUILayout.BeginVertical();
						{
							Currency newCurrency = (Currency)EditorGUILayout.ObjectField(_currency, typeof(Currency),false);

							if (newCurrency != _currency)
							{
								currencyManager.currencies[_currencyIndex] = newCurrency;
								EditorUtility.SetDirty(currencyManager);
							}

							GUILayout.Space(6);
							if (_currency == null)
							{
								GUI.enabled = false;
								EditorGUILayout.DoubleField(0);
								GUI.enabled = true;
							}
							else
								_currency.currentAmount = EditorGUILayout.DoubleField(_currency.currentAmount);
							GUILayout.Space(6);
							if (_currency == null)
							{
								GUI.enabled = false;
								EditorGUILayout.ColorField(default);
								GUI.enabled = true;
							}
							else
								_currency.color = EditorGUILayout.ColorField(_currency.color);
						}
						EditorGUILayout.EndVertical();

						EditorGUILayout.BeginVertical(GUILayout.Width(EditorGUIUtility.singleLineHeight * 2));
						{
							GUILayout.FlexibleSpace();
							GUI.contentColor = Color.red;
							bool removeElement = GUILayout.Button(new GUIContent(sprite), GUILayout.Height(EditorGUIUtility.singleLineHeight * 2), GUILayout.Width(EditorGUIUtility.singleLineHeight * 2));

							if (removeElement)
								currencyManager.currencies.RemoveAt(_currencyIndex);

							GUI.contentColor = Color.white;
							GUILayout.FlexibleSpace();
						}
						EditorGUILayout.EndVertical();
					}
					EditorGUILayout.EndHorizontal();

					EditorGUILayout.Space();

					Rect rect = EditorGUILayout.GetControlRect(false, 1);
					EditorGUI.DrawRect(rect, new Color(0.4f, 0.4f, 0.4f, 1));

					EditorGUILayout.Space();

					EditorGUILayout.BeginHorizontal();

						GUILayout.Space(16);
						if (_currency == null)
						{
							GUI.enabled = false;
							EditorGUILayout.Foldout(false, "Advanced Settings");
							GUI.enabled = true;
						}
						else
							displayAdvancedSettings[_currency] = EditorGUILayout.Foldout(displayAdvancedSettings[_currency], "Advanced Settings");

					EditorGUILayout.EndHorizontal();
					if (_currency != null && displayAdvancedSettings[_currency])
						DisplayAdvancedSettings(_currency);

				}
				EditorGUILayout.EndVertical();
			}
			EditorGUILayout.EndHorizontal();
		}

		public void DisplayAdvancedSettings(Currency _currency)
		{
			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(16);
			EditorGUILayout.BeginVertical();
			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField("Id", GUILayout.Width(EditorGUIUtility.singleLineHeight * 7));
				_currency.id = EditorGUILayout.IntField(_currency.id);
			}
			EditorGUILayout.EndHorizontal();
			

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Default Amount", GUILayout.Width(EditorGUIUtility.singleLineHeight * 7));
			_currency.defaultAmount = EditorGUILayout.DoubleField(_currency.defaultAmount);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Maximum Amount", GUILayout.Width(EditorGUIUtility.singleLineHeight * 7));
			_currency.maxAmount = EditorGUILayout.DoubleField(_currency.maxAmount);
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
		}
	}
}
