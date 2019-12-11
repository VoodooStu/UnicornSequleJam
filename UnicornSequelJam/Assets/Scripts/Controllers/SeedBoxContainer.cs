using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedBoxContainer : MonoBehaviour
{
    public List<SeedBox> _seedBoxes = new List<SeedBox>();

    public delegate void SeedSelected(SeedBox _box);
    public SeedSelected OnSeedSelected;

    public delegate Seed SeedPurchase();

    internal List<Seed> CurrentSeeds()
    {
        List<Seed> seeds = new List<Seed>();
        foreach(var b in _seedBoxes)
        {
            if (b.IsFull)
            {
                seeds.Add(b._currentSeed);
            }
        }
        return seeds;
    }

    public SeedPurchase OnSeedPurchase;

    internal void Initialize(List<Seed> currentSeeds)
    {
        for(int i = 0; i < _seedBoxes.Count&&i<currentSeeds.Count; i++)
        {
            _seedBoxes[i].PlaceSeed(currentSeeds[i]);
        }
    }
    public void ClickSeedBox(SeedBox _box)
    {
        
        OnSeedSelected?.Invoke(_box);
    }
}
