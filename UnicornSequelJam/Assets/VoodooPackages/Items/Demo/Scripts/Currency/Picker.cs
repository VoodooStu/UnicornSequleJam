using UnityEngine;

namespace VoodooPackages.Tech.Items
{
	public class Picker : MonoBehaviour
	{
		public Currency currency;
		public double value;

		/// <summary>
		/// Pick the currency
		/// </summary>
		public void Pick()
		{
			currency.Add(value);
		}
	}
}
