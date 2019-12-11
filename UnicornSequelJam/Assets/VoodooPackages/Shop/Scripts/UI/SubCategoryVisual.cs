using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoodooPackages.Tech;
using VoodooPackages.Tech.Items;

namespace VoodooPackages.Tool.Shop
{
    public class SubCategoryVisual : MonoBehaviour
	{
		public DynamicGridLayoutGroupCanvas dynamicGridLayoutGroupCanvas;
		public Transform content;

		//Cache
		private List<SkinVisual> skinVisuals;
		private RectTransform rectTransform;
		private RectTransform canvasRectTransform;
		private RectTransform scrollRectTransform;
		private Shop shop;
		
		/// <summary>
		/// Resize the component RectTransform to match the parent canvas one
		/// </summary>
		[ContextMenu("Resize component")]
		public void ResizeComponent()
		{
			if (canvasRectTransform == null)
				canvasRectTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
			
			if (rectTransform == null)
				rectTransform = GetComponent<RectTransform>();

			Rect canvasRect = canvasRectTransform.rect;
			rectTransform.sizeDelta = new Vector2(canvasRect.width,canvasRect.height);
		}

		/// <summary>
		/// Initialize the subCategory based on _subCategoryData
		/// </summary>
		/// <param name="_subCategoryData"></param>
		/// <param name="_categoryTransform"></param>
		public void Initialize(SubCategoryData _subCategoryData, Transform _categoryTransform)
		{
			if (shop == null)
				shop = Shop.Instance;
			
			_subCategoryData.LoadPayment();
			
			if (_subCategoryData is SubCategorySkinData subCategorySkinData)
			{
				skinVisuals = new List<SkinVisual>();	
				for (int i = 0; i < subCategorySkinData.skins.Count; i++)
				{
					Skin skin = subCategorySkinData.skins[i];
					if (skin != null && skin.enabled)
					{
						SkinVisual skinVisual = Instantiate(shop.shopData.skinVisualPrefab, content);
				
						ToggleGroup toggleGroup = _categoryTransform.GetComponent<ToggleGroup>();
						skinVisual.Initialize(skin, toggleGroup);
						skinVisuals.Add(skinVisual);
					}
				}
			}
			else if (_subCategoryData is SubCategoryBundleData subCategoryBundleData)
			{
				//TODO
			}
		}

		/// <summary>
		/// Modify the DynamicGridLayoutGroupCanvas values to match the default
		/// values _line & _column or the one from their _subCategoryData
		/// </summary>
		/// <param name="_line"></param>
		/// <param name="_column"></param>
		/// <param name="_subCategoryData"></param>
		/// <param name="_scrollRect"></param>
		public void ResizeContent(uint _line, uint _column, SubCategoryData _subCategoryData, ScrollRectSwipe _scrollRect)
		{
			if (dynamicGridLayoutGroupCanvas)
			{
				if (_subCategoryData == null || _subCategoryData.line == 0 && _subCategoryData.column == 0)
				{
					dynamicGridLayoutGroupCanvas.line = _line;
					dynamicGridLayoutGroupCanvas.column = _column;
				}
				else
				{
					dynamicGridLayoutGroupCanvas.line = _subCategoryData.line != 0 ? _subCategoryData.line : _line;
					dynamicGridLayoutGroupCanvas.column = _subCategoryData.column != 0 ? _subCategoryData.column : _column;
				}
				
				if (scrollRectTransform == null)
					scrollRectTransform = _scrollRect.GetComponent<RectTransform>();

				if (_scrollRect != null)
					dynamicGridLayoutGroupCanvas.containerRectTransform = scrollRectTransform;
			}
		}

		/// <summary>
		/// Update the Subcategory based on _subCategoryData
		/// </summary>
		/// <param name="_subCategoryData"></param>
		public void UpdateSubCategory(SubCategoryData _subCategoryData)
		{
			if (_subCategoryData is SubCategorySkinData subCategorySkinData)
			{
				for (int i = 0; i < skinVisuals.Count; i++)
				{
					SkinVisual skinVisual = skinVisuals[i];
					skinVisual.UpdateSkinVisual(subCategorySkinData.skins[i]);
					
					if(_subCategoryData.payment is PaymentComingSoon)
						skinVisual.EnableInteractivity(false);
				}
			}
			else if (_subCategoryData is SubCategoryBundleData subCategoryBundleData)
			{
				//TODO
			}
		}
		
		/// <summary>
		/// Visually set the selected skin to _skin
		/// </summary>
		/// <param name="_skin"></param>
		public void SelectSkin(Skin _skin)
		{
			foreach (SkinVisual skinVisual in skinVisuals)
			{
				Transform skinVisualTransform = skinVisual.transform;
				
				if (_skin != null && skinVisual.id == _skin.id)
					skinVisualTransform.localScale = new Vector3(1.1f, 1.1f, 1);
				else
					skinVisualTransform.localScale = new Vector3(1, 1, 1);
			}
		}

		/// <summary>
		/// Returns the skinVisual associated with the skin _skin
		/// </summary>
		/// <param name="_skin"></param>
		/// <returns></returns>
		public SkinVisual GetSkinVisual(Skin _skin)
		{
			SkinVisual skinVisual = skinVisuals.Find(x => x.id == _skin.id);
			return skinVisual;
		}
	}
}