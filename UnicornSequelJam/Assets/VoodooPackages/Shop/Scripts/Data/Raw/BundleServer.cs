using VoodooPackages.Tech.Items;

namespace VoodooPackages.Tool.Shop
{
    public class BundleServer : PackServer
    {
        public string paymentId;
        public int maxAmount;

        public BundleServer()
        {
            paymentId = "";
            maxAmount = 1;
        }
    }
}