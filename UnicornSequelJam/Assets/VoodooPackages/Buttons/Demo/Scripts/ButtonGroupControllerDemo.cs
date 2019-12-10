using System.Collections.Generic;
using UnityEngine;

namespace VoodooPackages.Tech.Buttons
{
    public class ButtonGroupControllerDemo : MonoBehaviour
    {
        public List<ButtonGroupController> buttonGroupControllers;

        private void Start()
        {
            foreach (ButtonGroupController buttonGroupController in buttonGroupControllers)
            {
                buttonGroupController.InstantiateButtonGroup();
                buttonGroupController.DefaultButtonClicked.AddListener(OnButtonClick);
            }
        }

        private void OnButtonClick()
        {
            Debug.Log("Default Button has been pressed");
        }
    }
}