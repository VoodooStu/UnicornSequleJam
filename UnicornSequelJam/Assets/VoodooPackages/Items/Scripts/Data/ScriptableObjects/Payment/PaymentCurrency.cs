using UnityEngine;

namespace VoodooPackages.Tech.Items
{
    [CreateAssetMenu(fileName = "PaymentCurrency", menuName = "VoodooPackages/Items/Payment/Currency")]
    public class PaymentCurrency : Payment
    {
        public Currency currency;
        public double cost;
        public float costMultiplier;
        public bool lockedOnNotPurchasable;
        [SerializeField] private int numberOfPurchaseDone;

        public int NumberOfPurchaseDone
        {
            get => numberOfPurchaseDone;
            set
            {
                numberOfPurchaseDone = value;
                SaveValues();
            }
    }
//        public double Price => cost * Mathf.Pow(costMultiplier, numberOfPurchaseDone);
        public double Price => cost * (1 + numberOfPurchaseDone);
        
        private void SaveValues()
        {
            SavedPaymentCurrency savedPaymentCurrency = new SavedPaymentCurrency(id,numberOfPurchaseDone);
            BinaryPrefs.SetClass(id,savedPaymentCurrency);
        }
        
        /// <summary>
        /// Use the paymentCurrency to make a purchase.
        /// </summary>
        public void Purchase()
        {
            currency.Remove(Price);
            NumberOfPurchaseDone++;
        }
        
        protected override bool IsPaymentAvailable()
        { 
            bool res = false;
            if (currency != null)
                res = currency.IsAmountAvailable(Price);

            return res;
        }

        public override void LoadFrom(SavedPayment _savedPayment)
        {
            if (!(_savedPayment is SavedPaymentCurrency savedPaymentCurrency)) 
                return;
            
            numberOfPurchaseDone = savedPaymentCurrency.numberOfPurchaseDone;
        }

        protected override void Reset()
        {
            base.Reset();
            currency = CurrencyManager.Instance != null ? CurrencyManager.Instance.MainCurrency : default;
            cost = 0;
            costMultiplier = 1f;
            lockedOnNotPurchasable = true;
            numberOfPurchaseDone = 0;
        }
    }
}