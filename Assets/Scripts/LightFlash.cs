using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Light))]
public class LightFlash : MonoBehaviour
{
	// On
	public bool On = false;
	
	// Duration
	public float Duration = 0.75f;

	// Repeats
	public bool Repeats = false;
	
	float mTimer = 0;
	float mInitialIntensity;
	
	
	/// <summary>
	/// Awake this instance.
	/// </summary>
	
	void Awake()
	{
		mInitialIntensity = light.intensity;
	}
	
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	
	void Update()
	{
		if( On )
		{
			mTimer += Time.deltaTime;
			
			if( mTimer > Duration )
			{
				if( Repeats )
					mTimer = 0;
				else
					On = false;
			}
			else
			{			
				// Adjust the light smoothly up and down along a single sin wave.
				float lightUpAmount = Mathf.Sin(Mathf.Lerp(0, Mathf.PI, mTimer / Duration));
				light.intensity = mInitialIntensity + 7*lightUpAmount;
			}
		}
	}
	
	
	public void ResetTimer()
	{
		mTimer = 0;
	}
}

