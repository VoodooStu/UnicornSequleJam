using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class PlantEffect : MonoBehaviour
{
    [Header("On enable")]
    public float _ShakeDuration = 1;
    public float _ShakeStrength = 1;
    public int _ShakeVibrato = 10;
    public ParticleSystem _ParticlesPlayed;
    Vector3 OriginalScale;
    
    private void Awake()
    {
        OriginalScale = transform.localScale;
    }
    private void OnEnable()
    {
        if (_ParticlesPlayed != null)
            _ParticlesPlayed.Play();
        transform.DOComplete(true);
        transform.DOShakeScale(_ShakeDuration, _ShakeStrength, _ShakeVibrato).OnComplete(() =>
        {
            transform.localScale = OriginalScale;
        });
        
    }

    

    private void OnDisable()
    {
        transform.DOComplete(true);
    }
}