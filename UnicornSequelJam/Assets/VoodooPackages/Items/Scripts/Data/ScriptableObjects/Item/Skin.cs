using UnityEngine;

namespace VoodooPackages.Tech.Items
{
    [CreateAssetMenu(fileName = "Skin", menuName = "VoodooPackages/Items/Item/Skin")]
    public class Skin : Item
    {
        //Server side
        public bool   enabled;
        public bool   unlockAtStart;
        public double price;

        //Gameplay side
        public delegate void SkinEvent(Skin _item);
        public event SkinEvent onSkinPurchasedSuccessfully;
        public event SkinEvent onSkinSelected;
		
        [SerializeField] private bool isCollected;
        [SerializeField] private bool isUsed;
        public GameObject             previewPrefab;

        /// <summary>
        /// Initialize the Skin with the values of the _itemServer as SkinServer.
        /// </summary>
        /// <param name="_itemServer"></param>
        public override void Initialize(ItemServer _itemServer)
        {
            if (!(_itemServer is SkinServer itemSkinServer))
            {
                return;
            }
            
            base.Initialize(_itemServer);
            
            enabled       = itemSkinServer.enabled;
            unlockAtStart = itemSkinServer.unlockAtStart;
            price         = itemSkinServer.price;
        }

        /// <summary>
        /// Method used to load the information from the _savedItem as SavedSkin
        /// </summary>
        /// <param name="_savedItem"></param>
        public override void LoadFrom(SavedItem _savedItem)
        {
            if (!(_savedItem is SavedSkin savedSkin))
            { 
                return;
            }
            
            isCollected = savedSkin.isCollected;
            isUsed      = savedSkin.isUsed;
        }

        /// <summary>
        /// Method used to collect your skin
        /// </summary>
        /// <param name="_amount"></param>
        public override void Collect(int _amount)
        {
            IsCollected = true;
        }
        
        public bool IsCollected
        {
            get => isCollected;
            set
            {
                isCollected = value;

                if (value)
                {
                    onSkinPurchasedSuccessfully?.Invoke(this);
                }
                
                Save();
            }
        }

        public bool IsUsed
        {
            get => isUsed;
            set
            {
                if (isUsed == value)
                    return;
				
                isUsed = value;

                if (value)
                {
                    onSkinSelected?.Invoke(this);
                }

                Save();
            }
        }

        protected override void Reset()
        {
           base.Reset();
           enabled       = true;
           unlockAtStart = false;
           price         = 0;
        }

        /// <summary>
        /// Convert the skin into a savedSkin and save it.
        /// </summary>
        private void Save()
        {
            SavedSkin savedSkin = new SavedSkin(id, isCollected, isUsed);
            BinaryPrefs.SetClass(id.ToString(),savedSkin);
        }
    }
}