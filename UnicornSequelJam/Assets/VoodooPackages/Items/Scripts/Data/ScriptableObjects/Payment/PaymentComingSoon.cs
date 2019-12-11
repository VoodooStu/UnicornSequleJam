using UnityEngine;

namespace VoodooPackages.Tech.Items
{
    [CreateAssetMenu(fileName = "PaymentComingSoon", menuName = "VoodooPackages/Items/Payment/Coming Soon")]
    public class PaymentComingSoon : Payment
    {
        public string message;
        protected override bool IsPaymentAvailable()
        {
            return true;
        }

        protected override void Reset()
        {
            base.Reset();
            message = "Coming Soon";
        }

        public override void LoadFrom(SavedPayment _savedPayment)
        {
            //Nothing to load
        }
    }
}