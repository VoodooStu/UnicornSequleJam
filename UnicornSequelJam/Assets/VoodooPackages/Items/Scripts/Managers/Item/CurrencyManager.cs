using System.Collections.Generic;

namespace VoodooPackages.Tech.Items
{
	public class CurrencyManager : SingletonMB<CurrencyManager>
	{
		public List<Currency> currencies;
		private ItemManager itemManager;

		public Currency MainCurrency
		{
			get
			{
				Currency res = null;
				if (currencies != null && currencies.Count > 0)
				{
					res = currencies[0];
				}

				return res;
			}
		}

		public event Currency.CurrencyEvent OnCurrencyAdded;
		public event Currency.CurrencyEvent OnCurrencyRemovedSuccess;
		public event Currency.CurrencyEvent OnCurrencyRemovedFailed;
		public event Currency.CurrencyEvent OnCurrencyChanged;

		private void Start()
		{
			itemManager = ItemManager.Instance;
			
			if (itemManager.IsInitialized)
				Initialize();
			else
				itemManager.onInitializeSuccess.AddListener(Initialize);
		}

		private void Initialize()
		{
			itemManager.onInitializeSuccess.RemoveListener(Initialize);

			for (var i = 0; i < currencies.Count; i++)
			{
				Currency currency = currencies[i];

				if (currency == null)
					continue;

				currency.OnCurrencyChanged += OnCurrencyChangedInvoker;
				currency.OnCurrencyAdded += OnCurrencyAddedInvoker;
				currency.OnCurrencyRemovedSuccess += OnCurrencyRemovedSuccessInvoker;
				currency.OnCurrencyRemovedFailed += OnCurrencyRemovedFailedInvoker;
				
			}
		}

		private void OnDestroy()
		{
			if (currencies == null || currencies.Count == 0)
				return;
			
			for (var i = 0; i < currencies.Count; i++)
			{
				Currency currency = currencies[i];

				if (currency == null)
					continue;
				
				currency.OnCurrencyChanged -= OnCurrencyChangedInvoker;
				currency.OnCurrencyAdded -= OnCurrencyAddedInvoker;
				currency.OnCurrencyRemovedSuccess -= OnCurrencyRemovedSuccessInvoker;
				currency.OnCurrencyRemovedFailed -= OnCurrencyRemovedFailedInvoker;
			}
		}
		
		#region Currency Events

		private void OnCurrencyChangedInvoker(Currency _currency)
		{
			OnCurrencyChanged?.Invoke(_currency);
		}

		private void OnCurrencyAddedInvoker(Currency _currency)
		{
			OnCurrencyAdded?.Invoke(_currency);
		}

		private void OnCurrencyRemovedSuccessInvoker(Currency _currency)
		{
			OnCurrencyRemovedSuccess?.Invoke(_currency);
		}

		private void OnCurrencyRemovedFailedInvoker(Currency _currency)
		{
			OnCurrencyRemovedFailed?.Invoke(_currency);
		}
		
		#endregion

		//Add value to the chosen currency
		#region Add
		
		/// <summary>
		/// Add _value to the first currency in the list of currency
		/// </summary>
		/// <param name="_value"></param>
		public void AddToMainCurrency(float _value)
		{
			if (currencies != null && currencies.Count > 0)
			{
				currencies[0].Add(_value);
			}
		}
		
		/// <summary>
		/// Add _value to the first currency in the list of currency
		/// </summary>
		/// <param name="_value"></param>
		public void AddToMainCurrency(double _value)
		{
			if (currencies != null && currencies.Count > 0)
			{
				currencies[0].Add(_value);
			}
		}

		/// <summary>
		/// Add _value to the currency _currency
		/// </summary>
		/// <param name="_value"></param>
		/// <param name="_currency"></param>
		public void AddToCurrency(double _value, Currency _currency)
		{
			if (currencies != null && currencies.Count > 0 && _currency != null && currencies.Contains(_currency))
			{
				_currency.Add(_value);
			}
		}

		/// <summary>
		/// Add _value to the currency with id _currencyId
		/// </summary>
		/// <param name="_value"></param>
		/// <param name="_currencyId"></param>
		public void AddToCurrency(double _value, int _currencyId)
		{
			if (currencies != null && currencies.Count > 0 && _currencyId != int.MinValue && currencies.Exists(x => x.id == _currencyId))
			{
				currencies.Find(x => x.id == _currencyId).Add(_value);
			}
		}
		
