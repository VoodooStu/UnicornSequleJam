using UnityEngine;

namespace VoodooPackages.Tech.Items
{
    [CreateAssetMenu(fileName = "Reward", menuName = "VoodooPackages/Items/Pack/Reward")]
    public class Reward : Pack
    {
        public override void LoadFrom(SavedPack _savedPack)
        {
            //Nothing to do here
        }

        /// <summary>
        /// Run through all pack contents and collect the referenced item to the precised amount
        /// </summary>
        /// <returns></returns>
        public override bool OnCollect()
        {
            foreach (PackContent content in contents)
            {
                Item item = ItemManager.Instance.GetItem(content.id);
                if (item == null)
                {
                    continue;
                }

                item.Collect(content.amount);
            }

            return true;
        }
    }
}