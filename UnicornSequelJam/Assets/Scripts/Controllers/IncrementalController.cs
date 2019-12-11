using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoodooPackages.Tech;
using VoodooPackages.Tech.Times;

public class IncrementalController : SingletonMB<IncrementalController>
{
    public List<GreenHouse> _greenHouses = new List<GreenHouse>();

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        TimeSpan span = DateTime.Now - TimeManager.Instance.LastLoginTime;
        foreach(var g in _greenHouses)
        {
            double returnAmount = g.Initialize(TimeManager.Instance.LastLoginTime);
        }
        
    }

}
