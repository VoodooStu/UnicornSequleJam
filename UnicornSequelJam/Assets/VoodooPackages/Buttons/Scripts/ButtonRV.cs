using UnityEngine;

namespace VoodooPackages.Tech.Buttons
{
	[AddComponentMenu("VoodooSauce/Rewarded Video Button")]
	public class ButtonRV : ButtonHandler
	{
		public delegate void BooleanRV(bool _value);

		public event BooleanRV OnRVCompleted;
		public event BooleanRV OnRVLoaded;

		protected override void Start()
		{
			base.Start();

#if VOODOO_SAUCE
			VoodooSauce.SubscribeOnRewardedVideoLoaded(OnRewardedVideoLoadComplete);
#else
			OnRewardedVideoLoadComplete(true);
#endif
		}
		
		protected override void OnButtonClicked()
		{
#if VOODOO_SAUCE
			VoodooSauce.ShowRewardedVideo(OnRewardedVideoCompleted);
#elif UNITY_EDITOR
			OnRewardedVideoCompleted(true);
#endif
		}

		private void OnRewardedVideoLoadComplete(bool _isLoaded)
		{
			OnRVLoaded?.Invoke(_isLoaded);
		}

		private void OnRewardedVideoCompleted(bool _completed)
		{
			OnRVCompleted?.Invoke(_completed);
		}
	}
}