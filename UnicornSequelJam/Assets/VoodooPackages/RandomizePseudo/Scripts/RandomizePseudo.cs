using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VoodooPackages.Tech
{

    public class RandomizePseudo : MonoBehaviour
    {
        /// <summary>
        /// Return a random string from _list
        /// </summary>
        /// <param name="_list"></param>
        /// <returns>Random string</returns>
        public string GetRandomPseudo(PseudoPool _list, int _minCharacterCount = 1, int _maxCharacterCount = -1)
        {
            if (_list == null || _list._strings.Count == 0)
            {
                return "";
            }

            return _list.GetRandomString(_minCharacterCount, _maxCharacterCount);
        }

        /// <summary>
        /// Return a random string from one of the _lists list
        /// </summary>
        /// <param name="_lists"></param>
        /// <returns>Random string</returns>
        public string GetRandomPseudo(List<PseudoPool> _lists, int _minCharacterCount = 1, int _maxCharacterCount = -1)
        {
            if (_lists == null || _lists.Count == 0)
            {
                return "";
            }

            return _lists[Random.Range(0, _lists.Count)].GetRandomString(_minCharacterCount, _maxCharacterCount);
        }

        /// <summary>
        /// Return a list of random strings from the _list.
        /// </summary>
        /// <param name="_list">The list of initial strings</param>
        /// <param name="_numberRequired">The number of random element needed</param>
        /// <param name="_duplicateAllowed">If true, duplicate are allowed, automatically true if the _list can't return enough strings</param>
        /// <returns></returns>
        public List<string> GetRandomPseudos(PseudoPool _list, int _numberRequired, bool _duplicateAllowed = false, int _minCharacterCount = 1, int _maxCharacterCount = -1)
        {
            // The list to return
            List<string> randomStrings = new List<string>();

            // For non duplicate elements
            List<string> stringAvailables = new List<string>();

            // Verify if there is name in the list, and the number of element is positive without 0
            if (_list == null || _list._strings.Count == 0 || _numberRequired <= 0)
            {
                return randomStrings;
            }

            // If no duplicate, verify there is at least as many names availables as number required 
            if (_list._strings.Count < _numberRequired && _duplicateAllowed == false)
            {
                Debug.Log("Not enough element in " + _list.name + " to have no duplicate");
                _duplicateAllowed = true;
            }

            // If still duplicated, create a sub list for the random pick strategy
            if (_duplicateAllowed == false)
            {
                stringAvailables = new List<string>(_list._strings);
            }

            // Get as many name as required
            for (int i = 0; i < _numberRequired; i++)
            {
                if (_duplicateAllowed)
                {
                    // Just add a random one
                    randomStrings.Add(GetRandomPseudo(_list));
                }
                else
                {
                    // Get a random on in the available list and remove it after
                    int _index = Random.Range(0, stringAvailables.Count);
                    randomStrings.Add(stringAvailables[_index]);
                    stringAvailables.RemoveAt(_index);
                }
            }

            return randomStrings;
        }

        /// <summary>
        /// Return a list of random strings pick from _lists.
        /// </summary>
        /// <param name="_lists">Multiple lists of string</param>
        /// <param name="_numberRequired">The number of random element needed</param>
        /// <param name="_duplicateAllowed">If true, duplicate are allowed, automatically true if the _lists can't return enough strings</param>
        /// <returns></returns>
        public List<string> GetRandomPseudos(List<PseudoPool> _lists, int _numberRequired, bool _duplicateAllowed = false, int _minCharacterCount = 1, int _maxCharacterCount = -1)
        {
            // The list to return
            List<string> randomStrings = new List<string>();

            // For non duplicate elements
            List<string> stringAvailables = new List<string>();

            // Verify if there is name in the list, and the number of element is positive without 0
            if (_lists == null || _numberRequired <= 0)
            {
                return randomStrings;
            }
            
            // Test for bad character count
            if (_maxCharacterCount != -1)
            {
                if (_maxCharacterCount < _minCharacterCount)
                {
                    Debug.LogWarning("Error string size asked with min:"+_minCharacterCount+" max:"+_maxCharacterCount+ " set _maxCharacterCount to -1 (infini)");
                    _maxCharacterCount = -1;
                    
                    if (_minCharacterCount == -1)
                    {
                        Debug.LogWarning("Error _minCharacterCount can't be equal to -1 (infiny) set to 1");
                        _minCharacterCount = 1;
                    }
                }
            }

            // If no duplicate, verify there is at least as many names availables as number required 

            if (_duplicateAllowed == false)
            {
                int _allCount = 0;
                foreach (PseudoPool _namePattern in _lists)
                {
                    _allCount += _namePattern._strings.Count;
                }

                if (_allCount < _numberRequired)
                {
                    Debug.Log("Not enough element in _lists to have no duplicate");
                    _duplicateAllowed = true;

                }
                else
                {
                    foreach (PseudoPool _namePattern in _lists)
                    {
                        stringAvailables = stringAvailables.Concat(_namePattern._strings).ToList();
                    }
                }

            }
            
            int _alea;
            string _value;
            bool _correct;
            
            // Get as many name as required
            for (int i = 0; i < _numberRequired; i++)
            {
                if (_duplicateAllowed)
                {
                    // Just add a random one which match requirement
                    do
                    {
                        _correct = false;
                
                        if (randomStrings.Count == 0)
                        {
                            Debug.LogWarning("No string selection found with min:"+_minCharacterCount+" max:"+_maxCharacterCount+ " return random");
                            return randomStrings;
                        }
                        _alea = Random.Range(0, randomStrings.Count);
                        _value = randomStrings[_alea];
                        randomStrings.RemoveAt(_alea);

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
                    
                    
                    randomStrings.Add(_value);
                }
                else
                {
                    // Get a random on in the available list and remove it after
                    do
                    {
                        _correct = false;
                
                        if (randomStrings.Count == 0)
                        {
                            Debug.LogWarning("No string selection found with min:"+_minCharacterCount+" max:"+_maxCharacterCount+ " return random");
                            return randomStrings;
                        }
                        _alea = Random.Range(0, randomStrings.Count);
                        _value = randomStrings[_alea];
                        randomStrings.RemoveAt(_alea);

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
                    
                    
                    randomStrings.Add(_value);

                }
            }

            return randomStrings;
        }

        
        
        #region Singleton

        public static RandomizePseudo instance = null;

        void Awake()
        {
            //Check if instance already exists
            if (instance == null)

                //if not, set instance to this
                instance = this;

            //If instance already exists and it's not this:
            else if (instance != this)

                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);

            //Sets this to not be destroyed when reloading scene
            DontDestroyOnLoad(gameObject);
        }

        #endregion
    }
}