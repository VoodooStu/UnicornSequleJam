using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VoodooPackages.Tech.Items;

namespace VoodooPackages.Tool.Shop
{

    [CustomEditor(typeof(RewardSubcategories))]
    public class RewardSubCategoriesInspector : Editor
    {
        private const int                   SmallPacing = 6;
        private const int                   SmallField = 30;

        private RewardSubcategories         config;
        private string                      assetBaseName;

        private int                         indexToRemove;
        private Reward                      fallbackReward;

        private Dictionary<int, SubCategorySkinData>  idToSubs;


        private void OnEnable()
        {
            config = (RewardSubcategories)target;
            assetBaseName = config.name;
            GetExistingSubCategories();
            indexToRemove = -1;
            GetReward();
        }

        private void GetExistingSubCategories()
        {
            SubCategorySkinData[] subs = Resources.LoadAll<SubCategorySkinData>("Data");

            idToSubs = new Dictionary<int, SubCategorySkinData>();
            foreach (var sub in subs)
            {
                if (sub != null)
                {
                    idToSubs.Add(sub.id, sub);
                }
            }
        }

        public SubCategorySkinData GetSubCategory(int id)
        {
            if (idToSubs == null)
            {
                GetExistingSubCategories();
            }

            if (idToSubs.ContainsKey(id))
            {
                return idToSubs[id];
            }

            return null;
        }

        private void GetReward()
        {
            Reward[] rewards = Resources.LoadAll<Reward>("Data");

            for (int i = 0; i < rewards.Length; i++)
            {
                if (rewards[i].id == config.fallbackRewardId)
                {
                    fallbackReward = rewards[i];
                    break;
                }
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            Undo.RecordObject(config, "Undo modifications");

            EditorGUI.BeginChangeCheck();
            {
                DrawTarget();
            }
            if (EditorGUI.EndChangeCheck())
            {
                if (assetBaseName != config.name)
                {
                    AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(config), config.name);
                }

                EditorUtility.SetDirty(config);
                AssetDatabase.SaveAssets();
            }

            serializedObject.ApplyModifiedProperties();
        }

        public void DrawTarget()
        {
            EditorGUILayout.BeginVertical();
            {
                DrawFallbackReward();                
                DrawSubCategories();
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawFallbackReward()
        {
            GUILayout.Space(SmallPacing);
            EditorGUILayout.LabelField(new GUIContent("Sub category fallback reward", "The reward you get if no SubCategory reward's is available"));
            GUILayout.Space(SmallPacing);

            EditorGUI.BeginChangeCheck();
            {
                fallbackReward = EditorGUILayout.ObjectField(fallbackReward, typeof(Reward), false) as Reward;
            }
            if (EditorGUI.EndChangeCheck())
            {
                config.fallbackRewardId = fallbackReward?.id ?? int.MinValue;
                EditorUtility.SetDirty(config);
            }
        }

        private void DrawSubCategories()
        {
            GUILayout.Space(SmallPacing);
            EditorGUILayout.LabelField(new GUIContent("Sub category probability", "A Super reward pick from a pool belonging to the designated SubCategory"));
            GUILayout.Space(SmallPacing);

            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("+"))
                {
                    config.subCategoriesSkin.Add(int.MinValue);
                }

                if (GUILayout.Button("X", GUILayout.Width(SmallField)))
                {
                    config.subCategoriesSkin.Clear();
                }
            }
            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < config.subCategoriesSkin.Count; i++)
            {
                DrawSubCategoryLine(i);
            }

            if (indexToRemove >= 0 &&
                indexToRemove < config.subCategoriesSkin.Count)
            {
                config.subCategoriesSkin.RemoveAt(indexToRemove);
                indexToRemove = -1;
                EditorUtility.SetDirty(config);
            }
        }

        private void DrawSubCategoryLine(int index)
        {
            SubCategorySkinData sub = GetSubCategory(config.subCategoriesSkin[index]);
            
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Space(SmallPacing);
                EditorGUI.BeginChangeCheck();
                { 
                    sub = EditorGUILayout.ObjectField(sub, typeof(SubCategorySkinData), false) as SubCategorySkinData;
                }
                if (EditorGUI.EndChangeCheck())
                {
                    config.subCategoriesSkin[index] = sub.id;
                    EditorUtility.SetDirty(config);
                }
                GUILayout.Space(SmallPacing);
                
                if (GUILayout.Button("X", GUILayout.Width(SmallField)))
                {
                    indexToRemove = index;
                }
            }
            EditorGUILayout.EndHorizontal();

        }
    }
}