using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using VoodooPackages.Tech;
using VoodooPackages.Tech.Items;
using Random = UnityEngine.Random;

namespace VoodooPackages.Tool.Shop
{
    public class ShopPurchaseHelper : SingletonMB<ShopPurchaseHelper>
    {
        private Shop shop;
        private SkinManager skinManager;
        private ShopView shopView;

        public int numberOfRandomIteration = 25;
        
        [Range(0,1)]
        public float coeffRandomIteration = 0.1f;

        [Tooltip("In ms")]
        public int selectionMaxDelay = 1500;
        
        [Tooltip("In ms")]
        public int lastFocusDuration = 2000;

        public delegate Task RewardAnimationEvent();
        public RewardAnimationEvent OnSkinAcquired;
        
        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            shop = Shop.Instance;
            skinManager = SkinManager.Instance;
            shopView = ShopView.Instance;

            shop.OnPurchaseRequested += OnPurchaseRequested;
        }

        private void OnDestroy()
        {
            if (shop != null)
                shop.OnPurchaseRequested -= OnPurchaseRequested;
        }

        private void OnPurchaseRequested(SubCategoryData _subCategoryData, Payment _payment)
        {
            if (_subCategoryData is SubCategorySkinData subCategorySkinData)
            {
                List<Skin> remainingItems = subCategorySkinData.skins.FindAll(x => !x.IsCollected && x.enabled);
                PurchaseOrGoToNextSubCategory(remainingItems, _payment);
            }
            else if (_subCategoryData is SubCategoryBundleData subCategoryDataBundle)
            {
                List<Bundle> remainingItems = subCategoryDataBundle.bundles.FindAll(x => x.AmountAvailable > 0);
                PurchaseOrGoToNextSubCategory(remainingItems, _payment);
            }
        }

        //TODO : merge the two methods
        private void PurchaseOrGoToNextSubCategory(List<Skin> _skins, Payment _payment)
        {
            if (_skins.Count > 0)
                Purchase(_skins,_payment);
            else
            {
                int newPageIndex = shop.GetNextSubCategoryIndexWithCurrencyPurchasableElements();
                if (newPageIndex >= 0)
                    shopView.scrollRect.ChangePage(newPageIndex);
                else
                {
                    shopView.scrollRect.ChangePage(Int32.MaxValue);
                }
            }
        }

        private void PurchaseOrGoToNextSubCategory(List<Bundle> _bundles, Payment _payment)
        {
            if (_bundles.Count > 0)
                Purchase(_bundles,_payment);
            else
            {
                int newPageIndex = shop.GetNextSubCategoryIndexWithCurrencyPurchasableElements();
                if (newPageIndex >= 0)
                    shopView.scrollRect.ChangePage(newPageIndex);
                else
                {
                    shopView.scrollRect.ChangePage(Int32.MaxValue);
                }
            }
        }

        private void Purchase(List<Skin> _skins, Payment _payment)
        {
            if (_payment != null && _payment.IsAvailable)
            {
                //TODO Add code here if you want to do something before the item is purchased

                if (_skins.Count > 0)
                {
                    Skin skin = _skins.GetRandomValue();
                    
                    if (_payment is PaymentCurrency paymentCurrency)
                    {
                        paymentCurrency.Purchase();
                    }

                    LaunchRSA(_skins, skin);
                }
            }
            else
            {
                //TODO Add code here if you want to do something if the user can't buy with _payment
                Debug.Log("can't buy item");
            }
        }

        private void Purchase(List<Bundle> _bundles, Payment _payment)
        {
            if (_payment != null && _payment.IsAvailable)
            {
                //TODO Add code here if you want to do something before the bundle is purchased

                if (_bundles.Count > 0)
                {
                    Bundle bundle = _bundles.GetRandomValue();
                    if (bundle != null)
                    {
                        if (_payment is PaymentCurrency paymentCurrency)
                        {
                            paymentCurrency.Purchase();
                            foreach (PackContent content in bundle.contents)
                            {
                                Item item = ItemManager.Instance.GetItem(content.id);
                                
                                if (item == null)
                                    continue;

                                item.Collect(content.amount);
                            }
                        }
                    }
                }
            }
            else
            {
                //TODO Add code here if you want to do something if the user can't buy with _payment
                Debug.Log("can't buy item");
            }
        }

        /// <summary>
        /// Wrapper for the Random Selection Animation
        /// </summary>
        /// <param name="_skins"></param>
        /// <param name="_skin"></param>
        private async void LaunchRSA(List<Skin> _skins, Skin _skin)
        {
            await LaunchRSAAsync(_skins, _skin);
        }

        /// <summary>
        /// Launch the "Random Selection Animation" in async mode with the list of skin _skins and the result skin _skin
        /// </summary>
        /// <param name="_skins"></param>
        /// <param name="_skin"></param>
        /// <returns></returns>
        private async Task LaunchRSAAsync(List<Skin> _skins, Skin _skin)
        {
            shop.UpdatePayment();
            shopView.EnableInteractivity(false);
            Queue<Skin> randomSelectionSkins = new Queue<Skin>();
            
            if (_skins.Count > 1)
            {
                int nbIteration = (int) Random.Range(numberOfRandomIteration * (1f - coeffRandomIteration),
                    numberOfRandomIteration * (1f + coeffRandomIteration));
                
                Skin skin = null;

                for (int i = 0; i < nbIteration - 1; i++)
                {
                    List<Skin> localSkins = new List<Skin>(_skins);
                    localSkins.Remove(skin);
                    skin = localSkins.GetRandomValue();
                    randomSelectionSkins.Enqueue(skin);
                }
            }

            randomSelectionSkins.Enqueue(_skin);

            try
            {
                while (randomSelectionSkins.Count > 0)
                {
                    shopView.SelectSkin(randomSelectionSkins.Dequeue());
                    await Task.Delay(selectionMaxDelay / (randomSelectionSkins.Count + 1));
                }

                skinManager.CollectSkin(_skin);
                shopView.SelectToggle(_skin);

                await Task.WhenAny(OnSkinAcquiredInvoker(), Task.Delay(lastFocusDuration));
                shopView.SelectSkin(null);
                shopView.EnableInteractivity();
            }
            catch
            {
                skinManager.CollectSkin(_skin);
                BinaryPrefs.ForceSave();
                throw;
            }
        }

        private async Task OnSkinAcquiredInvoker()
        {
            if (OnSkinAcquired != null)
                await OnSkinAcquired();
            else
            {
                await Task.Delay(lastFocusDuration);
            }
        }
    }
}