		#endregion
		
		//Remove value from the chosen currency
		#region Remove
		
		/// <summary>
		/// Remove _value from the first currency in the list of currency
		/// </summary>
		/// <param name="_value"></param>
		public void RemoveFromMainCurrency(float _value)
		{
			if (currencies != null && currencies.Count > 0)
			{
				currencies[0].Remove(_value);
			}
		}

		/// <summary>
		/// Remove _value from the first currency in the list of currency
		/// </summary>
		/// <param name="_value"></param>
		public void RemoveFromMainCurrency(double _value)
		{
			if (currencies != null && currencies.Count > 0)
			{
				currencies[0].Remove(_value);
			}
		}

		/// <summary>
		/// Remove _value from the currency _currency
		/// </summary>
		/// <param name="_value"></param>
		/// <param name="_currency"></param>
		public void RemoveFromCurrency(double _value, Currency _currency)
		{
			if (currencies != null && currencies.Count > 0 && _currency != null && currencies.Contains(_currency))
			{
				_currency.Remove(_value);
			}
		}

		/// <summary>
		/// Remove _value from the currency with id _currencyId
		/// </summary>
		/// <param name="_value"></param>
		/// <param name="_currencyId"></param>
		public void RemoveFromCurrency(double _value, int _currencyId)
		{
			if (currencies != null && currencies.Count > 0 && _currencyId != int.MinValue && currencies.Exists(x => x.id == _currencyId))
			{
				currencies.Find(x => x.id == _currencyId).Remove(_value);
			}
		}
		
		#endregion

		//Set value to the chosen currency
		#region Set
		
		/// <summary>
		/// Set the first currency _value to the first currency in the list of currency
		/// </summary>
		/// <param name="_value"></param>
		public void SetMainCurrency(float _value)
		{
			if (currencies != null && currencies.Count > 0)
			{
				currencies[0].SetValue(_value);
			}
		}
		
		/// <summary>
		/// Set the first currency _value to the first currency in the list of currency
		/// </summary>
		/// <param name="_value"></param>
		public void SetMainCurrency(double _value)
		{
			if (currencies != null && currencies.Count > 0)
			{
				currencies[0].SetValue(_value);
			}
		}

		/// <summary>
		/// Set the first currency _value to _currency
		/// </summary>
		/// <param name="_currency"></param>
		public void SetCurrency(double _value, Currency _currency)
		{
			if (currencies != null && currencies.Count > 0 && _currency != null && currencies.Contains(_currency))
			{
				_currency.SetValue(_value);
			}
		}

		/// <summary>
		/// Add _value to the currency with id _currencyId
		/// </summary>
		/// <param name="_value"></param>
		/// <param name="_currencyId"></param>
		public void SetCurrency(double _value, int _currencyId)
		{
			if (currencies != null && currencies.Count > 0 && _currencyId != int.MinValue && currencies.Exists(x => x.id == _currencyId))
			{
				currencies.Find(x => x.id == _currencyId).SetValue(_value);
			}
		}

		#endregion

		//Reset the value of the chosen currency
		#region Reset
		
		/// <summary>
		/// Reset the first currency in the list of currency
		/// </summary>
		public void ResetMainCurrency()
		{
			if (currencies != null && currencies.Count > 0)
			{
				currencies[0].ResetValue();
			}
		}
		
		/// <summary>
		/// Reset the currency _currency
		/// </summary>
		/// <param name="_currency"></param>
		public static void ResetCurrency(Currency _currency)
		{
			if (_currency != null)
			{
				_currency.ResetValue();
			}
		}
		
		/// <summary>
		/// Reset the currency with id _currencyId
		/// </summary>
		/// <param name="_currencyId"></param>
		public void ResetCurrency(int _currencyId)
		{
			if (currencies != null && currencies.Count > 0 && _currencyId != int.MinValue && currencies.Exists(x => x.id == _currencyId))
			{
				currencies.Find(x => x.id == _currencyId).ResetValue();
			}
		}
		
		/// <summary>
		/// Reset all currency in the list of currency
		/// </summary>
		public void ResetAllCurrencies()
		{
			for (int i = 0; i < currencies.Count; i++)
			{
				Currency currency = currencies[i];
				currency.ResetValue();
			}
		}

		#endregion
	}
}