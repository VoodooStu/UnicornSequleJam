using TMPro;
using UnityEngine;

namespace VoodooPackages.Tool.Shop
{
    public class ShopButton : MonoBehaviour
    {
        public TextMeshProUGUI text;
        public CategoryData category;
        
        private Shop shop;
        
        private void Awake()
        {
            shop = Shop.Instance;
            if (text != null && category != null)
            {
                text.text = category.name;
            }
        }
        
        /// <summary>
        /// Will display the shop and directly show the category in the shopButton.
        /// If the category variable is left empty, it will go to the first category of the shop.
        /// </summary>
        public void DisplayShop()
        {
            if (shop == null)
                shop = Shop.Instance;
            
            if (shop)
                shop.ShowCategory(category);
        }
    }
}
