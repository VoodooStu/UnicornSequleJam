using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VoodooPackages.Tech;
using static SeedBoxContainer;

public class GreenHouse : MonoBehaviour
{
    public List<Seed> _currentSeeds = new List<Seed>();
    
    public List<Seed> _currentPlants = new List<Seed>();
    public List<PlantData> _plantData = new List<PlantData>();
    public string GreenHouseName;

    private string SeedString = "CURRENT_SEEDS";
    private string PlantsString = "CURRENT_PLANTS";
    public SeedBoxContainer _seedBoxContainer;
    public PlantBoxContainer _plantBoxContainer;

    SeedBox _selectedBox;

    public void SaveSeeds()
    {
        if (_seedBoxContainer != null)
        {
            _currentSeeds = _seedBoxContainer.CurrentSeeds();
            List<string> ids = new List<string>();
            foreach (var s in _currentSeeds)
            {
                ids.Add(s._id);
            }
            BinaryPrefs.SetClass<List<string>>(GreenHouseName + SeedString, ids);
        }
        if (_plantBoxContainer != null)
        {
            _plantData = _plantBoxContainer.GetData();
            if (_plantData != null)
                BinaryPrefs.SetClass<List<PlantData>>(GreenHouseName + PlantsString, _plantData);
        }
       
        

    }

    private void Awake()
    {
        LoadSeeds();
        if (_seedBoxContainer != null)
        {
            _seedBoxContainer.OnSeedPurchase += SeedPurchaseEvent;
            _seedBoxContainer.OnSeedSelected += SeedSelectedEvent;
        }
        if (_plantBoxContainer != null)
        {
            _plantBoxContainer.OnBoxSelect += PlantBoxSelected;
        }
    }

    private void PlantBoxSelected(PlantBox _box)
    {
        if (_box.IsFull)
        {
            // Try Harvest
        }
        else if(_selectedBox!=null &&_selectedBox.IsFull)
        {
            _box.PlantSeed(_selectedBox._currentSeed);
            _selectedBox.RemoveSeed();
            
        }
        _selectedBox = null;
        SaveSeeds();


    }

    private void SeedSelectedEvent(SeedBox _seedBox)
    {
        if (_selectedBox != null&&_seedBox!=null && _selectedBox!=_seedBox)
        {
            if(_selectedBox.IsFull&& _seedBox.IsFull)
            {
                // Merge seeds attempt
                SeedController.Instance.MergeSeeds(_selectedBox._currentSeed, _seedBox._currentSeed, (s) => {
                    _seedBox.PlaceSeed(s);
                    _selectedBox.RemoveSeed();
                    _selectedBox = null;
                }, () => {
                    _selectedBox = null;

                });

            }
            else if(!_seedBox.IsFull && _selectedBox.IsFull)
            {
                // Move seed from box
                _seedBox.PlaceSeed(_selectedBox._currentSeed);
                _selectedBox.RemoveSeed();
            }
            else if (_seedBox.IsFull && !_selectedBox.IsFull)
            {
                _selectedBox = _seedBox;
            }
            else
            {
                _selectedBox = null;
            }

        }
        else
        {

            // Selecting new box
            if (_seedBox._currentSeed != null)
                _selectedBox = _seedBox;
            else
            {
                _seedBox.PlaceSeed(SeedController.Instance.GetSeed(0));
                _selectedBox = null;
            }
                
        }
        SaveSeeds();


    }

    private Seed SeedPurchaseEvent()
    {
        Seed temp =  SeedController.Instance.GetSeed(0);

        SaveSeeds();
        return temp;
    }

    private void LoadSeeds()
    {
        List<string> ids = new List<string>();
        ids = BinaryPrefs.GetClass<List<String>>(GreenHouseName + SeedString, new List<string>());
        _currentSeeds = new List<Seed>();
        if (ids != null && ids.Count > 0)
        {
            foreach (var s in ids)
            {
                _currentSeeds.Add(SeedController.Instance._seedLibrary.FirstOrDefault<Seed>(u => u._id == s));
            }
        }
        _plantData = BinaryPrefs.GetClass<List<PlantData>>(GreenHouseName + PlantsString, new List<PlantData>());
        

    }

    private void OnDestroy()
    {
        if (_seedBoxContainer != null)
        {
            _seedBoxContainer.OnSeedPurchase -= SeedPurchaseEvent;
            _seedBoxContainer.OnSeedSelected -= SeedSelectedEvent;
        }
    }

    public void AddSeed(Seed seed)
    {
        _currentSeeds.Add(seed);
        SaveSeeds();
    }

    private bool HasSeed(Seed seed)
    {
        return _currentSeeds.Contains(seed);
    }

    private void RemoveFromSeeds(Seed seed)
    {
        if (_currentSeeds.Contains(seed))
        {
            _currentSeeds.Remove(seed);
        }
    }


    internal double Initialize(DateTime lastlogin)
    {
        LoadSeeds();
        if (_seedBoxContainer!=null)
        {
            _seedBoxContainer.Initialize(_currentSeeds);
        }
        if (_plantBoxContainer != null)
        {
            _plantBoxContainer.Initialize(_plantData);
        }

        return 0;
    }
}
[Serializable]
public struct PlantData
{
    public DateTime plantTime;
    public string plantID;
}