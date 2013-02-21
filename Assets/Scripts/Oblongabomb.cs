using UnityEngine;
using System.Collections;

public class Oblongabomb : Projectile 
{
	public float torque = 200f;
	public int bouncesRemaining = 1;
	
	void Awake() 
	{
		rigidbody.maxAngularVelocity = torque;
		rigidbody.AddTorque(new Vector3(0f, 0f, -torque)/Time.timeScale, ForceMode.Impulse);
		
		//collider.material.bounceCombine = PhysicMaterialCombine.Maximum;
		//collider.material.bounciness = 1f;
	}
	
	
	void OnCollisionEnter(Collision collision)
	{
		bouncesRemaining--;
		if( bouncesRemaining <= 0 )
		{
			if( exploder != null )
				exploder.Explode();
		}
	}
}
