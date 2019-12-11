using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SeedBox : Button
{
    public delegate void OnClickCallBack(SeedBox box);
    public OnClickCallBack ClickCall;
    public delegate void OnDropCallBack(SeedBox targetBox,SeedBox selectedBox);
    public OnDropCallBack DropCall;
    public override void OnPointerDown(PointerEventData eventData)
    {
        ClickCall?.Invoke(this);
        base.OnPointerDown(eventData);
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0))
        {
            Debug.Log("POINTER ENTER " + this.name);
            if (InputController.Instance._dragSeedBox != null && InputController.Instance._dragSeedBox != this)
                InputController.Instance._nextSeedBox = this;
        }
       
        base.OnPointerEnter(eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0))
        {
            if (InputController.Instance._dragSeedBox != null && InputController.Instance._nextSeedBox == this)
                InputController.Instance._nextSeedBox = null;
        }
        base.OnPointerEnter(eventData);
    }
    public bool IsFull
    {
        get
        {
            return _currentSeed != null;
        }
    }
    public Text _seedText;
    public Seed _currentSeed;
    public Image _seedIcon;

    internal void PlaceSeed(Seed seed)
    {
        _currentSeed = seed;
        _seedText.text = seed._name;
        _seedIcon.sprite = seed._icon;
    }

    internal void RemoveSeed()
    {
        _seedText.text = "Empty";
        _currentSeed = null;
        _seedIcon.sprite = null;
    }
    public void ForceAdd()
    {
        if (_currentSeed == null)
        {
            PlaceSeed(SeedController.Instance.GetSeed(0));
            SeedController.Instance.AddSeed(_currentSeed);
        }
        else
        {
            TestController.Instance.SelectSeedBox(this);
        }
           
    }
}
