namespace VoodooPackages.Tech.Items
{
    public class SkinServer : ItemServer
    {
        public bool     enabled;
        public bool     unlockAtStart;
        public double   price;

        public SkinServer()
        {
            enabled       = true;
            unlockAtStart = false;
            price         = 0;
        }
    }
}