using System;
using System.IO;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using VoodooPackages.Tech;
using VoodooPackages.Tech.Items;

namespace VoodooPackages.Tool.Shop
{
    public abstract class SubCategoryData : ScriptableObject
	{
		//Server side
		public int    id;
		public string title;
		[HideInInspector] public Payment payment;
		
		//Gameplay side
		public uint line;
		public uint column;
		public SubCategoryVisual prefab;
		
		private const string DataPath = "Assets/Resources/Data/Payment";

		/// <summary>
		/// Instantiate the prefab of the subcategory under _parent and initialize it with the related data.
		/// Returns the instance of the prefab.
		/// </summary>
		/// <param name="_parent"></param>
		/// <returns></returns>
		public SubCategoryVisual AddSubCategory(Transform _parent)
		{
			SubCategoryVisual subCategoryVisual = Instantiate(prefab, _parent);
			subCategoryVisual.Initialize(this, _parent);

			return subCategoryVisual;
		}
		
		protected virtual void Reset()
		{
			id = Guid.NewGuid().GetHashCode();
			title = name;
			prefab = null;
			CreatePaymentInstance(typeof(PaymentCurrency));
			line = 0;
			column = 0;
		}

		/// <summary>
		/// Create a new instance of the payment type as _type.
		/// By default, it will create a PaymentCurrency instance.
		/// </summary>
		/// <param name="_type"></param>
		public void CreatePaymentInstance(Type _type)
		{
#if UNITY_EDITOR
			string path = Path.Combine(DataPath, "Payment.asset");
			if (payment == null)
			{
				if (!Directory.Exists(DataPath))
					Directory.CreateDirectory(DataPath);
			}
			else
			{
				path = AssetDatabase.GetAssetPath(payment);
				AssetDatabase.DeleteAsset(path);
			}
                
			string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path);
			Payment newCreatedPayment = (Payment)CreateInstance(_type);
			payment = newCreatedPayment;

			AssetDatabase.CreateAsset(newCreatedPayment, assetPathAndName);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
#endif
		}

		/// <summary>
		/// Load the previously saved payment
		/// </summary>
		public void LoadPayment()
		{
			SavedPayment savedPayment = BinaryPrefs.GetClass<SavedPayment>(payment.id);
			if (savedPayment != null)
			{
				payment.LoadFrom(savedPayment);
			}
		}

		/// <summary>
		/// Method used to do an action on all the items contained in the subcategory
		/// </summary>
		/// <param name="_action"></param>
		public abstract void DoOnAllItems(Action<Item> _action);
		

		/// <summary>
		/// Initialize the SubCategoryData with the values of the _subCategoryDataServer.
		/// </summary>
		/// <param name="subCategoryServer"></param>
		public virtual void Initialize(SubCategoryServer subCategoryServer)
		{
			id    = subCategoryServer.id;
			title = subCategoryServer.title;
			Payment newPayment = Resources.LoadAll<Payment>("Data").FirstOrDefault(x => x.id == subCategoryServer.paymentId);
			if (newPayment != null)
				payment = newPayment;
		}
	}
}