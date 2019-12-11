using System;
using System.Collections.Generic;
using UnityEngine;
using VoodooPackages.Tech.Items;

namespace VoodooPackages.Tool.Shop
{
    [CreateAssetMenu(fileName = "SubCategorySkinData", menuName = "VoodooPackages/Shop/Sub Category/Skin")]
    public class SubCategorySkinData : SubCategoryData
    {
        public List<Skin> skins;
        
        //Cache
        private SkinManager skinManager;
		
        protected override void Reset()
        {
            base.Reset();
            skins = new List<Skin>();
        }

        /// <summary>
        /// Method used to do an action on all the skins of this subcategory
        /// </summary>
        /// <param name="_action"></param>
        public override void DoOnAllItems(Action<Item> _action)
        {
            for (int i = 0; i < skins.Count; i++)
            {
                Skin skin = skins[i];
                if (skin != null)
                    _action(skin);
            }
        }

        /// <summary>
        /// Initialize the SubCategoryDataServer with the values of the _subCategoryServer as SubCategorySkinServer.
        /// </summary>
        /// <param name="subCategoryServer"></param>
        public override void Initialize(SubCategoryServer subCategoryServer)
        {
            if (!(subCategoryServer is SubCategorySkinServer subCategoryDataSkinServer))
                return;
            
            base.Initialize(subCategoryServer);
            skinManager = SkinManager.Instance;
            
            foreach (int skinId in subCategoryDataSkinServer.skinIds)
            {
                skins.Add(skinManager.GetSkin(x => x.id == skinId));
            }
        }
    }
}