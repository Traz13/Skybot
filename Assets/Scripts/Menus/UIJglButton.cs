using UnityEngine;
using System.Collections;

public class UIJglButton : UIButton
{
	public delegate void DidPress(UIJglButton button);
	public event DidPress didPress;
	
	public UISprite background;
	public UILabel label;
	
	public object userData;
	public bool isDown;

	
	protected override void OnPress(bool isPressed)
	{
		base.OnPress(isPressed);
		
		isDown = isPressed;
			
		if( didPress != null )
			didPress(this);
	}
}

