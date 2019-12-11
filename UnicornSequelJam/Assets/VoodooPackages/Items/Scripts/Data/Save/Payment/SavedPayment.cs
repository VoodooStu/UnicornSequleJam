using System;

namespace VoodooPackages.Tech.Items
{
    [Serializable]
    public class SavedPayment
    {
        public string id;

        public SavedPayment(string _id)
        {
            id = _id;
        }
    }
}