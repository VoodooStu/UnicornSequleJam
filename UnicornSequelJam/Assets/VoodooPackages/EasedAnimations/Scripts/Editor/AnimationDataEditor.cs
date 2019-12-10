using UnityEditor;
using UnityEngine;

namespace VoodooPackages.Tech.EasedAnimations
{
    [CustomEditor(typeof(AnimationData))]
    public class AnimationDataEditor : Editor
    {
        private AnimationData myTarget;

        private bool isLoaded;

        private float spaceIndentation = 15f;
        private float labelWeight = 25f;

        public override void OnInspectorGUI()
        {
            if (myTarget == null)
            {
                myTarget = (AnimationData) target;
            }

            if (isLoaded == false)
            {
                isLoaded = true;
            }

            EditorGUI.BeginChangeCheck();

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Delay Before", GUILayout.Width(80));
                myTarget.beforeDelay = EditorGUILayout.FloatField(myTarget.beforeDelay);
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Duration", GUILayout.Width(80));
                myTarget.duration = EditorGUILayout.FloatField(myTarget.duration);
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Delay After", GUILayout.Width(80));
                myTarget.afterDelay = EditorGUILayout.FloatField(myTarget.afterDelay);
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);
            
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Loop", GUILayout.Width(80));
                myTarget.loop = EditorGUILayout.Toggle(myTarget.loop);
            }
            EditorGUILayout.EndHorizontal();

            
            
            if (EditorGUI.EndChangeCheck())
            {
                if (myTarget.duration <= 0)
                {
                    myTarget.duration = 1;
                }
            }

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            ShowVector3Parameters(ref myTarget.position, "Position");
            GUILayout.Space(10);
            ShowVector3Parameters(ref myTarget.rotation, "Rotation");
            GUILayout.Space(10);
            ShowVector3Parameters(ref myTarget.scale, "Scale");
            GUILayout.Space(10);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            
            ShowFloatParameters(ref myTarget.alpha, "Alpha");
            
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            ShowRotate(ref myTarget.rotate);
            
            
            
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(target);
            }

        }

        // Show Parameters
       

        #region Float
        private void ShowFloatParameters(ref FloatData _data, string _label)
        {
            _data.toggle = EditorGUILayout.ToggleLeft(_label, _data.toggle);
            if (_data.toggle)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    ShowMethodButtons(ref _data);
                }
                EditorGUILayout.EndHorizontal();
                
                ShowMethodChoice(ref _data);
            }
        }
        #endregion
        
        #region Vector3
        private void ShowVector3Parameters(ref Vector3Data _data, string _label)
        {
            _data.toggle = EditorGUILayout.ToggleLeft(_label, _data.toggle);
            if (_data.toggle)
            {
                ShowAxisParameters(ref _data);
            }
            
        }
        
        private void ShowAxisParameters(ref Vector3Data _data)
        {
            if (_data.xyzData.toggle)
            {
                ShowSpecificAxis(ref _data.xyzData, "XYZ");
            }
            else
            {
                EditorGUILayout.BeginVertical();
                {
                    ShowSpecificAxis(ref _data.xyzData, "XYZ");
                    GUILayout.Space(10);
                    ShowSpecificAxis(ref _data.xData, "X");
                    ShowSpecificAxis(ref _data.yData, "Y");
                    ShowSpecificAxis(ref _data.zData, "Z");
                }
                EditorGUILayout.EndVertical();
            }
        }

        private void ShowSpecificAxis(ref FloatData _data, string _label)
        {
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Space(spaceIndentation);
                
                _data.toggle = EditorGUILayout.ToggleLeft(_label, _data.toggle, GUILayout.Width(labelWeight* _label.Length));
 
                ShowMethodButtons(ref _data);
                
                GUILayout.Space(spaceIndentation);
            }
            EditorGUILayout.EndHorizontal();

            ShowMethodChoice(ref _data);
        }
        
        #endregion

        private void ShowRotate(ref RotateData _data)
        {
            _data.toggle = EditorGUILayout.ToggleLeft("Rotate", _data.toggle);
            if (_data.toggle)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("X", GUILayout.Width(labelWeight));
                    _data.rotate.x = EditorGUILayout.FloatField(_data.rotate.x);
                    
                    EditorGUILayout.LabelField("Y", GUILayout.Width(labelWeight));
                    _data.rotate.y = EditorGUILayout.FloatField(_data.rotate.y);
                    
                    EditorGUILayout.LabelField("Z", GUILayout.Width(labelWeight));
                    _data.rotate.z = EditorGUILayout.FloatField(_data.rotate.z);

                    GUILayout.Space(20);
                    
                    EditorGUILayout.LabelField("x", GUILayout.Width(10));
                    _data.multiplier = EditorGUILayout.FloatField(_data.multiplier,GUILayout.Width(labelWeight*2));
                    
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        
        #region Method Buttons

        private void ShowMethodButtons(ref FloatData _data)
        {
            if (_data.toggle)
            {
                
                GUILayout.FlexibleSpace();
                _data.methodChoice = GUILayout.Toolbar (_data.methodChoice, new string[] {"Curve", "Values"}, GUILayout.Width(100));
                GUILayout.FlexibleSpace();
                
            }
            
        }
        private void ShowMethodChoice(ref FloatData _data)
        {
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Space(spaceIndentation);
                if (_data.toggle)
                {
                    switch (_data.methodChoice) {
                        case 0:
                            ShowCurve(ref _data);  
                            break;
                        
                        case 1:
                            ShowValue(ref _data);
                            break;
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

        }

        #endregion
       
        
        // Show Value
        private void ShowCurve(ref FloatData _data)
        {
            GUILayout.Space(spaceIndentation);
            _data.curve = EditorGUILayout.CurveField(_data.curve);
            EditorGUILayout.LabelField("x", GUILayout.Width(10));
            _data.multiplier = EditorGUILayout.FloatField(_data.multiplier, GUILayout.Width(30));
        }

        private void ShowValue(ref FloatData _data)
        {
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Space(spaceIndentation);
                EditorGUILayout.LabelField("Start Value", GUILayout.Width(100));
                _data.startValue= EditorGUILayout.FloatField(_data.startValue);
                EditorGUILayout.LabelField("End Value", GUILayout.Width(100));
                _data.endValue= EditorGUILayout.FloatField(_data.endValue);
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}