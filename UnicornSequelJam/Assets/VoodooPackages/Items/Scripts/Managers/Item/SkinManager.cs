using System;
using System.Collections.Generic;
using System.Linq;

namespace VoodooPackages.Tech.Items
{
	public class SkinManager : SingletonMB<SkinManager>
	{
		private List<Skin>  skins;
		private ItemManager itemManager;

		public event Skin.SkinEvent OnSkinPurchasedSuccessfully;
		public event Skin.SkinEvent OnSkinSelected;
		
		#region Initialization
		
		private void Start()
		{
			itemManager = ItemManager.Instance;
			
			if (itemManager.IsInitialized)
				Initialize();
			else
				itemManager.onInitializeSuccess.AddListener(Initialize);
		}

		/// <summary>
		/// Initialize the SkinManager
		/// </summary>
		private void Initialize()
		{
			skins = itemManager.GetItems(x => x is Skin).OfType<Skin>().ToList();
			skins.RemoveAll(x => !x.enabled);

			foreach (Skin skin in skins)
			{
				skin.onSkinPurchasedSuccessfully += OnSkinPurchaseSuccessfullyInvoker;
				skin.onSkinSelected += OnSkinSelectedInvoker;
				
				if (skin.unlockAtStart && !skin.IsCollected)
					CollectSkin(skin);
			}
		}

		private void OnSkinPurchaseSuccessfullyInvoker(Skin _skin)
		{
			OnSkinPurchasedSuccessfully?.Invoke(_skin);
		}

		private void OnSkinSelectedInvoker(Skin _skin)
		{
			OnSkinSelected?.Invoke(_skin);
		}

		private void OnDestroy()
		{
			if (skins == null)
				return;
			
			foreach (Skin skin in skins)
			{
				skin.onSkinPurchasedSuccessfully -= OnSkinPurchaseSuccessfullyInvoker;
				skin.onSkinSelected -= OnSkinSelectedInvoker;
			}
		}

		#endregion

		/// <summary>
		/// Return all the skins
		/// </summary>
		/// <returns></returns>
		public List<Skin> GetSkins()
		{
			return skins;
		}

		/// <summary>
		/// Returns all the skins matching the condition _condition
		/// </summary>
		/// <param name="_id"></param>
		/// <returns></returns>
		public List<Skin> GetSkins(Predicate<Skin> _condition)
		{
			return skins.FindAll(_condition);
		}
		
		/// <summary>
		/// Returns the skin with id _id
		/// </summary>
		/// <param name="_id"></param>
		/// <returns></returns>
		public Skin GetSkin(int _id)
		{
			return skins.Find(x => x.id == _id);
		}
		

		/// <summary>
		/// Returns the first skin matching the condition _condition
		/// </summary>
		/// <param name="_id"></param>
		/// <returns></returns>
		public Skin GetSkin(Predicate<Skin> _condition)
		{
			return skins.Find(_condition);
		}
		
		/// <summary>
		/// Collect the skin _skin
		/// </summary>
		/// <param name="_skin"></param>
		public void CollectSkin(Skin _skin)
		{
			if (_skin != null)
				_skin.IsCollected = true;
		}

		/// <summary>
		/// Collect all the skins enabled in your game
		/// </summary>
		public void CollectAllSkins()
		{
			DoOnAllSkins(CollectSkin);
		}

		/// <summary>
		/// Lose the skin _skin
		/// </summary>
		/// <param name="_skin"></param>
		public static void LoseSkin(Skin _skin)
		{
			if (_skin != null && _skin.IsCollected)
			{
				_skin.IsCollected = false;
				_skin.IsUsed      = false;
			}
		}

		/// <summary>
		/// Lose all the skins enabled in your game
		/// </summary>
		/// <param name="_areYouSure"></param>
		public void LoseAllSkins(bool _areYouSure = false)
		{
			if (_areYouSure)
				DoOnAllSkins(LoseSkin);
		}

		private void DoOnAllSkins(Action<Skin> _skinAction)
		{
			foreach (var item in skins)
			{
				_skinAction(item);
			}
		}
	}
}
