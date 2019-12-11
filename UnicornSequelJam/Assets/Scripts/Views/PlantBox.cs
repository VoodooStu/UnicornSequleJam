using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using VoodooPackages.Tech.Times;
using DG.Tweening;

public class PlantBox : MonoBehaviour
{

    public delegate void SelectBox(PlantBox _box);
    public SelectBox OnBoxSelect;

    public MeshRenderer _mesh;
    public Material _growing;
    public Material _harvestable;
    public Material _goldProducing;

    public Transform _spawnPoint;

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
        SetColor();
        if (!IsFull)
        {
            return;
        }
        UpdateTimer();
      
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
            if (SeedText != null)
                SeedText.text = _currentSeed._name + "\n" + ((int)((currentGrowthTime / totalGrowthTime)*100)).ToString() + "%";
        }
        else
        {
            if (!_hasReturnedSeed)
            {
                if (SeedText != null)
                    SeedText.text = _currentSeed._name + "\n" + "Collect Seed";
            }
            else
            {
                if (SeedText != null)
                    SeedText.text = _currentSeed._name + "\n" + "Give Gold";
            }
                

        }

    }

    public bool IsFull
    {
        get
        {
            return _currentSeed != null;
        }
    }

    public delegate void PlantSeedCallBack(PlantBox box,SeedBox seedBox);
    public PlantSeedCallBack PlantCallBack;
    public Text SeedText;
    public Seed _currentSeed;
    public DateTime _plantTime;
    public Image _iconImage;
    internal bool _hasReturnedSeed = false;
    internal void PlantSeed(Seed seed)
    {
        CleanObjects();
        _currentSeed = seed;
        if (SeedText != null)
            SeedText.text = seed._name;
        _plantTime = DateTime.Now;
        Transform t = Instantiate(seed._sproutObject, _spawnPoint).transform;
        t.localPosition = Vector3.zero;
        t.localScale = Vector3.zero;
        t.DOScale(Vector3.one, 0.1f);
        _sapling = t.gameObject;
        _plant = Instantiate(seed._flowerObject, _spawnPoint).gameObject;
        _plant.transform.localPosition = Vector3.zero;
        _hasReturnedSeed = false;
    }

    private void CleanObjects()
    {
        if (_sapling != null)
            Destroy(_sapling);
        if (_plant != null)
            Destroy(_plant);
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
        if(SeedText!=null)
            SeedText.text = _currentSeed._name;
        _plantTime = _data.plantTime;
        _hasReturnedSeed = _data.hasReturnedSeed;
        CleanObjects();
        Transform t = Instantiate(_currentSeed._sproutObject, _spawnPoint).transform;
        t.localPosition = Vector3.zero;
        t.localScale = Vector3.zero;
        t.DOScale(Vector3.one, 0.1f);
        _sapling = t.gameObject;
        _plant = Instantiate(_currentSeed._flowerObject, _spawnPoint).gameObject;
        _plant.transform.localPosition = Vector3.zero;
        SetColor();
    }
    private GameObject _sapling;
    private GameObject _plant;

    void SaplingSwitch(bool v)
    {
        if (_sapling != null)
            _sapling.SetActive(v);
    }
    void PlantSwitch(bool v)
    {
        if (_plant != null)
            _plant.SetActive(v);
    }
    private void SetColor()
    {

        if (!IsFull)
        {
            SaplingSwitch(false);
            PlantSwitch(false);
        }
        else
        {
            SaplingSwitch(!fullyGrown);
            PlantSwitch(fullyGrown);
        }
      
    }
}

