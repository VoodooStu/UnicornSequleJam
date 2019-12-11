using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VoodooPackages.Tech;
using VoodooPackages.Tech.Items;

//using Voodoo.Sauce.Internal.RemoteConfig;

namespace VoodooPackages.Tool.Shop
{
    public class ShopView : View<ShopView>
	{
		public ScrollRectSwipe scrollRect;
		public Transform subCategoryDotZoneParent;
		public GameObject header;
		public GameObject paymentArea;
		public GameObject chestRoomArea;
		public GameObject comingSoonArea;
		public PaymentPicker paymentPicker;
		public RVButton rvButton;
		public Transform previewPlaceholder3D;
		public Image preview2D;
		
		#region Cache variables
		private List<CategoryVisual> categories;
		private TextMeshProUGUI chestRoomText;
		private TextMeshProUGUI comingSoonText;
		private TextMeshProUGUI titleText;
		private Payment payment;
		private Canvas canvas;
		private CanvasGroup canvasGroup;
		private new Camera camera;
		private GameObject previewInstance;
		private Shop shop;
		#endregion
		
		protected override void Init()
		{
			base.Init();
			
			OnTransitionFinished += () =>
			{
				if (!m_Visible)
					canvas.gameObject.SetActive(false);
			};

			OnShow += () =>
			{
				canvas.gameObject.SetActive(m_Visible);
				StartCoroutine(PopPreview(Vector3.one, true));
			};
			
			OnHide += FirstHide;
			OnHide += () =>
			{
				if (isActiveAndEnabled)
					StartCoroutine(PopPreview(Vector3.zero, false));
			};
		}
		
		private void FirstHide()
		{
			canvas.gameObject.SetActive(m_Visible);
			camera.enabled = true;
			OnHide -= FirstHide;
		}
		
		/// <summary>
		/// Create and initialize the shop view based on _shopData.
		/// This method is called automatically in the Initialize method of the Shop.
		/// </summary>
		/// <param name="_shopData"></param>
		public void CreateVisualShop(ShopData _shopData)
		{
			categories = new List<CategoryVisual>();
			chestRoomText = chestRoomArea.GetComponentInChildren<TextMeshProUGUI>(true);
			comingSoonText = comingSoonArea.GetComponentInChildren<TextMeshProUGUI>(true);
			titleText = header.GetComponentInChildren<TextMeshProUGUI>(true);
			canvas = GetComponentInParent<Canvas>();
			canvasGroup = GetComponent<CanvasGroup>();
			camera = canvas.GetComponentInChildren<Camera>(true);
			shop = Shop.Instance;

			if (_shopData == null)
				return;
			
			if (rvButton != null)
				rvButton.UpdateValue(_shopData.RVBonusCurrency, _shopData.RVBonusValue);

			if (paymentPicker != null)
				paymentPicker.buttonPicker.onClick.AddListener(RequestPurchase);

			if (scrollRect != null)
			{
//					List<CategoryDataServer> categoryDataServers = RemoteConfigManager.Instance.List<CategoryDataServer>();

				for (var i = 0; i < _shopData.categories.Count; i++)
				{
					CategoryData categoryData = _shopData.categories[i];

//						CategoryDataServer categoryDataServer = categoryDataServers.Find(x => x.id == categoryData.id);
//						if (categoryDataServer != null)
//							categoryData.Initialize(categoryDataServer);

					CategoryVisual categoryVisual = categoryData.AddShopCategory(scrollRect, subCategoryDotZoneParent);
					categories.Add(categoryVisual);
				}
			}
			
			StartCoroutine(ResizeSubCategories(_shopData));
		}

		private void RequestPurchase()
		{
			shop.RequestPurchase();
		}
		
		private IEnumerator ResizeSubCategories(ShopData _shopData)
		{
			yield return new WaitForEndOfFrame();
			for (int i = 0; i < categories.Count; i++)
			{
				CategoryVisual categoryVisual = categories[i];
				if (categoryVisual != null)
					categoryVisual.ResizeSubCategories(_shopData.line, _shopData.column, _shopData.categories[i]);
			}
		}

