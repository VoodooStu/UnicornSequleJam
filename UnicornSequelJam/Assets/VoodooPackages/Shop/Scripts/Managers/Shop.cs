using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using VoodooPackages.Tech;
using VoodooPackages.Tech.Items;

namespace VoodooPackages.Tool.Shop
{
    public class Shop : SingletonMB<Shop>
	{
		public ShopData shopData;
		public Dictionary<CategoryData, Skin> currentlyUsedSkins;

		public bool initializeOnAwake = true;
		public UnityEvent onInitializeSuccess;
		public bool IsInitialized { private set; get; }
		
		public delegate void ItemPaymentEvent(SubCategoryData _subCategoryData, Payment _payment);
		public ItemPaymentEvent OnPurchaseRequested;
		public ShopPurchaseHelper.RewardAnimationEvent OnSkinAcquired;
		
		//Cache
		private ShopView shopView;
		private SkinManager skinManager;
		private CurrencyManager currencyManager;
		private ShopPurchaseHelper shopPurchaseHelper;
		
		private void Awake()
		{
			if (initializeOnAwake)
				Initialize();

			Application.targetFrameRate = 60;
		}

		#region Initialization
		
		/// <summary>
		/// If you deactivate the InitializeOnAwake boolean,
		/// you will need to call this method to initialize the ShopManager
		/// </summary>
		public void Initialize()
		{
			shopView = ShopView.Instance;
			skinManager = SkinManager.Instance;
			currencyManager = CurrencyManager.Instance;
			shopPurchaseHelper = ShopPurchaseHelper.Instance;
			
			currentlyUsedSkins = InitSelectedSkins();
			
			shopView.CreateVisualShop(shopData);
			
			StartCoroutine(DelayedInitialization());
		}

		private IEnumerator DelayedInitialization()
		{
			yield return new WaitForSeconds(1);

			UpdateShop();
			
			skinManager.OnSkinPurchasedSuccessfully += OnPurchaseSuccessful;
			skinManager.OnSkinSelected += OnSkinSelected;
			currencyManager.OnCurrencyChanged += OnCurrencyChanged;
			shopPurchaseHelper.OnSkinAcquired += OnSkinAcquiredInvoker;

			if (!IsInitialized)
			{
				Debug.Log("ShopManager initialization finished successfully");
				IsInitialized = true;
				onInitializeSuccess?.Invoke();
				Hide();
				
				if (currentlyUsedSkins.Count > 0)
					UpdateSkinPreview(currentlyUsedSkins.FirstOrDefault().Value);
				else
					UpdateSkinPreview(null);
			}
		}

		private void OnDestroy()
		{
			if (skinManager != null)
			{
				skinManager.OnSkinPurchasedSuccessfully -= OnPurchaseSuccessful;
				skinManager.OnSkinSelected -= OnSkinSelected;
			}
			
			if (currencyManager != null)
				currencyManager.OnCurrencyChanged -= OnCurrencyChanged;
			
			if (shopPurchaseHelper != null)
				shopPurchaseHelper.OnSkinAcquired -= OnSkinAcquiredInvoker;
		}

		private void OnPurchaseSuccessful(Skin _skin)
		{
			UpdateShop();
			UpdatePayment();
			InitSelectedSkins();
		}

		private void OnSkinSelected(Skin _skin)
		{
			UpdateShop();
			UpdateSelectedSkins(_skin);
			UpdateSkinPreview(_skin);
		}
		
		private void OnCurrencyChanged(Currency _currency)
		{
			UpdatePayment();
		}

		private Task OnSkinAcquiredInvoker()
		{
			return OnSkinAcquired?.Invoke();
		}

		private Dictionary<CategoryData, Skin> InitSelectedSkins()
		{
			Dictionary<CategoryData, Skin> usedSkins = new Dictionary<CategoryData, Skin>();
				
			foreach (CategoryData categoryData in shopData.categories)
			{
				if (categoryData.id == 0)
				{
					categoryData.id = System.Guid.NewGuid().GetHashCode();
				}
				
				foreach (SubCategoryData subCategoryData in categoryData.subCategories)
				{
					if (subCategoryData.id == 0)
					{
						subCategoryData.id = System.Guid.NewGuid().GetHashCode();
					}

					if (subCategoryData is SubCategorySkinData subCategorySkinData)
					{
						Skin usedSkin = subCategorySkinData.skins.Find(x => x.IsUsed);
						if (usedSkin != null)
						{
							usedSkins.Add(categoryData, usedSkin);
							break;
						}
					}
				}
			}

			return usedSkins;
		}
		
