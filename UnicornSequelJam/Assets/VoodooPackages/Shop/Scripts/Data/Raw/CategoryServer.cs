namespace VoodooPackages.Tool.Shop
{
    public class CategoryServer
    {
        public int    id;
        public string title;
        public string color;
        public int[]  subCategoryIds;

        public CategoryServer()
        {
            id             = int.MinValue;
            title          = "";
            color          = "";
            subCategoryIds = new int[]{};
        }
    }
}