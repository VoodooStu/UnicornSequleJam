using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace VoodooPackages.Tech.Items
{
    public abstract class Pack : ScriptableObject
    {
        // Server side
        public int               id;
        public string            packName;
        public List<PackContent> contents;
        public Sprite            image;
        public Color             color;

        public delegate void PackEvent(Pack _pack);
        public event PackEvent onCollectSuccessful;
        public event PackEvent onCollectFailed;

        protected virtual void Reset()
        {
            id       = Guid.NewGuid().GetHashCode();
            packName = name;
            contents = new List<PackContent>();
            image    = default;
            color    = Color.white;
        }

        /// <summary>
        /// Initialize the Pack with the values of the _packServer.
        /// </summary>
        /// <param name="_packServer"></param>
        public virtual void Initialize(PackServer _packServer)
        {
            id       = _packServer.id;
            packName = _packServer.itemName;
            contents = GetContentFromServer(_packServer);
            image    = Resources.Load<Sprite>(_packServer.image);
            ColorUtility.TryParseHtmlString(_packServer.color, out color);
        }

        /// <summary>
        /// Method that allows you to manage data with dependencies
        /// </summary>
        public virtual void OnInitialized()
        {
            
        }

        private List<PackContent> GetContentFromServer(PackServer _packServer)
        {
            contents = _packServer.contents;
            return contents;
        }

        /// <summary>
        /// Call the OnCollect method and fire the appropriate event (OnCollectSuccessful/OnCollectFailed)
        /// </summary>
        /// <param name="_amount"></param>
        public void Collect()
        {
            bool collect = OnCollect();
            
            if (collect)
            { 
                onCollectSuccessful?.Invoke(this);
            }
            else
            { 
                onCollectFailed?.Invoke(this);
            }
        }
        
        /// <summary>
        /// Method used to collect the pack
        /// </summary>
        /// <returns></returns>
        public abstract bool OnCollect();

        /// <summary>
        /// Method used to load the information from the _savedPack
        /// </summary>
        /// <param name="_savedPack"></param>
        public abstract void LoadFrom(SavedPack _savedPack);
    }

    [Serializable]
    public struct PackContent
    {
        public int  id;
        public int  amount;

        public PackContent(int _id, int _amount)
        {
            id = _id;
            amount = _amount;
        }
    }
}