		private void UpdateSelectedSkins(Skin _skin)
		{
			foreach (CategoryData shopCategoryData in shopData.categories)
			{
				List<SubCategorySkinData> subCategorySkinDatas = shopCategoryData.subCategories.Cast<SubCategorySkinData>().ToList();

				if (subCategorySkinDatas.Exists(subCategory => subCategory.skins.Contains(_skin)))
				{
					if (!currentlyUsedSkins.ContainsKey(shopCategoryData))
						currentlyUsedSkins.Add(shopCategoryData, _skin);
					else
						currentlyUsedSkins[shopCategoryData] = _skin;
				}
			}
		}

		#endregion

		#region Access Simplifiers

		/// <summary>
		/// Update the visual elements of the shop
		/// </summary>
		public void UpdateShop()
		{
			shopView.UpdateShopView(shopData);
		}
		
		/// <summary>
		/// Update the skin preview to display the skin _skin
		/// </summary>
		/// <param name="_skin"></param>
		public void UpdateSkinPreview(Skin _skin)
		{
			shopView.UpdateSkinPreview(_skin);
		}
		
		/// <summary>
		/// Update the payment value, interactivity, visual,...
		/// </summary>
		public void UpdatePayment()
		{
			shopView.UpdatePayment();
		}
		
		/// <summary>
		/// Display the category of your shop associated with the value _categoryData.
		/// Display the first category if _categoryData is null
		/// </summary>
		/// <param name="_categoryData"></param>
		public void ShowCategory(CategoryData _categoryData)
		{
			CategoryData displayedCategoryData = _categoryData != null ? _categoryData : shopData.categories[0];
			shopView.ShowCategory(displayedCategoryData);
		}
		
		/// <summary>
		/// Calls ShowCategory with a null value.
		/// </summary>
		public void Show()
		{
			ShowCategory(shopData.categories[0]);
		}

		/// <summary>
		/// Hide the View
		/// </summary>
		public void Hide()
		{
			shopView.Hide();
		}
		#endregion
		
		/// <summary>
		/// Returns a list of all sub categories with type T in the category _category. 
		/// </summary>
		/// <param name="_category"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public List<T> GetSubCategoriesOfType<T>(CategoryData _category) where T : SubCategoryData
		{
			List<T> subCategories = new List<T>();
			
			if (shopData.categories.Contains(_category))
				subCategories = _category.subCategories.FindAll(subCategory => subCategory is T).OfType<T>().ToList();
			
			return subCategories;
		}

