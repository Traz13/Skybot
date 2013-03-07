using UnityEngine;
using System.Collections;

public class Oblongabomb : Projectile 
{
	public float torque = 200f;
	public int bouncesRemaining = 3;
	
	public GameObject meshObject;
	
	
	void Awake() 
	{
		collider.material.bounceCombine = PhysicMaterialCombine.Maximum;
		collider.material.bounciness = 0.8f;
	}
	
	
	void Update()
	{
		if( meshObject != null )
			meshObject.transform.Rotate(new Vector3(0f, 0f, -1000f*Time.deltaTime));
	}
	
	
	void OnCollisionExit(Collision collision)
	{
		bouncesRemaining--;
		if( bouncesRemaining <= 0 || collision.gameObject.GetComponent<Player>() != null )
		{
			if( exploder != null )
				exploder.Explode();
		}
	}
}
