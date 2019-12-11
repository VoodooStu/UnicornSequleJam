using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;
using UnityEditor.AnimatedValues;

namespace VoodooPackages.Tech
{
	[CustomEditor(typeof(ScrollRectSwipe),true)]
	[CanEditMultipleObjects]
	public class ScrollRectSwipeInspector : ScrollRectEditor
	{
		SerializedProperty m_Interactable;
		SerializedProperty m_Swipe;
		SerializedProperty m_AccuracyRatio;
		SerializedProperty m_QueueSize;
		SerializedProperty m_ResetVerticalScrollOnSwipe;
		AnimBool m_ShowHorizontalSwipeValues;

		protected override void OnEnable()
		{
			base.OnEnable();
			m_Interactable = serializedObject.FindProperty("m_Interactable");
			m_Swipe = serializedObject.FindProperty("m_Swipe");
			m_AccuracyRatio = serializedObject.FindProperty("m_AccuracyRatio");
			m_QueueSize = serializedObject.FindProperty("m_QueueSize");
			m_ResetVerticalScrollOnSwipe = serializedObject.FindProperty("m_ResetVerticalScrollOnSwipe");
			m_ShowHorizontalSwipeValues = new AnimBool(Repaint);
			SetAnimBools(true);
		}

		private void SetAnimBools(bool instant)
		{
			SetAnimBool(m_ShowHorizontalSwipeValues, !m_Swipe.hasMultipleDifferentValues && m_Swipe.boolValue, instant);
		}

		private void SetAnimBool(AnimBool a, bool value, bool instant)
		{
			if (instant)
				a.value = value;
			else
				a.target = value;
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			m_ShowHorizontalSwipeValues.valueChanged.RemoveListener(Repaint);
		}

		public override void OnInspectorGUI()
		{
			SetAnimBools(false);
			base.OnInspectorGUI();
			
			EditorGUILayout.PropertyField(m_Interactable);
			EditorGUILayout.PropertyField(m_Swipe);
			if (EditorGUILayout.BeginFadeGroup(m_ShowHorizontalSwipeValues.faded))
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.Slider(m_AccuracyRatio, 0, 1);
				EditorGUILayout.IntSlider(m_QueueSize, 1, 60);
				EditorGUILayout.PropertyField(m_ResetVerticalScrollOnSwipe);
				EditorGUI.indentLevel--;
			}
			EditorGUILayout.EndFadeGroup();

			serializedObject.ApplyModifiedProperties();
		}
	}
}