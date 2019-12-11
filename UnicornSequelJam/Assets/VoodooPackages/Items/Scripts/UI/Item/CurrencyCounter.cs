using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VoodooPackages.Tech.Items
{
	public class CurrencyCounter : MonoBehaviour
	{
		public Image            icon;
		public TextMeshProUGUI  amount;
		public Currency         currency;

		public new Animation    animation;

		//Cache
		private CurrencyManager currencyManager;

		public void Start()
		{
			Init();
		}

		protected virtual void Init()
		{
			currencyManager = CurrencyManager.Instance;
			
			if (currency == null && currencyManager != null)
				currency = currencyManager.MainCurrency;
			
			if (currency != null)
			{
				currencyManager.OnCurrencyChanged += Display;
				currencyManager.OnCurrencyAdded += OnCurrencyAdded;
			}
			
			Display(currency);
		}

		protected virtual void OnCurrencyAdded(Currency _currency)
		{
			if (_currency != currency)
				return;
			
			//TODO : delay the animation.Play if the user is on android and the currencyAdded has been fired through a Rewarded Video.
			animation.Play();
		}

		public virtual void Display(Currency _currency)
		{
			if (_currency != currency)
				return;
			
			if (currency == null)
			{
				if (amount != null)
				{
					amount.text = "???";
				}

				if (icon != null)
				{
					icon.sprite = null;
					icon.color = new Color(1, 1, 1, 1);
				}
			}
			else
			{
				if (amount != null)
				{
					amount.text = currency.GetDisplayedValue();
				}

				if (icon != null)
				{
					icon.sprite = currency.icon;
					icon.color = currency.color;
				}
			}
		}

		private void OnDestroy()
		{
			if (currencyManager == null)
				return;
			
			currencyManager.OnCurrencyChanged -= Display;
			currencyManager.OnCurrencyAdded -= OnCurrencyAdded;
		}
	}
}