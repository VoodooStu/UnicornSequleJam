using UnityEngine;
using UnityEngine.UI;
using VoodooPackages.Tech.EasedAnimations;

namespace VoodooPackages.Tech.Buttons
{
	public class IconRV : MonoBehaviour
	{
		public Image icon;

		public Sprite rvIcon;
		public Sprite loadingIcon;

		public EasedAnimation genericAnimation;
 
		/// <summary>
		/// Update the display depending on the state of the Rewarded Video
		/// </summary>
		/// <param name="_isRVLoaded"></param>
		public void UpdateDisplay(bool _isRVLoaded)
		{
			if (_isRVLoaded)
			{
				icon.sprite = rvIcon;
				genericAnimation.StopAnimation();
				transform.localEulerAngles = Vector3.zero;
			}
			else
			{
				icon.sprite = loadingIcon;
				genericAnimation.StartAnimation();
			}
		}
		
	}
}