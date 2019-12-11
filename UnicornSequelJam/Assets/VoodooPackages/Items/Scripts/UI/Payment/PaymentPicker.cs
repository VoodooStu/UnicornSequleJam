using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VoodooPackages.Tech.Items
{
	public class PaymentPicker : MonoBehaviour
	{
		public TextMeshProUGUI paymentValue;
		public Image paymentIcon;
		public Button buttonPicker;

		public virtual void Display(PaymentCurrency _payment)
		{
			name = _payment.GetType().ToString();
			DisplayPayment(_payment);
		}

		protected virtual void DisplayPayment(PaymentCurrency _payment)
		{
			if (_payment != null)
			{
				if (paymentValue)
				{
					if (_payment.cost == 0)
					{
						if (paymentValue)
						{
							paymentValue.text = "FREE";
						}
					}
					else
						paymentValue.text = _payment.Price.ToShortString();
				}

				if (_payment.currency == null)
				{
					return;
				}

				if (paymentIcon != null)
				{
					paymentIcon.sprite = _payment.currency.icon;
					paymentIcon.color = _payment.currency.color;
				}

				if(_payment.lockedOnNotPurchasable)
					buttonPicker.interactable = _payment.IsAvailable;
			}
		}
	}
}