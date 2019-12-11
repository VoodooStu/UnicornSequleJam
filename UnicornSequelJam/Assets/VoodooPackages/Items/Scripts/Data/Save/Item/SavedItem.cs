using System;

namespace VoodooPackages.Tech.Items
{
    [Serializable]
    public class SavedItem
    {
        public int id;

        public SavedItem(int _id)
        {
            id = _id;
        }
    }
}