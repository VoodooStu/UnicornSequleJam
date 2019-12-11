using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoodooPackages.Tech;
using VoodooPackages.Tech.Items;
//using Voodoo.Sauce.Internal.RemoteConfig;

namespace VoodooPackages.Tool.Shop
{
    public class CategoryVisual : MonoBehaviour
    {
        public bool enableSubCategoryDots;
        public GameObject subCategoryDotPrefab;
        
        //Cache variables
        private ShopView shopView;
        private List<Image> subCategoryDotImages;
        private Sprite outlineSprite;
        private Sprite circleSprite;
        private List<Payment> payments;
        private List<SubCategoryVisual> subCategoryVisuals;
        private GameObject subCategoryDotZone;
        private ScrollRectSwipe scrollRect;
        private ToggleGroup toggleGroup;
        private LayoutGroup layoutGroup;

        /// <summary>
        /// Initialize the Category
        /// </summary>
        /// <param name="_categoryData"></param>
        /// <param name="_subCategoryDotsZoneParent"></param>
        /// <param name="_scrollRect"></param>
        public void Initialize(CategoryData _categoryData, Transform _subCategoryDotsZoneParent, ScrollRect _scrollRect)
        {
            shopView = ShopView.Instance;
            payments = new List<Payment>();
            subCategoryVisuals = new List<SubCategoryVisual>();
            scrollRect = (ScrollRectSwipe)_scrollRect;
            toggleGroup = GetComponent<ToggleGroup>();
            layoutGroup = GetComponent<LayoutGroup>();

//            List<SubCategoryDataServer> categoryDataServers = RemoteConfigManager.Instance.List<SubCategoryDataServer>();
//            categoryDataServers.AddRange(RemoteConfigManager.Instance.List<SubCategoryDataSkinServer>());
//            categoryDataServers.AddRange(RemoteConfigManager.Instance.List<SubCategoryDataBundleServer>());
            
            for (int i = 0; i < _categoryData.subCategories.Count; i++)
            {
                SubCategoryData subCategoryData = _categoryData.subCategories[i];
                if (subCategoryData != null)
                {
//                    SubCategoryDataServer categoryDataServer = categoryDataServers.Find(x => x.id == subCategoryData.id);
//                    if (categoryDataServer != null)
//                        subCategoryData.Initialize(categoryDataServer);
                    
                    SubCategoryVisual subCategoryVisual = subCategoryData.AddSubCategory(transform);
                    subCategoryVisuals.Add(subCategoryVisual);
                    payments.Add(subCategoryData.payment);
                }
            }
            
            scrollRect.content = GetComponent<RectTransform>();

            UpdateToggleGroup(_categoryData);
            InitDots(_categoryData, _subCategoryDotsZoneParent);
        }
        
        /// <summary>
        /// Show the category and modify the scrollRect content to use the RectTransform of this category
        /// </summary>
        public void Show()
        {
            gameObject.SetActive(true);
            subCategoryDotZone.SetActive(enableSubCategoryDots);
            scrollRect.content = GetComponent<RectTransform>();
            
            //FIX first layout display not beeing correct
            Canvas.ForceUpdateCanvases();
            layoutGroup.enabled = false;
            layoutGroup.enabled = true;
        }

        /// <summary>
        /// Hide the category
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
            subCategoryDotZone.SetActive(false);
        }

        /// <summary>
        /// Loop over each subCategoryData of the _categoryData  and resize them.
        /// They will match whether the default values _line & _column or the one from the their subCategoryData.
        /// </summary>
        /// <param name="_line"></param>
        /// <param name="_column"></param>
        /// <param name="_categoryData"></param>
        public void ResizeSubCategories(uint _line, uint _column, CategoryData _categoryData)
        {
            for (int i = 0; i < subCategoryVisuals.Count; i++)
            {
                SubCategoryVisual subCategoryVisual = subCategoryVisuals[i];
                subCategoryVisual.ResizeComponent();
                subCategoryVisual.ResizeContent(_line, _column, _categoryData.subCategories[i], scrollRect);
            }
        }

        /// <summary>
        /// Set the selected skin to _skin
        /// </summary>
        /// <param name="_skin"></param>
        public void SelectSkin(Skin _skin)
        {
            subCategoryVisuals[shopView.GetCurrentSubCategoryIndex()].SelectSkin(_skin);
        }
        
