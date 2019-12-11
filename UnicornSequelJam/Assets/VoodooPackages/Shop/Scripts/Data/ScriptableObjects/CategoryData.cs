using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VoodooPackages.Tech.Items;

namespace VoodooPackages.Tool.Shop
{
    [CreateAssetMenu(fileName = "CategoryData", menuName = "VoodooPackages/Shop/Category", order = -1)]
    public class CategoryData : ScriptableObject
    {
        //Server side
        public int    id;
        public string title;
        public Color  backgroundColor;
        
        //Gameplay side
        public CategoryVisual categoryPrefab;
        
        //Server side
        public List<SubCategoryData> subCategories;
        
        private void Reset()
        {
            id = Guid.NewGuid().GetHashCode();
            title = name;
            backgroundColor = Color.white;
            subCategories = new List<SubCategoryData>();
        }
        
        /// <summary>
        /// Perform action on all the items contained in the subcategories of this category
        /// </summary>
        /// <param name="_action"></param>
        public void DoOnAllItems(Action<Item> _action)
        {
            if (_action == null)
                return;

            for (int i = 0; i < subCategories.Count; i++)
            {
                SubCategoryData subCategory = subCategories[i];
                if (subCategory != null)
                    subCategory.DoOnAllItems(_action);
            }
        }

        /// <summary>
        /// Instantiate the categoryPrefab under the viewport of the _scrollRect and initialize it with the related data.
        /// Returns the instance of the prefab.
        /// </summary>
        /// <param name="_scrollRect"></param>
        /// <param name="_subCategoryDotsZoneParent"></param>
        public CategoryVisual AddShopCategory(ScrollRect _scrollRect, Transform _subCategoryDotsZoneParent)
        {
            CategoryVisual categoryVisual = Instantiate(categoryPrefab, _scrollRect.viewport);
            categoryVisual.name = name;
            categoryVisual.Initialize(this, _subCategoryDotsZoneParent, _scrollRect);

            return categoryVisual;
        }

        /// <summary>
        /// Initialize itself with its related _categoryDataServer.
        /// </summary>
        /// <param name="categoryServer"></param>
        public virtual void Initialize(CategoryServer categoryServer)
        {
            id      = categoryServer.id;
            title   = categoryServer.title;
            ColorUtility.TryParseHtmlString(categoryServer.color, out backgroundColor);
            var possibleSubCategories = Resources.LoadAll<SubCategoryData>("Data").ToList();
            foreach (int subCategoryId in categoryServer.subCategoryIds)
            {
                subCategories.Add(possibleSubCategories.Find(x => x.id == subCategoryId));
            }
        }
    }
}