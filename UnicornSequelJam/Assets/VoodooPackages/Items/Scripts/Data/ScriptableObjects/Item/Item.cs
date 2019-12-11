using System;
using UnityEngine;

namespace VoodooPackages.Tech.Items
{
	public abstract class Item : ScriptableObject
	{
		// Server side
		public int    id;
		public string itemName;
		public Sprite icon;
		public Color  color;

		protected virtual void Reset()
		{
			id          = Guid.NewGuid().GetHashCode();
			itemName    = name;
			icon        = default;
			color       = Color.white;
		}

		/// <summary>
		/// Initialize the Item with the values of the _itemServer.
		/// </summary>
		/// <param name="_itemServer"></param>
		public virtual void Initialize(ItemServer _itemServer)
		{
			id       = _itemServer.id;
			itemName = _itemServer.itemName;
			icon     = Resources.Load<Sprite>("Sprites/" + _itemServer.image);
			ColorUtility.TryParseHtmlString(_itemServer.color, out color);
		}

		/// <summary>
		/// Method used to load the information from the _savedItem
		/// </summary>
		/// <param name="_savedItem"></param>
		public abstract void LoadFrom(SavedItem _savedItem);

		/// <summary>
		/// Method used to collect _amount of your item (ex : 100 coins, 1 skin,...)
		/// </summary>
		/// <param name="_amount"></param>
		public abstract void Collect(int _amount);
	}
}