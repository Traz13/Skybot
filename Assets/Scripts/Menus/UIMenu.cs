using UnityEngine;
using System.Collections;

public class UIMenu : MonoBehaviour
{
#region 	EVENTS
	
	public delegate void WillShow(UIMenu menu);
	public delegate void DidShow(UIMenu menu);
	public delegate void WillHide(UIMenu menu);
	public delegate void DidHide(UIMenu menu);
	
	public event WillShow willShow;
	public event DidShow didShow;
	public event WillHide willHide;
	public event DidHide didHide;
	
#endregion
#region 	METHODS
	
	
	/// <summary>
	/// Show the menu.
	/// </summary>
	
	public virtual void Show(bool animated)
	{
		if( willShow != null )
			willShow(this);
		
		transform.localScale = Vector3.zero;
		gameObject.SetActive(true);
		
		if( animated )
		{
			TweenScale.Begin(gameObject, 0.15f, new Vector3(1.1f, 1.1f, 1.1f)).onFinished += delegate(UITweener tweenerA) 
			{
				TweenScale.Begin(gameObject, 0.05f, Vector3.one).onFinished += delegate(UITweener tweenerB) 
				{
					if( didShow != null )
						didShow(this);
				};
			};
		}
		else
		{
			transform.localScale = Vector3.one;
			if( didShow != null )
				didShow(this);
		}
	}
	
	
	/// <summary>
	/// Show the menu, animated by default.
	/// </summary>
	
	public void Show() 
	{
		Show(true);
	}
	
	
	/// <summary>
	/// Hide the menu.
	/// </summary>
	
	public virtual void Hide(bool animated)
	{
		if( willHide != null )
			willHide(this);
		
		if( animated )
		{
			TweenScale.Begin(gameObject, 0.15f, new Vector3(1.1f, 1.1f, 1.1f)).onFinished += delegate(UITweener tweenerA) 
			{
				TweenScale.Begin(gameObject, 0.05f, Vector3.zero).onFinished += delegate(UITweener tweenerB) 
				{
					if( didHide != null )
						didHide(this);
					
					gameObject.SetActive(false);
				};
			};
		}
		else
		{
			transform.localScale = Vector3.zero;
			if( didHide != null )
				didHide(this);
			
			gameObject.SetActive(false);
		}
	}
	
	
	/// <summary>
	/// Hide the menu, animated by default.
	/// </summary>
	
	public void Hide()
	{
		// Hide with animation by default.
		Hide(true);
	}
	
	
#endregion
}

