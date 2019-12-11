using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoodooPackages.Tech;

public class NewSeedCollectedView : View<NewSeedCollectedView>
{
    public Image seedImage;

    public void ShowNewSeed(Seed _seed)
    {
        Show();
        seedImage.sprite = _seed._icon;
    }
    // Start is called before the first frame update
    void Start()
    {
        Hide();
    }

    // Update is called once per frame
    
}
