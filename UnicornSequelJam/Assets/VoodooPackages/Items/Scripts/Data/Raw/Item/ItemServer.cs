namespace VoodooPackages.Tech.Items
{
    public class ItemServer
    {
        public int    id;
        public string itemName;
        public string image;
        public string color;

        public ItemServer()
        {
            id       = int.MinValue;
            itemName = "";
            image    = "";
            color    = "";
        }
    }
}