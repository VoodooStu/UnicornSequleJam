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
    public DateTime _plantTime;

    internal void PlantSeed(Seed seed)
    {
        _currentSeed = seed;
        SeedText.text = seed._name;
        _plantTime = DateTime.Now;
    }

    internal PlantData GetData()
    {
        PlantData _data;
        _data.plantTime = _plantTime;
        _data.plantID = _currentSeed._id;
        return _data;
    }

    internal void LoadPlant(PlantData _data)
    {
        _currentSeed = SeedController.Instance.GetSeed(_data.plantID);
        SeedText.text = _currentSeed._name;
        _plantTime = _data.plantTime;
    }
}

