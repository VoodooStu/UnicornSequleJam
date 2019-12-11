using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VoodooPackages.Tech;

public class SeedController : SingletonMB<SeedController>
{
    public List<Seed> _seedLibrary = new List<Seed>();

    public List<Seed> _viewedSeeds = new List<Seed>();

    

    private string SeedString = "CURRENT_SEEDS";
    private string ViewSeedString = "VIEWED_SEEDS";
    public void SaveSeeds()
    {
        List<string> ids = new List<string>();
       
        ids = new List<string>();
        foreach (var s in _viewedSeeds)
        {
            ids.Add(s._id);
        }
        BinaryPrefs.SetClass<List<string>>(ViewSeedString, ids);
    }

    internal Seed GetSeed(string plantID)
    {
        return _seedLibrary.FirstOrDefault<Seed>(u => u._id == plantID);
    }

    private void Awake()
    {
        LoadSeeds();
        
    }

    private void LoadSeeds()
    {
        List<string> ids = new List<string>();
        ids = BinaryPrefs.GetClass<List<String>>(SeedString);
        
        ids = BinaryPrefs.GetClass<List<String>>(ViewSeedString);
        if (ids != null && ids.Count > 0)
        {
            foreach (var s in ids)
            {
                _viewedSeeds.Add(_seedLibrary.FirstOrDefault<Seed>(u => u._id == s));
            }
        }
    }

    public void AddSeed(Seed seed)
    {
       
        SaveSeeds();
    }

    public void MergeSeeds(Seed firstSeed, Seed secondSeed, Action<Seed> onComplete, Action onFailed)
    {
        
            if (firstSeed._index == secondSeed._index && _seedLibrary.FirstOrDefault<Seed>(u => u._index == firstSeed._index + 1) != null)
            {
           
            Seed newSeed = _seedLibrary.FirstOrDefault<Seed>(u => u._index == firstSeed._index + 1);
            AddSeed(newSeed);
            if (!_viewedSeeds.Contains(newSeed))
            {
                _viewedSeeds.Add(newSeed);
                if (NewSeedCollectedView.Instance != null)
                    NewSeedCollectedView.Instance.ShowNewSeed(newSeed);
            }
                onComplete?.Invoke(newSeed);
                return;
            }
        

        onFailed?.Invoke();

        
    }

    internal Seed GetSeed(int v)
    {
        return _seedLibrary.First<Seed>(u => u._index == v);
    }

    public void PlantSeed(Seed seed, PlantBox box, Action onComplete, Action onFailed)
    {
        if (box.IsFull)
        {
            onFailed?.Invoke();
        }
        else
        {
           
            box.PlantSeed(seed);
            onComplete?.Invoke();
        }
    }

   
}
