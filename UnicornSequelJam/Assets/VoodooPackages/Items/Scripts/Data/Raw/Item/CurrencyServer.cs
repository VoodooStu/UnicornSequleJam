namespace VoodooPackages.Tech.Items
{
    public class CurrencyServer : ItemServer
    {
        public bool     enabled;
        public double   defaultAmount;
        public double   maxAmount;

        public CurrencyServer()
        {
            enabled         = false;
            defaultAmount   = 0;
            maxAmount       = double.MaxValue;
        }
    }
}