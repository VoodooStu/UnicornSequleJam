using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SeedBoxContainer : MonoBehaviour
{
    public List<SeedBox> _seedBoxes = new List<SeedBox>();

    public delegate void SeedSelected(SeedBox _box);
    public SeedSelected OnSeedSelected;
    public delegate void SeedDropped(SeedBox targetBox,SeedBox selectedBox);
    public SeedDropped OnSeedDropped;
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

    public bool HasEmptySpace
    {
        get
        {
            foreach (var b in _seedBoxes)
            {
                if (!b.IsFull)
                {
                    return true;
                }
            }
            return false;
        }
    }


    public SeedPurchase OnSeedPurchase;

    internal void Initialize(List<Seed> currentSeeds)
    {
        //for(int i = 0; i < _seedBoxes.Count&&i<currentSeeds.Count; i++)
        //{
        //    _seedBoxes[i].PlaceSeed(currentSeeds[i]);
        //}
        for (int i = 0; i < _seedBoxes.Count ; i++)
        {
            _seedBoxes[i].ClickCall += ClickSeedBox;
            _seedBoxes[i].DropCall += DropSeedBox;
        }
    }

    private void DropSeedBox(SeedBox targetBox,SeedBox selectedBox)
    {
        OnSeedDropped?.Invoke(targetBox,selectedBox);

    }

    public void ClickSeedBox(SeedBox _box)
    {
        
        OnSeedSelected?.Invoke(_box);
    }

    internal void AddToSlot(Seed _seed)
    {
        foreach(var b in _seedBoxes)
        {
            if (!b.IsFull)
            {
                b.PlaceSeed(_seed);
                return;
            }
        }
    }


}
