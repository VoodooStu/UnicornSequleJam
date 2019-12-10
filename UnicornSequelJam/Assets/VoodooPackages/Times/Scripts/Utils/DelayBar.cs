using UnityEngine.UI;

namespace VoodooPackages.Tech.Times
{
    public class DelayBar : Delay
    {
        private Image image;
	    public bool isInversed = false;

	    protected override void Initialize()
	    {
		    base.Initialize();
		    image = GetComponent<Image>();
		    image.enabled = false;
	    }

	    public override void StartCountdown()
        {
            base.StartCountdown();
            image.enabled = delay > 0;
        }

        protected override void Tick(Timer _timer)
	    {
		    UpdateImage(_timer.PastNormalized);

            base.Tick(_timer);
	    }

        private void UpdateImage(float _pourcentage)
        {
	        if (isInversed)
	        {
		        _pourcentage = 1f - _pourcentage;
	        }
	    
	        image.fillAmount = _pourcentage;
        }
    }
}