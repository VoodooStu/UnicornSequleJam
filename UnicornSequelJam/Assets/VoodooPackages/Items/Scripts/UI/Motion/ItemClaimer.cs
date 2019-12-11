using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoodooPackages.Tech.Buttons;
using VoodooPackages.Tech.EasedAnimations;

namespace VoodooPackages.Tech.Items
{
    public class ItemClaimer : MonoBehaviour
    {
        public GameObject   visuals;
        public Image        image;

        public ButtonStandard claimButton;

        public List<EasedAnimation> animations = new List<EasedAnimation>();
        
        public delegate void ClaimClicked();
        public event ClaimClicked OnClicked;

        /// <summary>
        /// Display ui elements with items image
        /// </summary>
        /// <param name="_item"></param>
        public void ShowItem(Item _item)
        {
            image.sprite    = _item.icon;
            image.color     = _item.color;

            transform.SetSiblingIndex(transform.parent.childCount);
            
            ShowVisuals();
        }

        private void ShowVisuals()
        {
            visuals.SetActive(true);

            for (int i = 0; i < animations.Count; i++)
                animations[i].StartAnimation();

            claimButton.OnClicked += ClaimPressed;
        }

        /// <summary>
        /// Called once claim Button is pressed
        /// </summary>
        public void ClaimPressed()
        {
            claimButton.OnClicked -= ClaimPressed;
            visuals.SetActive(false);

            OnClicked?.Invoke();
        }

    }
}