using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoodooPackages.Tech;
using TMPro;
public class OfflineWinningsView : View<OfflineWinningsView>
{
    public TextMeshProUGUI _winningsText;
    public void ShowWinnings(int coins)
    {
        _winningsText.text = "" + coins;
        Show();
    }
    public void HideScreen()
    {
        Hide();
    }
    private void Start()
    {
        Hide();
    }
}
