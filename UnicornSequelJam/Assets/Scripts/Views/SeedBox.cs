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
    public Text SeedText;
    public Seed _currentSeed;
   

    internal void PlaceSeed(Seed seed)
    {
        _currentSeed = seed;
        SeedText.text = seed._name;
    }

    internal void RemoveSeed()
    {
        SeedText.text = "Empty";
        _currentSeed = null;
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
