using System;
using UnityEditor;
using UnityEngine;

namespace VoodooPackages.Tech.Items
{
    [CustomEditor(typeof(ItemAnimationData))]
	public class ItemAnimationScriptableEditor : Editor
	{
		private ItemAnimationData myTarget;

		private bool isLoaded;

		private string[] curveTypes;

		private Texture2D uiReverse;

		public override void OnInspectorGUI()
		{
			if (myTarget == null)
			{
				myTarget = (ItemAnimationData) target;
			}

			if (isLoaded == false)
			{
				isLoaded = true;
				string[] _temp = Enum.GetNames(typeof(Ease));

				curveTypes = new string[_temp.Length + 1];
				curveTypes[0] = "Custom";
				_temp.CopyTo(curveTypes, 1);
				
				uiReverse = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/VoodooPackages/GenericItemAnimation/Sprites/reverse.png",
					typeof(Texture2D));

			}

			EditorGUI.BeginChangeCheck();
			
			EditorGUILayout.LabelField("Appearing", EditorStyles.boldLabel);
			
			GUILayout.Space(10);
			EditorGUILayout.LabelField("Radius");
			myTarget.disc.radiusMin = EditorGUILayout.FloatField("  min",myTarget.disc.radiusMin);
			myTarget.disc.radiusMax = EditorGUILayout.FloatField("  max",myTarget.disc.radiusMax);
			myTarget.disc.radiusFactor = EditorGUILayout.FloatField("  factor",myTarget.disc.radiusFactor);
			

			GUILayout.Space(10);
			myTarget.dispersion = EditorGUILayout.Slider("Dispersion", myTarget.dispersion,0f,1f);

			GUILayout.Space(10);		
			myTarget.delayAppearing = EditorGUILayout.FloatField("Delay Appearing",myTarget.delayAppearing);
			

			GUILayout.Space(10);
			EditorGUILayout.LabelField("Speed");

			ShowCurveSelector(myTarget.speedAppearing);
			
			GUILayout.Space(10);
			EditorGUILayout.LabelField("Scale");

			ShowCurveSelector(myTarget.scaleAppearing);

			myTarget.startScaleAppearing = EditorGUILayout.Vector3Field("Start Scale", myTarget.startScaleAppearing);
			myTarget.endScaleAppearing = EditorGUILayout.Vector3Field("End Scale", myTarget.endScaleAppearing);
			
			
			GUILayout.Space(10);
			
			
			EditorGUILayout.TextArea("",GUI.skin.horizontalSlider);
			GUILayout.Space(20);
			myTarget.delayAfterAppear = EditorGUILayout.FloatField("Delay between phases", myTarget.delayAfterAppear);
			GUILayout.Space(20);
			EditorGUILayout.TextArea("",GUI.skin.horizontalSlider);

			GUILayout.Space(10);
			EditorGUILayout.LabelField("Grouping", EditorStyles.boldLabel);
			
			myTarget.delayGrouping = EditorGUILayout.FloatField("Delay Grouping",myTarget.delayGrouping);
			
			EditorGUI.BeginChangeCheck();
			myTarget.allTogether = EditorGUILayout.Toggle("All Rewards together", myTarget.allTogether);
			GUILayout.Space(10);

			if (EditorGUI.EndChangeCheck())
			{
				Repaint();
				return;
			}
			
			if (myTarget.allTogether == false)
			{
				
				myTarget.delayBetweenRewards =
					EditorGUILayout.FloatField("Delay between rewards", myTarget.delayBetweenRewards);
				
				GUILayout.Space(10);
			}
			
			GUILayout.Space(10);
			EditorGUILayout.LabelField("Speed");
			ShowCurveSelector(myTarget.speedGrouping);
			
			GUILayout.Space(10);
			EditorGUILayout.LabelField("Scale");
			ShowCurveSelector(myTarget.scaleGrouping);
			myTarget.endScaleGrouping = EditorGUILayout.Vector3Field("End Scale", myTarget.endScaleGrouping);
			
			EditorUtility.SetDirty(myTarget);
		}

		private void ShowCurveSelector(Curve _targetCurve)
		{
			EditorGUILayout.BeginHorizontal();
			{
				EditorGUI.BeginChangeCheck();
				_targetCurve.useCurve = EditorGUILayout.Toggle(_targetCurve.useCurve, GUILayout.Width(10));
				if (EditorGUI.EndChangeCheck())
				{
						return;
				}

				if (_targetCurve.useCurve)
				{
					GUI.enabled = true;
					
				}
				else
				{
					GUI.enabled = false;
				}

				GUILayout.Space(5);
				
				EditorGUI.BeginChangeCheck();
				_targetCurve.curve = EditorGUILayout.CurveField(_targetCurve.curve);
				if (EditorGUI.EndChangeCheck())
				{
					if (_targetCurve.curveTypeIndex > 0)
					{
						_targetCurve.curveTypeIndex = 0;
					}
					return;
				}
				
				EditorGUI.BeginChangeCheck();
				_targetCurve.curveTypeIndex = EditorGUILayout.Popup(_targetCurve.curveTypeIndex, curveTypes, GUILayout.MaxWidth(100));
				if (EditorGUI.EndChangeCheck())
				{
					if (myTarget.scaleAppearing.curveTypeIndex-1 >= 0)
					{
						Ease _ease = (Ease)_targetCurve.curveTypeIndex-1;

						_targetCurve.curve = CurveEasing.GenerateAnimationCurve(_ease);
					}
					return;
				}

				if (_targetCurve.curveTypeIndex - 1 >= 0)
				{
					if (GUILayout.Button(uiReverse, GUILayout.Width(20), GUILayout.Height(20)))
					{
						Ease _ease = (Ease) _targetCurve.curveTypeIndex - 1;
						_targetCurve.reversed = !_targetCurve.reversed;
						_targetCurve.curve = CurveEasing.GenerateAnimationCurve(_ease);
						return;
					}
				}
				else
				{
					GUILayout.Space(20);
				}
				GUI.enabled = true;
			}
			EditorGUILayout.EndHorizontal();
		}
		
	}
}