		/// <summary>
		/// Get a random available skin in _subCategoryDataSkin. (null if there is none)
		/// </summary>
		/// <param name="_skin"></param>
		/// <returns></returns>
		public SubCategorySkinData GetSubCategoryDataSkin(Skin _skin)
		{
			for (int i = 0; i < shopData.categories.Count; i++)
			{
				CategoryData category = shopData.categories[i];
				for (int j = 0; j < category.subCategories.Count; j++)
				{
					SubCategoryData subCategory = category.subCategories[j];
					if (subCategory is SubCategorySkinData subCategoryDataSkin && subCategoryDataSkin.skins.Contains(_skin))
						return subCategoryDataSkin;
				}
			}

			return null;
		}

		
		//TODO : Modify the structure so that Sub Categories implement an interface IPurchasable saying that we can purchase item in it.
		/// <summary>
		/// Returns the index of the next sub category with purchasable elements inside.
		/// Returns -1 if there is none.
		/// </summary>
		/// <returns></returns>
		public int GetNextSubCategoryIndexWithCurrencyPurchasableElements()
		{
			int categoryIndex = shopView.GetCurrentCategoryIndex();
			
			List<SubCategoryData> subCategoryDatas = shopData.categories[categoryIndex].subCategories;
			subCategoryDatas = subCategoryDatas.FindAll(subCategory => subCategory.payment.GetType() == typeof(PaymentCurrency)).ToList();

			for (var i = subCategoryDatas.Count - 1; i >= 0; i--)
			{
				SubCategoryData subCategoryData = subCategoryDatas[i];
				
				if (subCategoryData is SubCategorySkinData subCategorySkinData)
				{
					if (!subCategorySkinData.skins.Exists(item => !item.IsCollected))
						subCategoryDatas.Remove(subCategorySkinData);
				}
				else if (subCategoryData is SubCategoryBundleData subCategoryBundleData)
				{
					if (!subCategoryBundleData.bundles.Exists(bundle => bundle.AmountAvailable > 0))
						subCategoryDatas.Remove(subCategoryBundleData);
				}
			}

			if (subCategoryDatas.Count > 0)
			{
//				subCategoryDatas = subCategoryDatas.OrderBy(x => (x.payment as PaymentCurrency).cost).ToList();

				int res = shopData.categories[categoryIndex].subCategories.IndexOf(subCategoryDatas[0]);
				return res;
			}

			return -1;
		}

		/// <summary>
		/// Returns the data associated with the current sub category
		/// </summary>
		/// <returns></returns>
		public SubCategoryData GetCurrentSubCategoryData()
		{
			int categoryIndex = shopView.GetCurrentCategoryIndex();
			int subCategoryIndex = shopView.GetCurrentSubCategoryIndex();
			
			SubCategoryData subCategoryData = shopData.categories[categoryIndex].subCategories[subCategoryIndex];
			
			return subCategoryData;
		}

		/// <summary>
		/// Get a random available skin in _subCategoryDataSkin.
		/// Returns null if there is none.
		/// </summary>
		/// <param name="subCategorySkinData"></param>
		/// <returns></returns>
		public Skin GetSkin(SubCategorySkinData subCategorySkinData)
		{
			Skin skin = null;

			foreach (var category in shopData.categories)
			{
				if (category.subCategories.Contains(subCategorySkinData))
				{
					List<Skin> subCategorySkins = new List<Skin>(subCategorySkinData.skins);
					subCategorySkins.RemoveAll(x => x.IsCollected);

                    if (subCategorySkins.Count <= 0)
                    {
                        continue;
                    }

					skin = subCategorySkins.GetRandomValue();
				}
			}

			return skin;
		}

		/// <summary>
		/// Request a purchase for a random element in the current sub category.
		/// Fires the OnPurchaseRequested event.
		/// </summary>
		public void RequestPurchase()
		{
			SubCategoryData currentSubCategoryData = GetCurrentSubCategoryData();
			OnPurchaseRequested?.Invoke(currentSubCategoryData, currentSubCategoryData.payment);
		}

		/// <summary>
		/// Call the LoseItem method from the ItemManager for all the items in the shop.
		/// Be careful with this since it will reset all the user purchases.
		/// </summary>
		/// <param name="_areYouSure"></param>
		public void UnBuyEverySkins(bool _areYouSure)
		{
			//Currently not working for bundles
			if (_areYouSure)
				shopData.DoOnAllItems(UnBuyItem);

			UpdateShop();
			UpdatePayment();
		}
		
		private void UnBuyItem(Bundle _bundle)
		{
			if (_bundle != null && _bundle.currentAmount > 0)
			{
				_bundle.currentAmount = 0;
				//TODO : UnBuyItem(items in bundle)
			}
		}

		private void UnBuyItem(Item _item)
		{
			if (_item != null && _item is Skin skin && skin.IsCollected)
			{
				SkinManager.LoseSkin(skin);
				
				//reduce cost value
				SubCategorySkinData subCategoryData = GetSubCategoryDataSkin(skin);
				if (subCategoryData != null && subCategoryData.payment is PaymentCurrency paymentCurrency)
				{
					if (paymentCurrency.NumberOfPurchaseDone > 0)
						paymentCurrency.NumberOfPurchaseDone--;
				}
			}
		}
	}
}
