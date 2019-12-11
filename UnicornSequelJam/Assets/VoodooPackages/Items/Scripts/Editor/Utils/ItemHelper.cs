using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace VoodooPackages.Tech.Items
{
    public class ItemHelper : ScriptableObject
    {
        [MenuItem("VoodooPackages/Items/Unlock all skins")]
        static void UnlockSkins()
        {
            List<Item> items = Resources.LoadAll<Item>("").ToList();
            foreach (Item item in items)
            {
                if (item is Skin skin)
                {
                    skin.Collect(1);
                }
            }
        }

        [MenuItem("VoodooPackages/Items/Unbuy all skins")]
        static void UnbuySkins()
        {
            List<Skin> items = Resources.LoadAll<Skin>("").ToList();
            foreach (Skin item in items)
            {
                if (item == null)
                {
                    continue;
                }

                SkinManager.LoseSkin(item);
            }
        }

        [MenuItem("VoodooPackages/Items/Reset currencies")]
        static void ResetCurrencies()
        {
            List<Currency> currencies = Resources.LoadAll<Currency>("").ToList();
            foreach (Currency currency in currencies)
            {
                CurrencyManager.ResetCurrency(currency);
            }
        }
    }
}