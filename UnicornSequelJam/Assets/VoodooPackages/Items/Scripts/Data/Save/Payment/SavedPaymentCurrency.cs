using System;

namespace VoodooPackages.Tech.Items
{
    [Serializable]
    public class SavedPaymentCurrency : SavedPayment
    {
        public int numberOfPurchaseDone;

        public SavedPaymentCurrency(string _id, int _numberOfPurchaseDone) : base(_id)
        {
            numberOfPurchaseDone = _numberOfPurchaseDone;
        }
    }
}