using UnityEngine;

namespace VoodooPackages.Tech.Buttons
{
	public class ButtonStandardDemo : MonoBehaviour
    {
	    public ButtonStandard buttonStandard;

	    private void OnEnable()
	    {
		    if (buttonStandard != null)
		    {
			    buttonStandard.OnClicked += OnButtonClick;
		    }
	    }

	    private void OnDisable()
	    {
		    if (buttonStandard != null)
		    {
			    buttonStandard.OnClicked -= OnButtonClick;
		    }
	    }

	    private void OnButtonClick()
	    {
		    Debug.Log("Button Standard has been pressed");
	    }
    }
}