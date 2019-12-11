using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using VoodooPackages.Tech.Times;

public class PlantBox : MonoBehaviour
{
    public bool fullyGrown
    {
        get
        {
            if (!IsFull)
                return false;
            return (DateTime.Now - _plantTime) > _currentSeed._growthTime;
        }
    }
    Timer checkTimer = new Timer(100, 0, 1, true);
    private void OnEnable()
    {
        //checkTimer.Start();
        
       
    }
    bool previousFullyGrown = false;
    
    private void TimerTicked(Timer _timer)
    {
       
        if (!IsFull)
        {
            return;
        }
        UpdateTimer();
        SetColor();
        if (!previousFullyGrown && fullyGrown)
        {
            previousFullyGrown = true;
            // TODO Trigger give seed
        }
          
    }

    private void UpdateTimer()
    {
        if (!fullyGrown)
        {
            double totalGrowthTime = _currentSeed._growthTime.TotalSeconds;
            double currentGrowthTime = (DateTime.Now - _plantTime).TotalSeconds;

            SeedText.text = _currentSeed._name + "\n" + ((int)((currentGrowthTime / totalGrowthTime)*100)).ToString() + "%";
        }
        else
        {
            if (!_hasReturnedSeed)
                SeedText.text = _currentSeed._name + "\n" + "Collect Seed";
            else
                SeedText.text = _currentSeed._name + "\n" + "Give Gold";

        }

    }

    public bool IsFull
    {
        get
        {
            return _currentSeed != null;
        }
    }
    public Text SeedText;
    public Seed _currentSeed;
    public DateTime _plantTime;
    public Image _iconImage;
    internal bool _hasReturnedSeed = false;
    internal void PlantSeed(Seed seed)
    {
        _currentSeed = seed;
        SeedText.text = seed._name;
        _plantTime = DateTime.Now;
        _hasReturnedSeed = false;
    }

    internal PlantData GetData()
    {
        PlantData _data;
        _data.plantTime = _plantTime;
        _data.plantID = _currentSeed._id;
        _data.hasReturnedSeed = _hasReturnedSeed;
        return _data;
    }

    internal Seed Harvest()
    {
        _hasReturnedSeed = true;
        return _currentSeed;
    }

    private void Start()
    {
        checkTimer = new Timer(1, 0, 0f, true);
        checkTimer.Ticked += TimerTicked;
        checkTimer.Start();
    }
    private void OnDisable()
    {
        //checkTimer.Stop();
    }
    private void OnDestroy()
    {
        checkTimer.Ticked -= TimerTicked;
    }
    internal void LoadPlant(PlantData _data)
    {
        _currentSeed = SeedController.Instance.GetSeed(_data.plantID);
        SeedText.text = _currentSeed._name;
        _plantTime = _data.plantTime;
        _hasReturnedSeed = _data.hasReturnedSeed;
        SetColor();
    }

    private void SetColor()
    {
        
        Color col = Color.white;
        if (!IsFull)
            col = Color.cyan;
        else if (fullyGrown)
        {
            if (_hasReturnedSeed)
                col = Color.yellow;
            else
                col = Color.magenta;
        }
        else
        {
            col = Color.green;
        }
        if(_iconImage!=null)
            _iconImage.color = col;
    }
}

