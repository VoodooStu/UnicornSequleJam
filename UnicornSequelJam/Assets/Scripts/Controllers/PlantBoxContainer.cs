using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantBoxContainer : MonoBehaviour
{

    public delegate void SelectPlantBox(PlantBox _box);
    public SelectPlantBox OnBoxSelect;
    public delegate void PlantSeedCallBack(PlantBox box, SeedBox seedBox);
    public PlantSeedCallBack PlantCall;

    public List<PlantBox> _plantBoxes = new List<PlantBox>();

    public bool HasEmptySpace
    {
        get
        {
            foreach(var b in _plantBoxes)
            {
                if (!b.IsFull)
                {
                    return true;
                }
            }
            return false;
        }
    }

    internal void Initialize(List<PlantData> currentSeeds)
    {
        for (int i = 0; i < _plantBoxes.Count; i++)
        {
            _plantBoxes[i].OnBoxSelect += ClickPlantBox;
            _plantBoxes[i].PlantCallBack += PlantCallBack;
        }
        if (currentSeeds == null || currentSeeds.Count< 1)
            return;
        for (int i = 0; i < _plantBoxes.Count && i < currentSeeds.Count; i++)
        {
            _plantBoxes[i].LoadPlant(currentSeeds[i]);
        }
       
    }

    private void PlantCallBack(PlantBox box, SeedBox seedBox)
    {
        PlantCall?.Invoke(box, seedBox);
    }

    public void ClickPlantBox(PlantBox _box)
    {
        OnBoxSelect?.Invoke(_box);
    }

    public List<PlantData> GetData()
    {
        List<PlantData> _data = new List<PlantData>();
        foreach(var b in _plantBoxes)
        {
            if (b.IsFull)
            {
                _data.Add(b.GetData());
            }
        }
        return _data;
    }
}
