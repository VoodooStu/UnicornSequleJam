using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VoodooPackages.Tech;

public class SeedController : SingletonMB<SeedController>
{
    public List<Seed> _seedLibrary = new List<Seed>();

    
    public List<Seed> _currentSeeds = new List<Seed>();

    public void MergeSeeds(Seed firstSeed, Seed secondSeed, Action<Seed> onComplete, Action onFailed)
    {
        if(HasSeed(firstSeed) && HasSeed(secondSeed))
        {
            if (firstSeed._index == secondSeed._index && _seedLibrary.First<Seed>(u => u._index == firstSeed._index + 1) != null)
            {
                Seed newSeed = _seedLibrary.First<Seed>(u => u._index == firstSeed._index + 1);
                onComplete?.Invoke(newSeed);
                return;
            }
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
        }else if (HasSeed(seed))
        {
            onFailed?.Invoke();
        }
        else
        {
            RemoveFromSeeds(seed);
            box.PlantSeed(seed);
            onComplete?.Invoke();
        }
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
}
