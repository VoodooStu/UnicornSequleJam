using System;

namespace VoodooPackages.Tech.Items
{
    [Serializable]
    public class SavedSkin : SavedItem
    {
        public bool isCollected;
        public bool isUsed;

        public SavedSkin(int _id, bool _isCollected, bool _isUsed = false) : base(_id)
        {
            id          = _id;
            isCollected = _isCollected;
            isUsed      = _isUsed;
        }
    }
}