using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeedBox : MonoBehaviour
{
    public bool IsFull
    {
        get
        {
            return _currentSeed != null;
        }
    }
    public Text SeedText;
    public Seed _currentSeed;
   

    internal void PlaceSeed(Seed seed)
    {
        _currentSeed = seed;
        SeedText.text = seed._name;
    }

    internal void RemoveSeed()
    {
        SeedText.text = "empty";
    }
    public void ForceAdd()
    {
        if(_currentSeed==null)
            PlaceSeed(SeedController.Instance.GetSeed(0));
    }
}
