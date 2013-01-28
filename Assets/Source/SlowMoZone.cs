using UnityEngine;
using System.Collections;

public class SlowMoZone : MonoBehaviour
{
	void OnTriggerEnter(Collider collider)
	{
		if( SlowMo.instance == null )
		{
			Debug.LogError("There's no SlowMo component in the game!");
			return;
		}
		
		// Only react to projectiles that aren't our own.
		if( collider.gameObject == transform.parent.gameObject || 
			collider.gameObject.GetComponent<Projectile>() == null )
			return;

		SlowMo.instance.on = true;
	}
	
	void OnTriggerExit(Collider collider)
	{
		if( SlowMo.instance == null )
		{
			Debug.LogError("There's no SlowMo component in the game!");
			return;
		}
		
		if( collider.gameObject == transform.parent.gameObject || 
			collider.gameObject.GetComponent<Projectile>() == null )
			return;
		
		SlowMo.instance.timer = SlowMo.instance.duration;
	}
}

