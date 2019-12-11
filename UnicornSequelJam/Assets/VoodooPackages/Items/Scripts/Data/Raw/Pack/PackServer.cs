using System.Collections.Generic;

namespace VoodooPackages.Tech.Items
{
    public class PackServer
    {
        public int               id;
        public string            itemName;
        public List<PackContent> contents;
        public string            image;
        public string            color;

        public PackServer()
        {
            id       = int.MinValue;
            itemName = "";
            contents = new List<PackContent>();
            image    = "";
            color    = "";
        }
    }
}