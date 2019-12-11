using System;
using System.Collections.Generic;
using UnityEngine;
using VoodooPackages.Tech.Items;

namespace VoodooPackages.Tool.Shop
{
    [CreateAssetMenu(fileName = "SubCategoryBundle", menuName = "VoodooPackages/Shop/Sub Category/Bundle")]
    public class SubCategoryBundleData : SubCategoryData
    {
        public List<Bundle> bundles;
        
        //Cache
        private ItemManager itemManager;
		
        protected override void Reset()
        {
            base.Reset();
            bundles = new List<Bundle>();
        }

        /// <summary>
        /// Method used to do an action on all the bundles' items of this subcategory
        /// </summary>
        /// <param name="_action"></param>
        public override void DoOnAllItems(Action<Item> _action)
        {
            for (int i = 0; i < bundles.Count; i++)
            {
                Bundle bundle = bundles[i];
                if (bundle == null)
                    continue;

                foreach (var packContent in bundle.contents)
                {
                    Item item = ItemManager.Instance.GetItem(packContent.id);
                    if (item != null)
                        _action(item);
                }
            }
        }

        /// <summary>
        /// Initialize the SubCategoryDataServer with the values of the _subCategoryServer as SubCategoryBundleServer.
        /// </summary>
        /// <param name="subCategoryServer"></param>
        public override void Initialize(SubCategoryServer subCategoryServer)
        {
            if (!(subCategoryServer is SubCategoryBundleServer subCategoryDataBundleServer))
                return;
            
            base.Initialize(subCategoryServer);
            itemManager = ItemManager.Instance;
            
            foreach (int bundleId in subCategoryDataBundleServer.bundleIds)
            {
                bundles.Add((Bundle)itemManager.GetPack(x => x.id == bundleId));
            }
        }
    }
}