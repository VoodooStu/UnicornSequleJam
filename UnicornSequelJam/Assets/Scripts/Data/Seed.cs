using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Seed", menuName = "Game Data/Seed")]
public class Seed : ScriptableObject
{
    public int _index;
    public string _id = Guid.NewGuid().ToString();
    public string _name;
    public Sprite _icon;
    public TimeSpan _growthTime
    {
        get
        {
            return new TimeSpan(0,_growthTimeInMinutes,0);
        }
    }
    public int _growthTimeInMinutes;
    public double _currencyPerMinute;
    
}
