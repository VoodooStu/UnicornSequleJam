using System;
using UnityEngine;
using UnityEngine.UI;

namespace VoodooPackages.Tech.Buttons
{
    public class ButtonRVVisual : MonoBehaviour
    {
        public ButtonRV rvButton;
        public Image background;
        public IconRV rvIcon;
        private Color backgroundInitialColor;

        private void Awake()
        {
            backgroundInitialColor = background.color;
        }
        
        private void Start()
        {
#if VOODOO_SAUCE
            rvButton.OnRVLoaded += SetInteractivity;
            rvButton.OnRVCompleted += OnRVCompleted;
#endif

            OnEnable();
        }

        private void OnDestroy()
        {
#if VOODOO_SAUCE
            rvButton.OnRVLoaded -= SetInteractivity;
            rvButton.OnRVCompleted -= OnRVCompleted;
#endif
        }

        private void OnEnable()
        {
#if VOODOO_SAUCE
            SetInteractivity(VoodooSauce.IsRewardedVideoAvailable());
#elif UNITY_EDITOR
            SetInteractivity(true);
#else
            SetInteractivity(false);
#endif
        }

        private void OnRVCompleted(bool _completed)
        {
#if VOODOO_SAUCE
            SetInteractivity(VoodooSauce.IsRewardedVideoAvailable());
#else
            SetInteractivity(false);
#endif
        }

        private void SetInteractivity(bool _isInteractable)
        {
            if (rvButton != null && rvButton.button != null)
                rvButton.button.interactable = _isInteractable;

            if (background != null)
            {
                if (_isInteractable)
                {
                    background.color = backgroundInitialColor;
                }
                else
                {
                    background.color = Color.grey;
                }
            }
            
            if (rvIcon != null)
            {
                rvIcon.UpdateDisplay(_isInteractable);
            }
        }
    }
}