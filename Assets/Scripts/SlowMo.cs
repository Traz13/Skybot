using UnityEngine;
using System.Collections;

public class SlowMo : MonoBehaviour
{
	public static SlowMo instance;
	
	public bool armed = true;
	public bool on = false;
	public float timer = 0f;
	public float duration = 0.2f;
	
	
	/// <summary>
	/// Awake this instance.
	/// </summary>
	
	void Awake()
	{
		if( instance != null )
			throw new System.Exception("Only one instance of SlowMo is allowed!");
		
		instance = this;
	}
	
	
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

