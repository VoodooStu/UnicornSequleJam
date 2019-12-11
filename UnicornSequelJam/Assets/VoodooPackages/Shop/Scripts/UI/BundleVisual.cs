using UnityEngine;

namespace VoodooPackages.Tool.Shop
{
    public class BundleVisual : MonoBehaviour
    {
//        [HideInInspector] public string id;
//        
//        [Header("Images")]
//        public Image itemIcon;
//        public Image placeholderIcon;
//
//        //Cache
//        private Toggle itemSelection;
//        private Outline outline;
//        private ShopManager shopManager;
//
//        public void Initialize(Pack _item, ToggleGroup _toggleGroup)
//        {
//            OnInitializeShopItem(_item);
//
//            UpdateShopItem(_item);
//            if (itemSelection != null)
//            {
//                itemSelection.group = _toggleGroup;
//                itemSelection.isOn = _item.IsUsed;
//            }
//        }
//
//        private void OnInitializeShopItem(Skin _item)
//        {
//            shopManager = ShopManager.Instance;
//            itemSelection = GetComponent<Toggle>();
//            outline = GetComponent<Outline>();
//            itemSelection.onValueChanged.AddListener(x => UpdateSelection(x, _item));
//        }
//
//        public void UpdateSelection(bool _value, Skin _item)
//        {
//            _item.IsUsed = _value;
//            if (_value)
//            {
//                outline.effectDistance = new Vector2(10,-10);
//            }
//            else
//            {
//                outline.effectDistance = new Vector2(2,-2);
//            }
//        }
//
//        public virtual void UpdateShopItem(Skin _item)
//        {
//            if (_item is null)
//            {
//                gameObject.SetActive(false);
//                return;
//            }
//
//            gameObject.SetActive(true);
//            id = _item.id;
//            OnDisplayShopItem(_item);
//        }
//        
//        private void OnDisplayShopItem(Skin _skin)
//        {
//            bool displayItem = false;
//			
//            if (itemSelection)
//            {
//                itemSelection.image.color = shopManager.shop.itemBackgroundColor;
//                displayItem = true;
//            }
//			
//            if (itemIcon)
//            {
//                itemIcon.sprite = _skin.icon;
//                itemIcon.color = _skin.color;
//                displayItem = true;
//            }
//
//            if (placeholderIcon)
//            {
//                placeholderIcon.sprite = shopManager.shop.placeholderSpriteItem;
//                placeholderIcon.color = shopManager.shop.placeholderSpriteColor;
//                displayItem = true;
//            }
//			
//            itemIcon.gameObject.SetActive(_skin.IsCollected);
//            placeholderIcon.gameObject.SetActive(!_skin.IsCollected);
//            itemSelection.interactable = _skin.IsCollected;
//            UpdateSelection(_skin.IsUsed, _skin);
//			
//            gameObject.SetActive(displayItem);
//        }
    }
}