		/// <summary>
		/// Display the chosen category _categoryData.
		/// Show the Shop if it's not visible.
		///	Will automatically update the preview with the currently used skin.
		///	Will directly move to the page corresponding to the skin's subcategory
		/// </summary>
		/// <param name="_categoryData"></param>
		public void ShowCategory(CategoryData _categoryData)
		{
			if (categories == null || _categoryData == null)
				return;

			bool isAlreadyInShop = m_Visible;
			
			if (!isAlreadyInShop)
				Show();

			for (var i = 0; i < categories.Count; i++)
			{
				CategoryVisual categoryVisual = categories[i];

				if (categoryVisual.name == _categoryData.name)
					categoryVisual.Show();
				else
					categoryVisual.Hide();
			}
			
			if (isAlreadyInShop)
			{
				if (shop.currentlyUsedSkins.ContainsKey(_categoryData))
					UpdateSkinPreview(shop.currentlyUsedSkins[_categoryData]);
				else
					UpdateSkinPreview(null);
			}

			int usedItemPageIndex = 0;
			
			for (var i = 0; i < _categoryData.subCategories.Count; i++)
			{
				SubCategoryData subCategoryData = _categoryData.subCategories[i];

				if (subCategoryData is SubCategorySkinData subCategorySkinData)
				{
					if (subCategorySkinData.skins.Exists(skin => skin.IsUsed))
					{
						usedItemPageIndex = i;
						break;
					}
				}
				else if (subCategoryData is SubCategoryBundleData subCategoryBundleData)
				{
					//TODO
				}
			}

			scrollRect.ChangePage(usedItemPageIndex, true);
		}

		/// <summary>
		/// Update the shop view based on _shop
		/// </summary>
		/// <param name="shopData"></param>
		public void UpdateShopView(ShopData shopData)
		{
			if (shopData == null || categories == null || categories.Count == 0)
				return;

			for (int i = 0; i < categories.Count; i++)
			{
				CategoryVisual categoryVisual = categories[i];
				if (categoryVisual)
					categoryVisual.UpdateCategory(shopData.categories[i]);
			}
		}

		/// <summary>
		/// Update the skin preview based on _skin
		/// Will launch an appearing animation for the 3D preview
		/// </summary>
		/// <param name="_skin"></param>
		public void UpdateSkinPreview(Skin _skin)
		{
			Vector3 startScale = Vector3.zero;
			float startDuration = 0f;

			if (previewInstance != null)
			{
				Destroy(previewInstance);
				startScale = new Vector3(0.8f,0.8f,0.8f);
				startDuration = m_FadeInDuration * 0.8f;
			}
			
			if (_skin == null)
			{
				previewPlaceholder3D.gameObject.SetActive(false);
				preview2D.gameObject.SetActive(false);
				return;
			}

			bool displayPlaceholder3D = _skin.previewPrefab != null && previewPlaceholder3D != null;
			bool displayPlaceholder2D = !displayPlaceholder3D && preview2D != null;

			if (previewPlaceholder3D != null)
				previewPlaceholder3D.gameObject.SetActive(displayPlaceholder3D);
			
			if (preview2D != null)
				preview2D.gameObject.SetActive(displayPlaceholder2D);

			if (displayPlaceholder3D)
			{
				previewInstance = Instantiate(_skin.previewPrefab, previewPlaceholder3D);
				previewInstance.transform.localScale = startScale;
				if (isActiveAndEnabled)
					StartCoroutine(PopPreview(Vector3.one, true, startDuration));
			}

			if (displayPlaceholder2D)
			{
				preview2D.sprite = _skin.icon;
				preview2D.color = _skin.color;
			}
		}

		/// <summary>
		/// Show or Hide the preview by poping it up or make it fade away
		/// </summary>
		/// <param name="_newScale"></param>
		/// <param name="_show"></param>
		/// <param name="_startDuration"></param>
		/// <returns></returns>
		private IEnumerator PopPreview(Vector3 _newScale, bool _show, float _startDuration = 0f)
		{
			float duration = _show ? m_FadeInDuration : m_FadeOutDuration;
			if (previewInstance != null)
			{
				float elapsedTime = Mathf.Clamp(_startDuration,0,duration);
				Vector3 startScale = previewInstance.transform.localScale;
				
				if (startScale == _newScale)
					yield break;
				
				while (elapsedTime < duration)
				{
					float time = elapsedTime / duration;
					float currentValue = time * time;
					previewInstance.transform.localScale = Vector3.Lerp(startScale, _newScale, currentValue);
					elapsedTime += Time.deltaTime;
					yield return null;
				}

				if (previewInstance != null)
					previewInstance.transform.localScale = _newScale;
			}
		}
		
