using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Damager : MonoBehaviour
{
	public float damage = 25f;
	public bool scaleWithForce = false;
	
	Hashtable hits = new Hashtable();
	
	
	void OnCollisionEnter(Collision collision)
	{
		// Only allow damage on objects with a Damageable component, who
		// we haven't already dealt damage to.
		Damageable damageable = collision.gameObject.GetComponent<Damageable>();
		if( damageable == null || hits.ContainsKey(damageable) )
			return;
		
		damageable.TakeDamage(damage);
		
		hits.Add(damageable, damageable);
	}
}

