using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantBoxContainer : MonoBehaviour
{
    public Transform BoxParent;
    public PlantBox PlantBoxPrefab;
    public Vector3 _Spacing;
    public int _count = 9;
    public int _rowCount = 4;
    public delegate void SelectPlantBox(PlantBox _box);
    public SelectPlantBox OnBoxSelect;
    public delegate void PlantSeedCallBack(PlantBox box, SeedBox seedBox);
    public PlantSeedCallBack PlantCall;
    public delegate void SavePlantCallback();
    public SavePlantCallback SavePlants;
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
        //CreateBoxes();
        for (int i = 0; i < _plantBoxes.Count; i++)
        {
            _plantBoxes[i].BoxClean += BoxCleaned;
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

    private void BoxCleaned()
    {
        SavePlants?.Invoke();
    }

    private void CreateBoxes()
    {
        for(int i = 0; i < _count/_rowCount; i++)
        {
            for(int h = 0; h < _rowCount; h++)
            {
                PlantBox p = Instantiate(PlantBoxPrefab, BoxParent);
                p.transform.localPosition = new Vector3(_Spacing.x * h, _Spacing.y, _Spacing.z * i);
                _plantBoxes.Add(p);
            }
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

    internal double GetEarnings(double v)
    {
        double total = 0;
        foreach(var b in _plantBoxes)
        {
            if (b.IsFull&&b.fullyGrown && b._hasReturnedSeed)
            {
                total += b._currentSeed._currencyPerMinute * v;
            }
        }
        return total;
    }
}
