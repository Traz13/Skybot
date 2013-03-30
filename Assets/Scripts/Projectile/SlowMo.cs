using UnityEngine;
using System.Collections;

public class SlowMo : StaticInstance<SlowMo>
{		
	public delegate void DidStart(SlowMo slowmo);
	public event DidStart didStart;
	
	public delegate void DidStop(SlowMo slowmo);
	public event DidStop didStop;
	
	public bool armed = true;
	bool mOn = false;
	public bool on {
		get { return mOn; }
		set { 
			if( !mOn && value )
			{
				if( didStart != null )
					didStart(this);
			}
			else if( mOn && !value )
			{
				if( didStop != null )
					didStop(this);
			}
			
			mOn = value;
		}
	}
	public float timer = 0f;
	public float duration = 0.2f;
	
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	
	void Update()
	{
		if( armed && on )
		{
			timer += Time.deltaTime;
			
			if( timer < 0.5f )
			{
				Time.fixedDeltaTime = 0.003f;
				Time.timeScale = 0.15f;
			}
			else if( timer > duration )
			{
				Time.fixedDeltaTime = 0.02f;
				Time.timeScale = 1f;
				on = false;
				timer = 0f;
			}
		}
	}
}

