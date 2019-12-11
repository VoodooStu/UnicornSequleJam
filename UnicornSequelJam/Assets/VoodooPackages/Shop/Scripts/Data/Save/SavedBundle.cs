using System;
using VoodooPackages.Tech.Items;

namespace VoodooPackages.Tool.Shop
{
    [Serializable]
    public class SavedBundle : SavedPack
    {
        public int currentAmount;

        public SavedBundle(string _id, int _currentAmount) : base(_id)
        {
            currentAmount = _currentAmount;
        }
    }
}