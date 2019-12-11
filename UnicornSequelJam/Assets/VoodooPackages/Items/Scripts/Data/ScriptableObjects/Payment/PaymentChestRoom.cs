using UnityEngine;

namespace VoodooPackages.Tech.Items
{
    [CreateAssetMenu(fileName = "PaymentChestRoom", menuName = "VoodooPackages/Items/Payment/Chest Room")]
    public class PaymentChestRoom : Payment
    {
        public string message;
        public double requirement;
        protected override bool IsPaymentAvailable()
        {
            return true;
        }

        protected override void Reset()
        {
            base.Reset();
            message     = "Collect in Chest Room";
            requirement = 3;
        }

        public override void LoadFrom(SavedPayment _savedPayment)
        {
            //Nothing to load
        }
    }
}