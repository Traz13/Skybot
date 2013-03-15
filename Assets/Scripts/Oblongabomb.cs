using UnityEngine;
using System.Collections;

public class Oblongabomb : Projectile 
{
	public float torque = 200f;
	public int bouncesRemaining = 3;
	
	public GameObject meshObject;
	
	float frameOfLastCollision = float.MaxValue;
	
	
	/// <summary>
	/// Awake this instance.
	/// </summary>
	
	void Awake() 
	{
		collider.material.bounceCombine = PhysicMaterialCombine.Maximum;
		collider.material.bounciness = 0.8f;
		collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
		collider.material.dynamicFriction = 0f;
		collider.material.dynamicFriction2 = 0f;
	}
	
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	
	void Update()
	{
		if( meshObject != null )
			meshObject.transform.Rotate(new Vector3(0f, 0f, -1000f*Time.deltaTime));
	}
	
	
	/// <summary>
	/// Collision enter
	/// </summary>
	
	void OnCollisionEnter(Collision collision)
	{
		if( collision.gameObject.GetComponent<SlowMoZone>() )
			return;
		
		LightFlash lightFlash = GetComponent<LightFlash>();
		if( lightFlash != null )
		{
			lightFlash.ResetTimer();
			lightFlash.On = true;
		}
	}
	
	
	/// <summary>
	/// Collision exit
	/// </summary>
	
	void OnCollisionExit(Collision collision)
	{
		if( frameOfLastCollision == Time.frameCount )
			return;

		bouncesRemaining--;
		if( bouncesRemaining <= 0 || collision.gameObject.GetComponent<Player>() != null )
		{
			if( exploder != null )
			{
				// Create a temporary object to show the explosion.
				GameObject explosion = new GameObject("OblongabombExplosion");
				explosion.transform.position = transform.position;
				
				Light explosionLight = explosion.AddComponent<Light>();
				explosionLight.range = light.range;
				explosionLight.intensity = light.intensity;
				explosionLight.color = light.color;
				
				LightFlash lightFlash = explosion.AddComponent<LightFlash>();
				lightFlash.On = true;
				Destroy(explosion, lightFlash.Duration);
				
				exploder.Explode();
			}
		}
		
		frameOfLastCollision = Time.frameCount;
	}
}
