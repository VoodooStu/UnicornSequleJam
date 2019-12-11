using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VoodooPackages.Tech;

public class PlantInteractorView : View<PlantInteractorView>
{
    private PlantBox _currentBox;
    public TextMeshProUGUI _nameText;
    public TextMeshProUGUI _statusText;
    public TextMeshProUGUI _coinRate;
    public void OpenPanel(PlantBox _box)
    {
        _currentBox = _box;
        FillOut();
        Show();
    }
    void FillOut()
    {
        _nameText.text = _currentBox._currentSeed._name;
        _statusText.text = _currentBox.fullyGrown?_currentBox._hasReturnedSeed?"Grown!":"Ready To Harvest!":"Growing!";
        _coinRate.text = _currentBox._currentSeed._currencyPerMinute +"/min";
    }

    private void Start()
    {
        Hide();
    }

    public void CloseClick()
    {
        _currentBox = null;
        Hide();
    }
    public void DestroySeed()
    {
        _currentBox.RemoveSeed();
        _currentBox = null;
        Hide();
    }

}
