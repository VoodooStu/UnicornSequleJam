using System;
using UnityEngine;

namespace VoodooPackages.Tech.Items
{
	public abstract class Payment : ScriptableObject
	{
		public string id;
		protected abstract bool IsPaymentAvailable();
		
		public bool IsAvailable => IsPaymentAvailable();
 
		protected virtual void Reset()
		{
			id = Guid.NewGuid().ToString();
		}

		/// <summary>
		/// Method used to load the information from the _savedPayment
		/// </summary>
		/// <param name="_savedPayment"></param>
		public abstract void LoadFrom(SavedPayment _savedPayment);
	}
}