using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VoodooPackages.Tech.Buttons;

namespace VoodooPackages.Tech
{
	public class SuccessView : View<SuccessView>
	{
		public ButtonGroupController m_ButtonGroupController;
		public Button m_IconButton;
		
		public void OnContinueButton()
		{
			Debug.Log("Write your code here");
			//TODO : Go To Main Menu
		}

		private void OnRewardedVideoCompleted(bool _completed)
		{
			m_IconButton.interactable = false;
			Debug.Log("Write your code here");
			if (_completed)
			{
				//TODO : Give Reward to the user
			}
			else
			{
				//TODO : Specific action when the Rewarded Video is canceled
			}
		}

		public override void Show()
		{
			base.Show();

			if (m_ButtonGroupController == null)
				return;

			m_ButtonGroupController.InstantiateButtonGroup();

			ButtonGroupData buttonGroupData = m_ButtonGroupController.GetButtonGroupData();
			ButtonGroupVisual buttonGroup = m_ButtonGroupController.GetButtonGroupVisual();

			if (m_IconButton != null && m_IconButton.image != null && buttonGroupData != null)
			{
				m_IconButton.image.sprite = buttonGroupData.image;
				m_IconButton.image.color = buttonGroupData.color;
			}

			if (buttonGroup != null)
			{
				Button specialButton = buttonGroup.specialButton != null ? buttonGroup.specialButton.button : null;
				
				if (specialButton != null)
				{
					if (m_IconButton != null && specialButton != null)
					{
						m_IconButton.interactable = specialButton.interactable;
						m_IconButton.onClick.RemoveAllListeners();
						m_IconButton.onClick.AddListener(() => specialButton.onClick.Invoke());
					}

					if (buttonGroup.specialButton is ButtonRV buttonRV)
					{
						buttonRV.OnRVLoaded += OnRVLoaded;
						buttonRV.OnRVCompleted += OnRewardedVideoCompleted;
					}

					buttonGroup.SpecialButtonDelayEnded.AddListener(OnSpecialButtonDelayEnded);

					TextMeshProUGUI textMeshProUGUI =
						buttonGroup.specialButton.GetComponentInChildren<TextMeshProUGUI>(true);
					if (textMeshProUGUI)
					{
						textMeshProUGUI.text = buttonGroupData.text;
					}
				}
			}
		}

		private void OnSpecialButtonDelayEnded()
		{
			if (m_IconButton != null)
				m_IconButton.interactable = false;
		}

		private void OnRVLoaded(bool _isLoaded)
		{
			if (m_IconButton != null)
				m_IconButton.interactable = _isLoaded;
		}
	}
}