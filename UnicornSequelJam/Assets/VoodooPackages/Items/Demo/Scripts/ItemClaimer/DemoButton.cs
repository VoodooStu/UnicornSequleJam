using System.Collections.Generic;
using UnityEngine;

namespace VoodooPackages.Tech.Items
{
    public class DemoButton : MonoBehaviour
    {
        public ItemClaimer itemClaimer;
        public List<Skin> skins = new List<Skin>();

        public void LaunchItemClaimer()
        {
            itemClaimer.ShowItem(skins[Random.Range(0, skins.Count)]);
        }
    }
}