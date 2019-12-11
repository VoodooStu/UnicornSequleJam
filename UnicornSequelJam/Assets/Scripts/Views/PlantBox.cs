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
        _currentSeed = seed;
        if (SeedText != null)
            SeedText.text = seed._name;
        _plantTime = DateTime.Now;
        Transform t = Instantiate(seed._sproutObject, _spawnPoint).transform;
        t.localPosition = Vector3.zero;
        t.localScale = Vector3.zero;
        t.DOScale(Vector3.one, 0.1f);
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
        if(SeedText!=null)
            SeedText.text = _currentSeed._name;
        _plantTime = _data.plantTime;
        _hasReturnedSeed = _data.hasReturnedSeed;
        SetColor();
    }

    private void SetColor()
    {
        
        Color col = Color.white;
        if (!IsFull)
        {
            if(_mesh!=null)
                _mesh.gameObject.SetActive(false);
        }
            
        else if (fullyGrown)
        {
            _mesh.gameObject.SetActive(true);
            if (_hasReturnedSeed)
            {
                if (_mesh != null)
                    _mesh.material = _goldProducing;
            }
            else
            {
                if (_mesh != null)
                    _mesh.material = _harvestable;
            }
        }
        else
        {
            _mesh.gameObject.SetActive(true);
            if (_mesh != null)
                _mesh.material = _growing;
        }
      
    }
}

