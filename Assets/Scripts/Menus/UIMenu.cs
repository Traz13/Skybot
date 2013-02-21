using UnityEngine;
using System.Collections;

public class UIMenu : MonoBehaviour
{
	public enum AnimationStyle {
		None,
		Scale,
		//Slide,
		//Flip,
	}
	
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
#region 	VARIABLES
	
	public AnimationStyle animationStyle = AnimationStyle.Scale;
	
	// Duration (0 == infinity).
	public float duration = 2f;
	float durationTimer = 0f;
	bool durationTimerOn = false;
	
	public Transform cameraTransform;
	
#endregion
#region 	UNITY_HOOKS
	
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	
	protected virtual void Update()
	{
		if( durationTimerOn )
		{
			// Hide the menu once our duration has elapsed.
			durationTimer += Time.deltaTime;
			if( durationTimer >= duration )
			{
				// Deactivate the timer and reset it.
				durationTimerOn = false;
				durationTimer = 0f;
				
				// Hide the menu.
				Hide(animationStyle);
			}
		}
	}
	

#endregion
#region 	METHODS
	
	
	/// <summary>
	/// Show the menu with the current animation style.
	/// </summary>
	
	public void Show() 
	{
		Show(animationStyle);
	}
	
	
	/// <summary>
	/// Hide the menu with the current animation style.
	/// </summary>
	
	public void Hide()
	{
		Hide(animationStyle);
	}
	
	
	/// <summary>
	/// Show the menu.
	/// </summary>
	
	public virtual void Show(AnimationStyle style)
	{
		if( willShow != null )
			willShow(this);
		
		switch( style )	
		{
			case AnimationStyle.None:
			{
				// Just instantly scale the menu up to full size and send the didShow event.
				transform.localScale = Vector3.one;
				gameObject.SetActive(true);
				if( didShow != null )
					didShow(this);
			
				break;
			}
			case AnimationStyle.Scale:
			{
				transform.localScale = Vector3.zero;
				gameObject.SetActive(true);
				
				// Scale the whole menu up from nothing with a slight bounce.
				TweenScale.Begin(gameObject, 0.15f, new Vector3(1.1f, 1.1f, 1.1f)).onFinished += delegate(UITweener tweenerA) 
				{
					TweenScale.Begin(gameObject, 0.05f, Vector3.one).onFinished += delegate(UITweener tweenerB) 
					{
						// Send the didShow only after all animations have completed.
						if( didShow != null )
							didShow(this);
					};
				};
			
				break;
			}
		}
		
		// Start the duration timer (if we have a duration).
		if( duration > 0f )
			durationTimerOn = true;
	}
	
	
	/// <summary>
	/// Hide the menu.
	/// </summary>
	
	public virtual void Hide(AnimationStyle style)
	{
		if( willHide != null )
			willHide(this);
		
		switch( style )
		{
			case AnimationStyle.None:
			{
				// Instantly scale the menu down to full size and send the didHide event.
				transform.localScale = Vector3.zero;
				if( didHide != null )
					didHide(this);
				
				// Deactivate the menu now that it's hidden.
				gameObject.SetActive(false);
			
				break;
			}
			case AnimationStyle.Scale:
			{
				// Scale the whole menu down to nothing with a slight bounce.
				TweenScale.Begin(gameObject, 0.15f, new Vector3(1.1f, 1.1f, 1.1f)).onFinished += delegate(UITweener tweenerA) 
				{
					TweenScale.Begin(gameObject, 0.05f, Vector3.zero).onFinished += delegate(UITweener tweenerB) 
					{
						// Send the didHide only after all animations have completed.
						if( didHide != null )
							didHide(this);
						
						// Deactivate the menu now that it's hidden.
						gameObject.SetActive(false);
					};
				};
			
				break;
			}
		}
	}
	
	
#endregion
}

