using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(SeedBox), true)]
public class SeedBoxEditor : ButtonEditor
{
    public override void OnInspectorGUI()
    {
        SeedBox data = (SeedBox)target;
        base.OnInspectorGUI();
       data._seedIcon = (Image)EditorGUILayout.ObjectField("Icon", data._seedIcon, typeof(Image), true);
        data._seedText = (Text)EditorGUILayout.ObjectField("Text", data._seedText, typeof(Text), true);
        data._currentSeed = (Seed)EditorGUILayout.ObjectField("Seed", data._currentSeed, typeof(Seed), true);
    }
}
