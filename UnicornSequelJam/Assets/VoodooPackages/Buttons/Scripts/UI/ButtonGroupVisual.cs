using System;
using UnityEngine;
using UnityEngine.Events;
using VoodooPackages.Tech.Times;

namespace VoodooPackages.Tech.Buttons
{
    public class ButtonGroupVisual : MonoBehaviour
    {
        public ButtonHandler defaultButton;
        public ButtonHandler specialButton;
        public DelayBar delayBar;

        public float defaultButtonDelay;
        public float specialButtonDelay;
        public bool initializeOnstart;

        public UnityEvent DefaultButtonClicked;
        public UnityEvent SpecialButtonClicked;
        public UnityEvent SpecialButtonDelayEnded;
        
        //Cache
        private DelayGameObjectActivation defaultButtonDelayGameObjectActivation;

        private void Start()
        {
            if (initializeOnstart)
            {
                Initialize();
                Launch();
            }
        }

        /// <summary>
        /// Initialize the ButtonGroup and subscribe to the different events
        /// </summary>
        public void Initialize()
        {
            if (defaultButton)
            {
                defaultButton.button.onClick.AddListener(ContinueButtonClickedInvoker);

                defaultButtonDelayGameObjectActivation = defaultButton.GetComponent<DelayGameObjectActivation>();
                if (defaultButtonDelayGameObjectActivation != null)
                    defaultButtonDelayGameObjectActivation.delay = defaultButtonDelay;
            }
            
            if (specialButton != null)
            {
                specialButton.button.onClick.AddListener(SpecialButtonClickedInvoker);
                if (specialButton is ButtonRV buttonRV)
                {
                    buttonRV.OnRVLoaded += OnRVLoaded;
                }
            }

            if (delayBar)
            {
                delayBar.delay = specialButtonDelay;
                delayBar.DelayEnded += OnSpecialButtonDelayEnded;
                delayBar.DelayEnded += SpecialButtonDelayEndedInvoker;
            }
        }

        private void OnDestroy()
        {
            if (defaultButton)
            {
                defaultButton.button.onClick.RemoveListener(ContinueButtonClickedInvoker);
            }
            
            if (specialButton != null)
            {
                specialButton.button.onClick.RemoveListener(SpecialButtonClickedInvoker);
                if (specialButton is ButtonRV buttonRV)
                {
                    buttonRV.OnRVLoaded -= OnRVLoaded;
                }
            }
            
            if (delayBar)
            {
                delayBar.DelayEnded -= OnSpecialButtonDelayEnded;
                delayBar.DelayEnded -= SpecialButtonDelayEndedInvoker;
            }
        }

        /// <summary>
        /// Launch the different countdowns
        /// </summary>
        public void Launch()
        {
            if (defaultButtonDelayGameObjectActivation)
                defaultButtonDelayGameObjectActivation.StartCountdown();
            
            if (specialButton == null)
                return;

#if VOODOO_SAUCE
            if (specialButton is ButtonRV && !VoodooSauce.IsRewardedVideoAvailable())
            {
                UpdateSpecialButtonInteractivity(false);
                return;
            }
#endif
            StartDelayBarCountdown();
        }
        
        /// <summary>
        /// Update the interactivity of the special button based on _isInteractable
        /// </summary>
        /// <param name="_isInteractable"></param>
        public void UpdateSpecialButtonInteractivity(bool _isInteractable)
        {
            if (specialButton != null && specialButton.button != null)
                specialButton.button.interactable = _isInteractable;
        }

        private void OnSpecialButtonDelayEnded(Timer _timer)
        {
#if VOODOO_SAUCE
            UpdateSpecialButtonInteractivity(specialButtonDelay <= 0 && VoodooSauce.IsRewardedVideoAvailable());
#else
            UpdateSpecialButtonInteractivity(specialButtonDelay <= 0);
#endif
        }

        private void OnRVLoaded(bool _isLoaded)
        {
            if (_isLoaded)
            {
                if (isActiveAndEnabled)
                    StartDelayBarCountdown();
                else
                    UpdateSpecialButtonInteractivity(true);
            }
            else
                UpdateSpecialButtonInteractivity(false);
        }

        private void StartDelayBarCountdown()
        {
            UpdateSpecialButtonInteractivity(true);
            if (delayBar != null)
                delayBar.StartCountdown();
        }
        
        private void ContinueButtonClickedInvoker()
        {
            DefaultButtonClicked?.Invoke();
        }

        private void SpecialButtonClickedInvoker()
        {
            SpecialButtonClicked?.Invoke();
        }

        private void SpecialButtonDelayEndedInvoker(Timer _timer)
        {
            SpecialButtonDelayEnded?.Invoke();
        }
    }
}