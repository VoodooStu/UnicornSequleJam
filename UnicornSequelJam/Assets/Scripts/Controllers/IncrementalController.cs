using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoodooPackages.Tech;
using VoodooPackages.Tech.Items;
using VoodooPackages.Tech.Times;

public class IncrementalController : SingletonMB<IncrementalController>
{
    public List<GreenHouse> _greenHouses = new List<GreenHouse>();
   
    private void Start()
    {
        Initialize();
    }
    Timer returnsTimer;
    public void Initialize()
    {
        returnsTimer = new Timer(10, 1,0f, true);
        returnsTimer.Looped += TimerTicked;
        returnsTimer.Start();
        TimeSpan span = DateTime.Now - TimeManager.Instance.LastLoginTime;
        double offlineEarnings = 0;
        foreach (var g in _greenHouses)
        {
            offlineEarnings += g.Initialize(span.TotalMinutes);
        }
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.AddToMainCurrency(offlineEarnings);
        }
    }

    private void TimerTicked(Timer _timer)
    {
        Debug.Log("Incremental Tick");
    }
}
