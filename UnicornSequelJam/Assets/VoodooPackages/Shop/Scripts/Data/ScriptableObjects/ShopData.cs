using System;
using System.Collections.Generic;
using UnityEngine;
using VoodooPackages.Tech.Items;

namespace VoodooPackages.Tool.Shop
{
    [CreateAssetMenu(fileName = "ShopData", menuName = "VoodooPackages/Shop/Shop", order = -2)]
	public class ShopData : ScriptableObject
	{
		public SkinVisual skinVisualPrefab;
		public BundleVisual bundleVisualPrefab;
		public Sprite placeholderSpriteItem;
		public Color placeholderSpriteColor;
		public Color itemBackgroundColor;
		public uint line;
		public uint column;
		public Currency RVBonusCurrency;
		public double RVBonusValue;
		public List<CategoryData> categories;

		private void Reset()
		{
			placeholderSpriteColor = Color.grey;
			itemBackgroundColor = Color.white;
			line = 2;
			column = 3;
			RVBonusCurrency = CurrencyManager.Instance.MainCurrency;
			RVBonusValue = 150;
			categories = new List<CategoryData>();
		}

		/// <summary>
		/// Perform action on all the items contained in the shop
		/// </summary>
		/// <param name="_action"></param>
		public void DoOnAllItems(Action<Item> _action)
		{
			if (_action == null)
				return;

			for (int i = 0; i < categories.Count; i++)
			{
				CategoryData category = categories[i];
				if (category != null)
					category.DoOnAllItems(_action);
			}
		}
	}
}