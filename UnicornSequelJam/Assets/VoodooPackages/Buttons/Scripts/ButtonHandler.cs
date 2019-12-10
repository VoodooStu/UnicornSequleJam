using UnityEngine;
using UnityEngine.UI;

namespace VoodooPackages.Tech.Buttons
{
	[RequireComponent(typeof(Button))]
	public abstract class ButtonHandler : MonoBehaviour
	{
		public Button button;

		protected virtual void Start()
		{
			button.onClick.AddListener(OnButtonClicked);
		}

		protected abstract void OnButtonClicked();
	}
}