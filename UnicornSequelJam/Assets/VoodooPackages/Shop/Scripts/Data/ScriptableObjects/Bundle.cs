using System.Linq;
using UnityEngine;
using VoodooPackages.Tech.Items;

namespace VoodooPackages.Tool.Shop
{
    [CreateAssetMenu(fileName = "Bundle", menuName = "VoodooPackages/Items/Pack/Bundle")]
    public class Bundle : Pack
    {
        // Server side
        public Payment payment;
        public int currentAmount;
        public int maxAmount;
        
        // Gameplay side
        public int AmountAvailable => maxAmount - currentAmount;

        protected override void Reset()
        {
            base.Reset();
            payment = default;
            currentAmount = 0;
            maxAmount = 1;
        }

        /// <summary>
        /// Initialize the Bundle with the values of the _bundleServer.
        /// </summary>
        /// <param name="_itemServer"></param>
        public void Initialize(BundleServer _bundleServer)
        {
            base.Initialize(_bundleServer);
            
            maxAmount = _bundleServer.maxAmount;
            payment = Resources.LoadAll<Payment>("Data").FirstOrDefault(x => x.id == _bundleServer.paymentId);
        }

        /// <summary>
        /// Method used to load the information from the _savedPack as SavedBundle
        /// </summary>
        /// <param name="_savedPack"></param>
        public override void LoadFrom(SavedPack _savedPack)
        {
            if (!(_savedPack is SavedBundle savedBundle)) 
                return;
            
            currentAmount = savedBundle.currentAmount;
        }

        /// <summary>
        /// Method used to collect the pack
        /// Returns true if the purchase succeeded, false otherwise
        /// </summary>
        /// <returns></returns>
        public override bool OnCollect()
        {
            bool res = false;
            
            if (payment.IsAvailable && AmountAvailable > 0)
            {
                if (payment is PaymentCurrency paymentCurrency)
                    paymentCurrency.Purchase();
                
                foreach (var content in contents)
                {
                    Item item = ItemManager.Instance.GetItem(content.id);
                    if (item == null)
                    {
                        continue;
                    }

                    item.Collect(content.amount);
                }

                currentAmount++;
                res = true;
            }
            else
            {
                Debug.LogWarning("Couldn't buy the pack " + name);
            }

            return res;
        }
    }
}