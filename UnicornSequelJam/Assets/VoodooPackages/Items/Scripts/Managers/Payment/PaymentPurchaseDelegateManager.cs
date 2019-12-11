//using UnityEngine;
//#if VOODOO_SAUCE
//using UnityEngine.Purchasing;
//#endif

namespace VoodooPackages.Tech.Items
{
//#if VOODOO_SAUCE
//	public class PaymentPurchaseDelegateManager : SingletonMB<PaymentPurchaseDelegateManager>, IPurchaseDelegate
//	{
//		public bool IsInitialized { private set; get; }
//		public delegate void InitializeEvent();
//		public InitializeEvent onInitialize;
//
//		private Item shopItemData;
//
//		public void Awake()
//		{
//			VoodooSauce.RegisterPurchaseDelegate(this);
//		}
//
//		public void Purchase(Item _shopItemData, Payment _payment)
//		{
//			shopItemData = _shopItemData;
//			VoodooSauce.Purchase(_payment.productId);
//		}
//
//		public string GetProductPrice(string _productId)
//		{
//			return IsInitialized ? VoodooSauce.GetLocalizedProductPrice(_productId) : "loading...";
//		}
//
//		public void OnInitializeSuccess()
//		{
//			IsInitialized = true;
//			onInitialize?.Invoke();
//			onInitialize = null;
//		}
//
//		public void OnInitializeFailure(InitializationFailureReason reason)
//		{
//			Debug.Log(string.Format("Error while initializing store: {0}", reason));
//			IsInitialized = true;
//			onInitialize?.Invoke();
//			onInitialize = null;
//		}
//
//		public void OnPurchaseComplete(string productId)
//		{
//			if (shopItemData != null)
//			{
//				shopItemData.IsCollected = true;
//				shopItemData = null;
//			}
//		}
//
//		public void OnPurchaseFailure(string productId, PurchaseFailureReason reason)
//		{
//			if (shopItemData != null)
//			{
//				Debug.Log(string.Format("Error while purchasing {0}. Bundle id provided: {1}, reason: {2}", shopItemData, productId, reason));
//				shopItemData = null;
//			}
//		}
//	}
//#else
	public class PaymentPurchaseDelegateManager : SingletonMB<PaymentPurchaseDelegateManager>{}
//#endif
}