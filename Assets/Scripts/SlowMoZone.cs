using UnityEngine;
using System.Collections;

public class SlowMoZone : MonoBehaviour
{
	void OnTriggerEnter(Collider collider)
	{
		if( SlowMo.Instance == null )
		{
			Debug.LogError("There's no SlowMo component in the game!");
			return;
		}
		
		// Only react to projectiles that aren't our own.
		Projectile projectile = collider.gameObject.GetComponent<Projectile>();
		if( projectile == null || projectile.launcher.transform.parent == transform.parent )
			return;

		SlowMo.Instance.on = true;
	}
	
	void OnTriggerExit(Collider collider)
	{
		if( SlowMo.Instance == null )
		{
			Debug.LogError("There's no SlowMo component in the game!");
			return;
		}
		
		// Only react to projectiles that aren't our own.
		Projectile projectile = collider.gameObject.GetComponent<Projectile>();
		if( projectile == null || projectile.launcher.transform.parent == transform.parent )
			return;
		
		SlowMo.Instance.timer = SlowMo.Instance.duration;
	}
}

