using System;

namespace VoodooPackages.Tech.Items
{
    [Serializable]
    public class SavedPack
    {
        public string id;

        public SavedPack(string _id)
        {
            id = _id;
        }
    }
}