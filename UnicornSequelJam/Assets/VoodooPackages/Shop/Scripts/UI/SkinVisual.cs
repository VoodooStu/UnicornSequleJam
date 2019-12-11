using UnityEngine;
using UnityEngine.UI;
using VoodooPackages.Tech.Items;

namespace VoodooPackages.Tool.Shop
{
    public class SkinVisual : MonoBehaviour
    {
        public int id;
        
        [Header("Images")]
        public Image skinIcon;
        public Image placeholderIcon;

        //Cache
        private Toggle skinSelection;
        private Outline outline;
        private Shop shop;

        /// <summary>
        /// Initialize the skinElement based on _skin and set is toggle group to _toggleGroup
        /// </summary>
        /// <param name="_skin"></param>
        /// <param name="_toggleGroup"></param>
        public void Initialize(Skin _skin, ToggleGroup _toggleGroup)
        {
            shop = Shop.Instance;
            skinSelection = GetComponent<Toggle>();
            outline = GetComponent<Outline>();
            skinSelection.onValueChanged.AddListener(x => UpdateSelection(x, _skin));

            UpdateSkinVisual(_skin);
            if (skinSelection != null)
            {
                skinSelection.group = _toggleGroup;
                skinSelection.isOn = _skin.IsUsed;
            }
        }
        
        /// <summary>
        /// Update the skin UI based on _skin
        /// </summary>
        /// <param name="_skin"></param>
        public void UpdateSkinVisual(Skin _skin)
        {
            if (_skin is null)
            {
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(true);
            id = _skin.id;
            DisplaySkin(_skin);
        }
        
        /// <summary>
        /// Set _skin IsUsed property to _value and modify the display accordingly
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_skin"></param>
        public void UpdateSelection(bool _value, Skin _skin)
        {
            _skin.IsUsed = _value;
            outline.effectDistance = _value ? new Vector2(10,-10) : new Vector2(2,-2);
        }

        /// <summary>
        /// Set the toggle interactivity to _enabled
        /// </summary>
        /// <param name="_enabled"></param>
        public void EnableInteractivity(bool _enabled)
        {
            skinSelection.interactable = _enabled;
        }
        
        private void DisplaySkin(Skin _skin)
        {
            bool displayItem = false;
			
            if (skinSelection)
            {
                skinSelection.image.color = shop.shopData.itemBackgroundColor;
                displayItem = true;
            }
			
            if (skinIcon)
            {
                skinIcon.sprite = _skin.icon;
                skinIcon.color = _skin.color;
                displayItem = true;
            }

            if (placeholderIcon)
            {
                placeholderIcon.sprite = shop.shopData.placeholderSpriteItem;
                placeholderIcon.color = shop.shopData.placeholderSpriteColor;
                displayItem = true;
            }
			
            skinIcon.gameObject.SetActive(_skin.IsCollected);
            placeholderIcon.gameObject.SetActive(!_skin.IsCollected);
            skinSelection.interactable = _skin.IsCollected;
            UpdateSelection(_skin.IsUsed, _skin);
			
            gameObject.SetActive(displayItem);
        }
    }
}