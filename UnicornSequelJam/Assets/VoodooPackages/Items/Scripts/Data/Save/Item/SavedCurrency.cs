using System;

namespace VoodooPackages.Tech.Items
{
	[Serializable]
	public class SavedCurrency : SavedItem
	{
		public double value;

		public SavedCurrency(int _id, double _value) : base(_id)
		{
			value = _value;
		}
	}
}
