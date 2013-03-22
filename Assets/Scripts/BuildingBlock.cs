using UnityEngine;
using System.Collections;

public class BuildingBlock : MonoBehaviour 
{	
	bool lightUp = false;
	float lightTimer = 0;
	float lightDuration = 0.5f;
	
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	
	void Update()
	{
		if( lightUp )
		{
			lightTimer += Time.deltaTime;
			
			float lightAmount = 0;
			if( lightTimer > lightDuration )
				lightUp = false;
			else
			{			
				// Adjust the light smoothly up and down along a single sin wave.
				lightAmount = Mathf.Sin(Mathf.Lerp(0, Mathf.PI, lightTimer / lightDuration));
			}
			
			// HACK: This will currently only work on vertex lit materials.
			renderer.material.SetColor("_Emission", new Color(lightAmount, lightAmount, lightAmount, 1));
		}
	}
	
	
	/// <summary>
	/// Collision enter
	/// </summary>
	
	void OnCollisionEnter(Collision collision)
	{
		if( !collision.gameObject.GetComponent<Projectile>() )
			return;
		
		lightUp = true;
		lightTimer = 0;
	}
}
