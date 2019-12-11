using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace VoodooPackages.Tech.Items
{ 
    [CustomEditor(typeof(Reward), true)]
    public class RewardInspector : Editor
    {
        private const int SmallSpacing = 6;
        private const int SmallField = 30;
        private const int NormalField = 100;

        private Reward                  config;
        private string                  assetBaseName;

        private Dictionary<int, Item>   idToItem;
        private int                     indexToRemove;

        private void OnEnable()
        {
            config = (Reward)target;
            if (config.contents == null)
            {
                config.contents = new List<PackContent>();
            }

            assetBaseName = config.name;
            GetExistingItems();
            indexToRemove = -1;
        }


        private void GetExistingItems()
        {
            List<Item> items = Resources.LoadAll<Item>("").ToList();

            idToItem = new Dictionary<int, Item>();
            for (var i = 0; i < items.Count; i++)
            {
                var item = items[i];
                if (item != null && idToItem.ContainsKey(item.id) == false)
                {
                    idToItem.Add(item.id, item);
                }
            }
        }

        public Item GetItem(int id)
        {
            if (idToItem == null)
            {
                GetExistingItems();
            }

            if (idToItem.ContainsKey(id))
            {
                return idToItem[id];
            }

            return null;
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
                EditorUtility.SetDirty(config);
            }
            
            if (assetBaseName != config.name && Event.current.keyCode == KeyCode.Return)
            {
                AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(config), config.name);
                AssetDatabase.SaveAssets();
                assetBaseName = config.name;
            }

            serializedObject.ApplyModifiedProperties();
        }

        public void DrawTarget()
        {
            EditorGUILayout.BeginVertical("Box");
            {
                EditorGUILayout.BeginHorizontal("Box");
                EditorGUI.BeginChangeCheck();
                {
                    config.image = (Sprite) EditorGUILayout.ObjectField(config.image, typeof(Sprite), false,
                        GUILayout.Height(EditorGUIUtility.singleLineHeight * 4),
                        GUILayout.Width(EditorGUIUtility.singleLineHeight * 4));
                }
                if (EditorGUI.EndChangeCheck())
                {
                    EditorUtility.SetDirty(config);
                }
                
                EditorGUILayout.BeginVertical();
                
                EditorGUI.BeginChangeCheck();
                {
                    config.name = EditorGUILayout.TextField(config.name);
                    config.packName = config.name;
                }
                if (EditorGUI.EndChangeCheck())
                {
                    EditorUtility.SetDirty(config);
                }
                
                GUILayout.Space(SmallSpacing);
                
                EditorGUI.BeginChangeCheck();
                {
                    config.id = EditorGUILayout.IntField(config.id);
                }
                if (EditorGUI.EndChangeCheck())
                {
                    EditorUtility.SetDirty(config);
                }
                
                GUILayout.Space(SmallSpacing);
                
                EditorGUI.BeginChangeCheck();
                {
                    config.color = EditorGUILayout.ColorField(config.color);
                }
                if (EditorGUI.EndChangeCheck())
                {
                    EditorUtility.SetDirty(config);
                }
                
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
                
                DrawPackContents();                    
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawPackContents()
        {
            GUILayout.Space(SmallSpacing);

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Items and amounts");
                if (GUILayout.Button("+", GUILayout.Width(SmallField)))
                {
                    config.contents.Add(new PackContent { id = int.MinValue, amount = 0 });
                    EditorUtility.SetDirty(config);
                }

                if (GUILayout.Button("X", GUILayout.Width(SmallField)))
                {
                    config.contents.Clear();
                    EditorUtility.SetDirty(config);
                }
            }
            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < config.contents.Count; i++)
            {
                DrawPackContent(i);
            }

            if (indexToRemove >= 0 && indexToRemove < config.contents.Count)
            {
                config.contents.RemoveAt(indexToRemove);
                indexToRemove = -1;
                EditorUtility.SetDirty(config);
            }
        }

        private void DrawPackContent(int index)
        {
            PackContent content = config.contents[index];
            Item        item    = GetItem(content.id);
            int         amount  = content.amount;
            
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Space(SmallSpacing);
                item = EditorGUILayout.ObjectField(item, typeof(Item), false) as Item;
                GUILayout.Space(SmallSpacing);                
                amount = EditorGUILayout.IntField(amount);
                GUILayout.Space(SmallSpacing);
                
                if (GUILayout.Button("X", GUILayout.Width(SmallField)))
                {
                    indexToRemove = index;
                }
            }
            EditorGUILayout.EndHorizontal();

            if (item != null)
            {
                int itemId = item?.id ?? int.MinValue;
                if (itemId != content.id || amount != content.amount)
                {
                    config.contents[index] = new PackContent { id = itemId, amount = amount };
                    EditorUtility.SetDirty(config);
                }
            }
        }
    }
}