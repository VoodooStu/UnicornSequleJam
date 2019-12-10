namespace VoodooPackages.Tech.Buttons
{
	public class ButtonStandard : ButtonHandler
	{
		public delegate void ButtonClicked();
		public event ButtonClicked OnClicked;
	
		protected override void OnButtonClicked()
		{
			OnClicked?.Invoke();
		}
	}
}