		#region Payment

		/// <summary>
		/// Display the payment based on _payment and enable/disable the corresponding areas
		/// </summary>
		/// <param name="_payment"></param>
		public void DisplayPayment(Payment _payment)
		{
			if (_payment == null)
				return;
			
			payment = _payment;
			if (_payment is PaymentCurrency paymentCurrency)
			{
				DisplayPaymentArea();
				if (paymentPicker != null)
					paymentPicker.Display(paymentCurrency);
				if (rvButton != null)
					rvButton.UpdateInteractivity();
			}
			else if (_payment is PaymentChestRoom paymentChestRoom)
			{
				DisplayChestRoomArea();
				if (chestRoomText != null)
					chestRoomText.text = paymentChestRoom.message;
			}
			else if (_payment is PaymentComingSoon paymentComingSoon)
			{
				DisplayComingSoonArea();
				if (comingSoonText != null)
					comingSoonText.text = paymentComingSoon.message;
			}
		}

		/// <summary>
		/// Reload the current payment
		/// </summary>
		public void UpdatePayment()
		{
			DisplayPayment(payment);
		}

		private void DisplayPaymentArea()
		{
			if (paymentArea)
				paymentArea.SetActive(true);
			
			if (comingSoonArea)
				comingSoonArea.SetActive(false);
				
			if (chestRoomArea)
				chestRoomArea.SetActive(false);
		}

		private void DisplayComingSoonArea()
		{
			if (paymentArea)
				paymentArea.SetActive(false);
			
			if (comingSoonArea)
				comingSoonArea.SetActive(true);
				
			if (chestRoomArea)
				chestRoomArea.SetActive(false);
		}

		private void DisplayChestRoomArea()
		{
			if (paymentArea)
				paymentArea.SetActive(false);
			
			if (comingSoonArea)
				comingSoonArea.SetActive(false);
				
			if (chestRoomArea)
				chestRoomArea.SetActive(true);
		}
		
		#endregion

		#region Helper
		
		/// <summary>
		/// Returns the index of the current category
		/// </summary>
		/// <returns></returns>
		public int GetCurrentCategoryIndex()
		{
			int categoryIndex = categories.FindIndex(x => x.isActiveAndEnabled);

			return categoryIndex;
		}

		/// <summary>
		/// Returns the index of the current subCategory
		/// </summary>
		/// <returns></returns>
		public int GetCurrentSubCategoryIndex()
		{
			int subCategoryIndex = scrollRect.CurrentPage;

			return subCategoryIndex;
		}

		/// <summary>
		/// Set the shopView's CanvasGroup interactable variable to _enable.
		/// Also set the scrollRectSwipe interactable variable to _enable.
		/// </summary>
		/// <param name="_enable"></param>
		public void EnableInteractivity(bool _enable = true)
		{
			canvasGroup.interactable = _enable;
			scrollRect.m_Interactable = _enable;
		}

		/// <summary>
		/// Get the _skin toggle and set it's isOn variable to true.
		/// </summary>
		/// <param name="_skin"></param>
		public void SelectToggle(Skin _skin)
		{
			categories.Find(x => x.isActiveAndEnabled).SelectToggle(_skin);
		}

		/// <summary>
		/// Set the currently selected skin to _skin
		/// </summary>
		/// <param name="_skin"></param>
		public void SelectSkin(Skin _skin)
		{
			int categoryIndex = GetCurrentCategoryIndex();
			categories[categoryIndex].SelectSkin(_skin);
		}

		/// <summary>
		/// Update the subCategory title
		/// </summary>
		public void UpdateSubCategoryTitle()
		{
			SubCategoryData subCategoryData = shop.GetCurrentSubCategoryData();
			
			if (titleText != null)
				titleText.text = subCategoryData.title;
		}
		
		#endregion
	}
}
