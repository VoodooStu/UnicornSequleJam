using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace VoodooPackages.Tech.Buttons
{
    public class ButtonGroupController : MonoBehaviour
    {
        public ButtonGroupData defaultButtonGroupData;
        [FormerlySerializedAs("parentButtonGroupData")] public Transform parentButtonGroupVisual;
        public bool enableSpecialButtonGroupDatas;
        public bool destroySpecialButtonOnClick;
        public List<ButtonGroupData> buttonGroupDatas;
        public UnityEvent DefaultButtonClicked;
        private bool isInitialized;

        //Cache
        private WeightedRandomizer<ButtonGroupData> weightedButtonGroupDatas;
        private ButtonGroupVisual _buttonGroupVisualInstance;
        private ButtonGroupData _buttonGroupData;

        /// <summary>
        /// Initialize the weightedButtonGroupDatas. Can be done only once per session.
        /// </summary>
        public void Initialize()
        {
            if (!isInitialized)
            {
                weightedButtonGroupDatas = new WeightedRandomizer<ButtonGroupData>();
                foreach (ButtonGroupData _buttonGroupData in buttonGroupDatas)
                {
                    if (_buttonGroupData == null)
                        continue;

#if VOODOO_SAUCE
                    if (_buttonGroupData.buttonGroupVisualPrefab != null &&
                        _buttonGroupData.buttonGroupVisualPrefab.specialButton != null)
                    {
                        Voodoo.Sauce.Components.NoAdsButton noAdsButton = _buttonGroupData.buttonGroupVisualPrefab.specialButton.GetComponent<Voodoo.Sauce.Components.NoAdsButton>();
                        if (noAdsButton == null || !VoodooSauce.IsPremium())
                        {
                            weightedButtonGroupDatas.AddElement(_buttonGroupData, _buttonGroupData.weight);
                        }
                    }
                    else
                        weightedButtonGroupDatas.AddElement(_buttonGroupData, _buttonGroupData.weight);
#else

                    weightedButtonGroupDatas.AddElement(_buttonGroupData, _buttonGroupData.weight);
#endif
                    isInitialized = true;
                }
            }
        }

        public void InstantiateDefaultButton()
        {
            Initialize();
            
            _buttonGroupData = defaultButtonGroupData;
            _buttonGroupVisualInstance = Instantiate(_buttonGroupData.buttonGroupVisualPrefab, parentButtonGroupVisual != null ? parentButtonGroupVisual : transform);
            _buttonGroupVisualInstance.DefaultButtonClicked.AddListener(ContinueButtonClickedInvoker);
        }

        /// <summary>
        /// Instantiate the randomly choose ButtonGroup and subscribe to its event
        /// </summary>
        public void InstantiateButtonGroup()
        {
            Initialize();
            
            if (_buttonGroupVisualInstance != null)
                Destroy(_buttonGroupVisualInstance.gameObject);

            #if VOODOO_SAUCE
            _buttonGroupData = enableSpecialButtonGroupDatas && weightedButtonGroupDatas.Count > 0 ? weightedButtonGroupDatas.TakeOne() : defaultButtonGroupData;
            #else
            _buttonGroupData = defaultButtonGroupData;
            #endif
            _buttonGroupVisualInstance = Instantiate(_buttonGroupData.buttonGroupVisualPrefab, parentButtonGroupVisual != null ? parentButtonGroupVisual : transform);
            _buttonGroupVisualInstance.DefaultButtonClicked.AddListener(ContinueButtonClickedInvoker);
            if (destroySpecialButtonOnClick)
                _buttonGroupVisualInstance.SpecialButtonClicked.AddListener(DestroyButton);
        }
        
        private void ContinueButtonClickedInvoker()
        {
            DefaultButtonClicked?.Invoke();
            DestroyButton();
        }

        private void DestroyButton()
        {
            if (_buttonGroupVisualInstance != null)
            {
                Destroy(_buttonGroupVisualInstance.gameObject);
            }
        }

        /// <summary>
        /// Returns the current ButtonGroupData
        /// </summary>
        /// <returns></returns>
        public ButtonGroupData GetButtonGroupData()
        {
            return _buttonGroupData;
        }

        /// <summary>
        /// Returns the current ButtonGroupVisual
        /// </summary>
        /// <returns></returns>
        public ButtonGroupVisual GetButtonGroupVisual()
        {
            return _buttonGroupVisualInstance;
        }
    }
}