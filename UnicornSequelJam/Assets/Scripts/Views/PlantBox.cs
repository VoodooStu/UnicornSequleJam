using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class PlantBox : MonoBehaviour
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
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void PlantSeed(Seed seed)
    {
        _currentSeed = seed;
        SeedText.text = seed._name;
    }
}
