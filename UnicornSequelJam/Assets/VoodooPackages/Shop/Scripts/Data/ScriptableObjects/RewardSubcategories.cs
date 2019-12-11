using System.Collections.Generic;
using UnityEngine;
using VoodooPackages.Tech.Items;

namespace VoodooPackages.Tool.Shop
{
    [CreateAssetMenu(fileName = "RewardSubCategories", menuName = "VoodooPackages/Items/Pack/Reward Sub Categories")]
    public class RewardSubcategories : Reward
    {
        public List<int> subCategoriesSkin = new List<int>();
        public int       fallbackRewardId  = int.MinValue;

        //Cache
        private Shop        shop;
        private ItemManager itemManager;
        
        /// <summary>
        /// Called when everything is initialized
        /// </summary>
        public override void OnInitialized()
        {
            UpdateContent();
        }

        private void UpdateContent()
        {
            if (itemManager == null)
            { 
                itemManager = ItemManager.Instance;
            }
            
            if (shop == null)
            {
                shop = Shop.Instance;
            }

            if (shop == null)
                return;

            int itemId = int.MinValue;
            if (contents != null && contents.Count > 0)
            {
                itemId = contents[0].id;
            }
            
            Skin skin = itemManager.GetItem(itemId) as Skin;
            if (skin != null && skin.IsCollected == false)
            {
                skin.onSkinPurchasedSuccessfully += OnSkinCollected;
                return;
            }

            contents?.Clear();

            SubCategorySkinData[] subs = Resources.LoadAll<SubCategorySkinData>("Data");

            for (int i = 0; i < subs.Length; i++)
            {
                int index = subCategoriesSkin.IndexOf(subs[i].id);
                if (index < 0)
                { 
                    continue;
                }

                Skin newSkin = shop.GetSkin(subs[i]);
                if (newSkin != null)
                {
                    newSkin.onSkinPurchasedSuccessfully += OnSkinCollected;
                    image = newSkin.icon;
                    color = newSkin.color;
                    packName = newSkin.itemName;
                    contents.Add(new PackContent { id = newSkin.id, amount = 1 });
                    return;
                }
            }

            Pack pack = itemManager.GetPack(fallbackRewardId);
            image     = pack.image;
            color     = pack.color;
            packName  = pack.packName;
            contents.AddRange(pack.contents);
        }

        private void OnSkinCollected(Skin _skin)
        {
            _skin.onSkinPurchasedSuccessfully -= OnSkinCollected;
            UpdateContent();
        }
    }
}