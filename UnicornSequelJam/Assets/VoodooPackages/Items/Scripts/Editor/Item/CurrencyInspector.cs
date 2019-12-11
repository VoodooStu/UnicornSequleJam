using UnityEditor;
using UnityEngine;

namespace VoodooPackages.Tech.Items
{
    [CustomEditor(typeof(Currency))]
	public class CurrencyInspector : Editor
	{
		private Currency currency;
		private string assetName;
		private string newAssetName;
		
		private void OnEnable()
		{
			currency = (Currency)target;
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			Undo.RecordObject(currency, "Undo currency modifications");
			DrawCurrency();

			if (!string.IsNullOrEmpty(newAssetName) && Event.current.keyCode == KeyCode.Return)
			{
				AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(currency), newAssetName);
				AssetDatabase.SaveAssets();
				newAssetName = "";
			}

			EditorUtility.SetDirty(currency);
			serializedObject.ApplyModifiedProperties();
		}

		public void DrawCurrency()
		{
			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.BeginHorizontal("Box");
			currency.icon = (Sprite)EditorGUILayout.ObjectField(currency.icon, typeof(Sprite), false, GUILayout.Height(EditorGUIUtility.singleLineHeight * 4), GUILayout.Width(EditorGUIUtility.singleLineHeight * 4));

			EditorGUILayout.BeginVertical();

			assetName = EditorGUILayout.TextField(currency.name);

			if (assetName != currency.name)
				newAssetName = assetName;

			GUILayout.Space(6);
			currency.currentAmount = EditorGUILayout.DoubleField(currency.currentAmount);
			GUILayout.Space(6);
			currency.color = EditorGUILayout.ColorField(currency.color);
			EditorGUILayout.EndVertical();

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Enabled", GUILayout.Width(120));
			currency.enabled = EditorGUILayout.Toggle(currency.enabled);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Id", GUILayout.Width(120));
			currency.id = EditorGUILayout.IntField(currency.id);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Default Amount", GUILayout.Width(120));
			currency.defaultAmount = EditorGUILayout.DoubleField(currency.defaultAmount);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Maximum Amount", GUILayout.Width(120));
			currency.maxAmount = EditorGUILayout.DoubleField(currency.maxAmount);
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.EndVertical();
		}
	}
}
