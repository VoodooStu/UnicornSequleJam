using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using VoodooPackages.Tech;

public class NewAreaUnlocked : View<NewAreaUnlocked>
{
    void Start()
    {
        Hide();
    }

    public void HideClick()
    {
        if (BlackOutView.Instance != null)
            BlackOutView.Instance.Show();
        base.Hide();
    }

    public override void Hide()
    {
       
    }

    public void ShowNewArea()
    {
        Show();
    }
}