        /// <summary>
        /// Update the category UI based on _categoryData
        /// </summary>
        /// <param name="_categoryData"></param>
        public void UpdateCategory(CategoryData _categoryData)
        {
            UpdateToggleGroup(_categoryData);
            
            for (int i = 0; i < subCategoryVisuals.Count; i++)
            {
                SubCategoryVisual subCategoryVisual = subCategoryVisuals[i];
                subCategoryVisual.UpdateSubCategory(_categoryData.subCategories[i]);
            }
        }

        /// <summary>
        /// Get the _skin toggle and set it's isOn variable to true
        /// </summary>
        /// <param name="_skin"></param>
        public void SelectToggle(Skin _skin)
        {
            foreach (SubCategoryVisual subCategoryElement in subCategoryVisuals)
            {
                SkinVisual skinVisual = subCategoryElement.GetSkinVisual(_skin);
                if (skinVisual != null)
                {
                    Toggle toggle = skinVisual.GetComponent<Toggle>();
                    toggle.isOn = true;
                    break;
                }
            }
        }
        
        private void InitDots(CategoryData _categoryData, Transform _subCategoryDotsZoneParent)
        {
            if (_subCategoryDotsZoneParent)
            {
                subCategoryDotZone = Instantiate(subCategoryDotPrefab, _subCategoryDotsZoneParent);
                subCategoryDotZone.SetActive(enableSubCategoryDots);
            }
            
            if (_categoryData.subCategories.Count > 0)
            {
                if (scrollRect)
                    scrollRect.OnPageChanged += OnPageChanged;

                subCategoryDotImages = new List<Image>();
            }
            
            if (_categoryData.subCategories.Count > 1 && subCategoryDotZone != null)
            {
                //Cache variables
                outlineSprite = Resources.Load<Sprite>("Outline");
                circleSprite = Resources.Load<Sprite>("Circle");

                if (enableSubCategoryDots)
                {
                    //Add sub category number information (small dots)
                    for (int i = 0; i < _categoryData.subCategories.Count; i++)
                    {
                        SubCategoryData subCategory = _categoryData.subCategories[i];

                        if (subCategory == null)
                            continue;

                        if (enableSubCategoryDots)
                            InstantiateDotsImage(i);
                    }
                }
            }
            else
            {
                scrollRect.movementType = ScrollRect.MovementType.Clamped;
            }
        }

        private void UpdateToggleGroup(CategoryData _categoryData)
        {
            bool allowSwitchOff = true;
            for (var i = 0; i < _categoryData.subCategories.Count; i++)
            {
                var subCategory = _categoryData.subCategories[i];
                if (subCategory && subCategory is SubCategorySkinData subCategorySkinData)
                {
                    foreach (Skin skin in subCategorySkinData.skins)
                    {
                        if (skin.IsUsed && !skin.IsCollected)
                        {
                            skin.IsUsed = false;
                            Skin newIsUsedSkin = subCategorySkinData.skins.Find(x => x.IsCollected);
                            if (newIsUsedSkin)
                                newIsUsedSkin.IsUsed = true;
                        }
                        
                        if (skin.IsCollected)
                        {
                            allowSwitchOff = false;
                        }
                    }
                }
            }
            toggleGroup.allowSwitchOff = allowSwitchOff;
        }

        private void InstantiateDotsImage(int _subCategoryIndex)
        {
            GameObject newImageGO = DefaultControls.CreateImage(new DefaultControls.Resources());
            newImageGO.name = "dot " + _subCategoryIndex;
            newImageGO.transform.SetParent(subCategoryDotZone.transform);
            newImageGO.transform.localScale = Vector3.one;
            Image image = newImageGO.GetComponent<Image>();
            image.sprite = _subCategoryIndex == 0 ? circleSprite : outlineSprite;

            subCategoryDotImages.Add(image);
        }

        private void OnPageChanged(int _pageIndex)
        {
            if (!isActiveAndEnabled)
                return;
            
            if (enableSubCategoryDots && subCategoryDotImages.Count > 0)
            {
                for (int i = 0; i < subCategoryDotImages.Count; i++)
                {
                    Image image = subCategoryDotImages[i];
                    image.sprite = i == _pageIndex ? circleSprite : outlineSprite;
                }
            }

            if (_pageIndex >= 0)
            {
                Payment subCategoryPayment = payments[_pageIndex];
                shopView.DisplayPayment(subCategoryPayment);
                shopView.UpdateSubCategoryTitle();
            }
        }
    }
}