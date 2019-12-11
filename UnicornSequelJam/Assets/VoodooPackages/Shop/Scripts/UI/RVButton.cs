using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VoodooPackages.Tech.Items;

namespace VoodooPackages.Tool.Shop
{
    //TODO : The entire class needs to be removed and replaced by the ButtonRV of the Buttons package
    public class RVButton : MonoBehaviour
    {
        public Button buttonPicker;
        public TextMeshProUGUI value;
        public Image icon;
        
        //Cache
        private CurrencyManager currencyManager;
        private Currency RVBonusCurrency;
        private double RVBonusValue;

        /// <summary>
        /// Update the UI based on _currency and _value.
        /// </summary>
        /// <param name="_currency"></param>
        /// <param name="_value"></param>
        public void UpdateValue(Currency _currency, double _value)
        {
            if (currencyManager == null)
                currencyManager = CurrencyManager.Instance;
            
            RVBonusCurrency = _currency;
            RVBonusValue = _value;

            value.text = "+" + RVBonusValue.ToShortString();

            if (RVBonusCurrency == null)
                RVBonusCurrency = currencyManager.MainCurrency;

            if (RVBonusCurrency != null)
            {
                icon.sprite = RVBonusCurrency.icon;
                icon.color = RVBonusCurrency.color;
            }

#if VOODOO_SAUCE
            UpdateInteractivity();

            MoPubManager.OnRewardedVideoLoadedEvent += OnRewardedVideoLoaded;
#endif
        }

        /// <summary>
        /// Update the interactivity of the button based on the rewarded video availability
        /// </summary>
        public void UpdateInteractivity()
        {
            bool buttonPickerInteractivity;

#if VOODOO_SAUCE
            buttonPickerInteractivity = VoodooSauce.IsRewardedVideoAvailable();
#else
            buttonPickerInteractivity = true;
#endif
            if (buttonPicker)
                buttonPicker.interactable = buttonPickerInteractivity;
        }

        private void OnRewardedVideoLoaded(string _adUnit)
        {
            buttonPicker.interactable = true;
        }

        /// <summary>
        /// Show the Rewarded Video
        /// </summary>
        public void ShowRewardedVideo()
        {
            if (currencyManager == null)
                currencyManager = CurrencyManager.Instance;
            
#if VOODOO_SAUCE
            VoodooSauce.ShowRewardedVideo(OnComplete);
#else
            currencyManager.AddToCurrency(RVBonusValue, RVBonusCurrency);
#endif
        }

#if VOODOO_SAUCE
        private void OnComplete(bool _onComplete)
        {
            if (_onComplete)
                currencyManager.AddToCurrency(RVBonusValue, RVBonusCurrency);

#if !UNITY_EDITOR
            buttonPicker.interactable = false;
#endif
        }
#endif
    }
}