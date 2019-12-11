using System;
using System.Threading.Tasks;
using UnityEngine;
using VoodooPackages.Tech.Items;

namespace VoodooPackages.Tool.Shop
{
    public class ShopDemoScript : MonoBehaviour
    {
	    private Shop shop;
	    private SkinManager skinManager;
		private void Start()
		{
			shop = Shop.Instance;
			skinManager = SkinManager.Instance;
			
			skinManager.OnSkinPurchasedSuccessfully += OnPurchaseSucceeded;
			shop.OnSkinAcquired += OnSkinAcquired;
		}

		private void OnDestroy()
		{
			if (shop != null)
				shop.OnSkinAcquired -= OnSkinAcquired;
			
			if (skinManager != null)
				skinManager.OnSkinPurchasedSuccessfully -= OnPurchaseSucceeded;
		}

		private Task OnSkinAcquired()
		{
			return Task.Delay(1500);
		}

		private void OnPurchaseSucceeded(Item _item)
		{
			GiveReward(_item);
		}

		private void GiveReward(Item _item)
		{
			//TODO : implement the gift of specific reward.
			Debug.Log("To implement : give reward specific for " + _item.name + " with id " + _item.id);
		}
	}
}