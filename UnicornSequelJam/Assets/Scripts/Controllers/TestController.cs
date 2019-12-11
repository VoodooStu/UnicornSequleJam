using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoodooPackages.Tech;

public class TestController : SingletonMB<TestController>
{
	private Seed _currentlySelectedSeed;
    private SeedBox _lastSelectedSeedBox;
    public void SelectSeedBox(SeedBox seedBox)
	{
        if (_currentlySelectedSeed != null && _lastSelectedSeedBox != null)
        {
            SeedController.Instance.MergeSeeds(seedBox._currentSeed, _currentlySelectedSeed, (s) => {
                seedBox.PlaceSeed(s);
                _lastSelectedSeedBox.RemoveSeed();
                _lastSelectedSeedBox = null;
                _currentlySelectedSeed = null;
               
            }, () => {

                _currentlySelectedSeed = null;
                _lastSelectedSeedBox = null;
            });
        }
        else
        {
            _lastSelectedSeedBox = seedBox;
            _currentlySelectedSeed = seedBox._currentSeed;
        }
       
	}

    
}
