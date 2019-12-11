namespace VoodooPackages.Tool.Shop
{
    public class SubCategoryServer
    {
        public int    id;
        public string title;
        public string paymentId;

        public SubCategoryServer()
        {
            id          = int.MinValue;
            title       = "";
            paymentId   = "";
        }
    }
}