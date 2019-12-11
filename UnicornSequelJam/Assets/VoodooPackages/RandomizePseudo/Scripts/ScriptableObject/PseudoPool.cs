using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace VoodooPackages.Tech
{
    [CreateAssetMenu(fileName = "PseudoPool", menuName = "VoodooPackages/Randomize/PseudoPool", order = 1)]
    public class PseudoPool : ScriptableObject
    {

        [FormerlySerializedAs("_Strings")] public List<string> _strings;

        public string GetRandomString(int _minCharacterCount = 1, int _maxCharacterCount = -1)
        {
            // Test for bad character count
            if (_maxCharacterCount != -1)
            {
                if (_maxCharacterCount < _minCharacterCount)
                {
                    Debug.LogWarning("Error string size asked with min:"+_minCharacterCount+" max:"+_maxCharacterCount+ " return empty");
                    return String.Empty;
                }
            }
            
            
            List<string> _StringsSelection = new List<string>(_strings);
            
            int _alea;
            string _value;
            bool _correct;

            do
            {
                _correct = false;
                
                if (_StringsSelection.Count == 0)
                {
                    Debug.LogWarning("No string selection found with min:"+_minCharacterCount+" max:"+_maxCharacterCount+ " return empty");
                    return String.Empty;
                }
                _alea = Random.Range(0, _StringsSelection.Count);
                _value = _StringsSelection[_alea];
                _StringsSelection.RemoveAt(_alea);

                if (_value.Length > _minCharacterCount)
                {
                    if (_maxCharacterCount != -1)
                    {
                        if (_value.Length < _maxCharacterCount)
                        {
                            _correct = true;
                        }
                    }
                    else
                    {
                        _correct = true;
                    }
                }
                
                
                

            } while (_correct==false);
            
            return _value;
        }

    }
}