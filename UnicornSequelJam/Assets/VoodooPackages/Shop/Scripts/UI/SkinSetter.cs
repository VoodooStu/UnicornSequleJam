using UnityEngine;
using VoodooPackages.Tech.Items;

namespace VoodooPackages.Tool.Shop
{
    public class SkinSetter : MonoBehaviour
    {
        public Transform parent;
        public CategoryData category;

        //Cache
        private Shop shop;
        private SkinManager skinManager;
        private GameObject currentSkin;

        private void Start()
        {
            shop = Shop.Instance;
            skinManager = SkinManager.Instance;
            skinManager.OnSkinSelected += OnSkinSelected;
            
            UpdateSkin();
        }

        private void OnDestroy()
        {
            if (skinManager != null)
                skinManager.OnSkinSelected -= OnSkinSelected;
        }

        private void OnSkinSelected(Skin _skin)
        {
            UpdateSkin();
        }

        /// <summary>
        /// Update the skin currently displayed
        /// </summary>
        public void UpdateSkin()
        {
           Skin newSkin = shop.currentlyUsedSkins[category];
           
           if (currentSkin != null)
               Destroy(currentSkin);

           if (newSkin.previewPrefab != null)
           {
               currentSkin = Instantiate(newSkin.previewPrefab, parent);
           }
        }
    }
}