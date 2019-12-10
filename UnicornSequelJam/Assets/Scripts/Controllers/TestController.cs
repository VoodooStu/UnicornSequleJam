using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour
{
	private Seed _currentlySelectedSeed;
    private SeedBox _lastSelectedSeedBox;
    public void SelectSeedBox(SeedBox seedBox)
	{
        if (_currentlySelectedSeed != null && _lastSelectedSeedBox != null)
        {
            SeedController.Instance.MergeSeeds(seedBox._currentSeed, _currentlySelectedSeed, (s) => {
                seedBox._currentSeed = s;
                _lastSelectedSeedBox.RemoveSeed();
                _lastSelectedSeedBox = null;
            }, () => {

                _currentlySelectedSeed = null;
                _lastSelectedSeedBox = null;
            });
        }
        _lastSelectedSeedBox = seedBox;
        _currentlySelectedSeed = seedBox._currentSeed;
	}

    
}
