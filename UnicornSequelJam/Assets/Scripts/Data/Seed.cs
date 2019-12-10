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

    
}
