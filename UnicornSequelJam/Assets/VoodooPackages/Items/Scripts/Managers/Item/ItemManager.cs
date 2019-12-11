using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace VoodooPackages.Tech.Items
{
	public class ItemManager : SingletonMB<ItemManager>
	{
		private List<Item>  items;
		private List<Pack>  packs;
		
		public bool         initializeOnAwake = true;
		public UnityEvent   onInitializeSuccess;
		public bool IsInitialized { private set; get; }

		public static event Pack.PackEvent OnPackCollectSuccessful;
		public static event Pack.PackEvent OnPackCollectFailed;
		
		private void Awake()
		{
			if (initializeOnAwake)
				Initialize();
		}

        private void Start()
        {
	        if (packs == null || packs.Count <= 0)
		        return;

	        for (int i = 0; i < packs.Count; i++)
            {
                packs[i].OnInitialized();
            }
        }

        private void OnDestroy()
        {
	        if (packs == null)
		        return;
	        
	        for (int i = 0; i < packs.Count; i++)
	        {
		        Pack pack = packs[i];
		        if (pack == null)
			        return;
		        
		        pack.onCollectSuccessful -= OnPackCollectSuccessfulInvoker;
		        pack.onCollectFailed -= OnPackCollectFailedInvoker;
	        }
        }

        /// <summary>
        /// Initialize the ItemManager
        /// This method is launched automatically if initializeOnAwake is set to true.
        /// </summary>
        public void Initialize()
		{
			if (!IsInitialized)
			{
				items = Resources.LoadAll<Item>("").ToList();
				packs = Resources.LoadAll<Pack>("").ToList();
				LoadItems();
			}
		}

		private void LoadItems()
		{
			if (items != null && items.Count > 0)
			{
				for (int i = 0; i < items.Count; i++)
				{
					Item item = items[i];
					SavedItem savedItem = BinaryPrefs.GetClass<SavedItem>(item.id.ToString());
					if (savedItem != null)
					{
						items[i].LoadFrom(savedItem);
					}
				}
			}
			else
			{
				Debug.LogWarning("There is no item in your game ! Please verify that everything is setup correctly", this);
			}
			
			LoadPack();

			if (!IsInitialized)
			{
				IsInitialized = true;
				onInitializeSuccess?.Invoke();
			}
		}

		private void LoadPack()
		{
			if (packs != null && packs.Count > 0)
			{
				for (int i = 0; i < packs.Count; i++)
				{
					Pack pack = packs[i];
					SavedPack savedPack = BinaryPrefs.GetClass<SavedPack>(pack.id.ToString());
					if (savedPack != null)
						packs[i].LoadFrom(savedPack);

					pack.onCollectSuccessful += OnPackCollectSuccessfulInvoker;
					pack.onCollectFailed += OnPackCollectFailedInvoker;
				}
            }
            else
			{
				Debug.LogWarning("There is no pack in your game ! Please verify that everything is setup correctly", this);
			}
		}

		private void OnPackCollectSuccessfulInvoker(Pack _pack)
		{
			OnPackCollectSuccessful?.Invoke(_pack);
		}

		private void OnPackCollectFailedInvoker(Pack _pack)
		{
			OnPackCollectFailed?.Invoke(_pack);
		}

		/// <summary>
		/// Return all the items
		/// </summary>
		/// <returns></returns>
		public List<Item> GetItems()
		{
			return items;
		}

		/// <summary>
		/// Return all the items matching the condition _condtion
		/// </summary>
		/// <param name="_condition"></param>
		/// <returns></returns>
		public List<Item> GetItems(Predicate<Item> _condition)
		{
			return items.FindAll(_condition);
		}
		
		/// <summary>
		/// Returns the item matching the condition _condition
		/// </summary>
		/// <param name="_id"></param>
		/// <returns></returns>
		public Item GetItem(Predicate<Item> _condition)
		{
			return items.Find(_condition);
		}

		/// <summary>
		/// Returns the item with id _id
		/// </summary>
		/// <param name="_id"></param>
		/// <returns></returns>
		public Item GetItem(int _id)
		{
			return items.FirstOrDefault(x => x.id == _id);
		}

		/// <summary>
		/// Return all the packs
		/// </summary>
		/// <returns></returns>
		public List<Pack> GetPacks()
		{
			return packs;
		}

		/// <summary>
		/// Return all the packs matching the condition _condition
		/// </summary>
		/// <param name="_condition"></param>
		/// <returns></returns>
		public List<Pack> GetPacks(Predicate<Pack> _condition)
		{
			return packs.FindAll(_condition);
		}

		/// <summary>
		/// Returns the pack with id _id
		/// </summary>
		/// <param name="_id"></param>
		/// <returns></returns>
		public Pack GetPack(int _id)
		{
			return packs.Find(x => x.id == _id);
		}

		/// <summary>
		/// Returns the pack with id _id
		/// </summary>
		/// <param name="_id"></param>
		/// <returns></returns>
		public Pack GetPack(Predicate<Pack> _condition)
		{
			return packs.Find(_condition);
		}
	}
}
