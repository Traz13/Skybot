using UnityEngine;
using System.Collections;

public class UIMainMenu : MonoBehaviour
{
	public UIJglButton startButton;
	
	
	void Awake()
	{
		startButton.didPress += startButtonDidPress;
	}
	
	
	void startButtonDidPress(UIJglButton button)
	{
		if( button.isDown )
			return;
		
		Application.LoadLevel(1);
		
		// Pan the camera down to the playing area.
		
		gameObject.SetActive(false);
	}
}

