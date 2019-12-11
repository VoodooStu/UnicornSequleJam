using System;
using UnityEngine;

namespace VoodooPackages.Tech.Items
{
    [CreateAssetMenu(fileName = "Currency", menuName = "VoodooPackages/Items/Item/Currency")]
    public class Currency : Item
    {
	    //Server side
	    public bool     enabled;
	    public double   defaultAmount;
	    public double   maxAmount;

	    //Gameplay side
		public double   currentAmount; //Might be possible to make it private set;

		public delegate void CurrencyEvent(Currency _currency);
		public event CurrencyEvent OnCurrencyAdded;
		public event CurrencyEvent OnCurrencyRemovedSuccess;
		public event CurrencyEvent OnCurrencyRemovedFailed;
		public event CurrencyEvent OnCurrencyChanged;

		protected override void Reset()
		{
			base.Reset();
			defaultAmount   = 0;
			maxAmount       = double.MaxValue;
			currentAmount   = 0;
		}

		private void CurrencyChanged()
		{
			OnCurrencyChanged?.Invoke(this);
			SavedCurrency savedCurrency = new SavedCurrency(id,currentAmount);
			BinaryPrefs.SetClass(id.ToString(),savedCurrency);
		}

		/// <summary>
		/// Initialize the Currency with the values of the _itemServer.
		/// </summary>
		/// <param name="_itemServer"></param>
		public override void Initialize(ItemServer _itemServer)
		{
			if (!(_itemServer is CurrencyServer itemCurrencyServer))
            {
				return;
            }

			base.Initialize(_itemServer);

			enabled = itemCurrencyServer.enabled;
			defaultAmount = itemCurrencyServer.defaultAmount;
			maxAmount = itemCurrencyServer.maxAmount;
		}

		/// <summary>
		/// Method used to load the information from the _savedItem
		/// </summary>
		/// <param name="_savedItem"></param>
		public override void LoadFrom(SavedItem _savedItem)
		{
			if (!(_savedItem is SavedCurrency savedCurrency))
            { 
				return;
            }
			
			SetValue(savedCurrency.value);
		}
		
		/// <summary>
		/// Method used to collect _amount of your currency (ex : add 100 coins)
		/// </summary>
		/// <param name="_amount"></param>
		public override void Collect(int _amount)
		{
			Add(_amount);
		}

		/// <summary>
		/// Set the currentAmount to _value without exceeding the maxAmount or getting below 0.
		/// Fires the CurrencyChanged event.
		/// </summary>
		/// <param name="_value"></param>
		public void SetValue(double _value)
		{
			currentAmount = Math.Max(0,_value);
			currentAmount = Math.Min(currentAmount, maxAmount);
			CurrencyChanged();
		}

		/// <summary>
		/// Reset the value to 0 and update the savedValues accordingly
		/// </summary>
		public void ResetValue()
		{
			SetValue(0);
		}

		/// <summary>
		/// Add _value to the current currency value.
		/// Call the Remove method if _value is negative.
		/// Fires the CurrencyManager events if _fireCallbacks is set to true.
		/// </summary>
		/// <param name="_value"></param>
		/// <param name="_fireCallbacks"></param>
		/// <returns>true if the _value has been successfully added</returns>
		public void Add(double _value, bool _fireCallbacks = true)
		{
			if (_value < 0)
			{
				Remove(-_value);
				return;
			}

			double intValue = _value - _value % 1;

			//Error if (currentValue + intValue) > double.MaxValue. Not worth checking it.
			currentAmount += intValue;
			currentAmount = Math.Min(currentAmount, maxAmount);

			if (_fireCallbacks)
			{
				OnCurrencyAdded?.Invoke(this);
				CurrencyChanged();
			}
		}

		/// <summary>
		/// Remove _value to the current currency value.
		/// Fires the CurrencyManager events if _fireCallbacks is set to true.
		/// </summary>
		/// <param name="_value"></param>
		/// <param name="_fireCallbacks"></param>
		/// <returns>true if the _value has been successfully removed</returns>
		public void Remove(double _value, bool _fireCallbacks = true)
		{
			double intValue = _value - _value % 1;

			bool isAmountAvailable = IsAmountAvailable(intValue);

			if (isAmountAvailable)
            {
                currentAmount -= intValue;
            }

			if (_fireCallbacks)
			{
				if (isAmountAvailable)
				{
					OnCurrencyRemovedSuccess?.Invoke(this);
					CurrencyChanged();
				}
				else
				{
					OnCurrencyRemovedFailed?.Invoke(this);
				}
			}
		}

		/// <summary>
		/// Is _amount available for your currency
		/// </summary>
		/// <param name="_amount"></param>
		/// <returns></returns>
		public bool IsAmountAvailable(double _amount)
		{
			return _amount <= currentAmount;
		}

		/// <summary>
		/// Get the well-formed value
		/// </summary>
		/// <returns></returns>
		public string GetDisplayedValue()
		{
			return currentAmount.ToShortString();
		}
    }
}
