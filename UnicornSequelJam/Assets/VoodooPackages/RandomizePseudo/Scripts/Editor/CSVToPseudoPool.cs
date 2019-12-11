using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;

namespace VoodooPackages.Tech
{
	public class CSVToPseudoPool : EditorWindow
	{

		// Scriptable object and paths 
		PseudoPool scriptableObject = null;
		string sourcePath = "";
		
		string[] extensionManaged = new string[2] {".csv", "csv"  };
		
		private const string dataPath = "Assets/VoodooPackages/RandomizePseudo/Data";
		
		[MenuItem("VoodooPackages/Randomize/RandomizePseudo/CSV to Pseudo Pool",false,1000)]
		static void Init()
		{
			// Get existing open window or if none, make a new one:
			CSVToPseudoPool window = (CSVToPseudoPool)EditorWindow.GetWindow(typeof(CSVToPseudoPool));
			window.Show();
		}

		void OnGUI()
		{
			#region File to import
			EditorGUILayout.LabelField("Choose file to import :", EditorStyles.boldLabel);

			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Select", GUILayout.Width(100)))
			{
				sourcePath = EditorUtility.OpenFilePanelWithFilters("Select file to import", sourcePath, extensionManaged);
			}

			EditorGUILayout.LabelField(sourcePath);

			EditorGUILayout.EndHorizontal();
			#endregion

			if (sourcePath == "")
			{
				return;
			}


			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Import", GUILayout.Width(100)))
			{
				ImportCSV();
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			
		}
		
		void ImportCSV()
		{
			// Get the File
			string file = File.ReadAllText(sourcePath);

			// Split by lines
			string[] lines = Regex.Split(file, @"\r\n|\n\r|\n|\r");



			// Scriptable
			scriptableObject = CreateInstance(typeof(PseudoPool)) as PseudoPool;
			scriptableObject._strings = new List<string>();
			
			for (int i =0; i<lines.Length; i++)
			{

				// Get the _header
				string[] header = Regex.Split(lines[i], ",");
				
				for (int y =0; y<header.Length; y++)
				{
					scriptableObject._strings.Add(header[y]);
				}
			}
			
			if(!Directory.Exists(dataPath))
			{    
				//if it doesn't, create it
				Directory.CreateDirectory(dataPath);
 
			}

			// Create the asset in the project
			AssetDatabase.CreateAsset(scriptableObject, dataPath+"/" + "NewLocalizationFile" + ".asset");
			
			AssetDatabase.Refresh();
			CSVToPseudoPool window = (CSVToPseudoPool)EditorWindow.GetWindow(typeof(CSVToPseudoPool));
			window.Close();

		}
		
	}
}