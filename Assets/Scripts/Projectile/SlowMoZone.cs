using UnityEngine;
using System.Collections;

public class SlowMoZone : MonoBehaviour
{		
	void OnTriggerEnter(Collider collider)
	{
		if( SlowMo.Instance == null )
			return;
		
		// Only react to projectiles that aren't our own.
		Projectile projectile = collider.gameObject.GetComponent<Projectile>();
		if( projectile == null || projectile.launcher.transform.parent == transform.parent )
			return;

		SlowMo.Instance.on = true;
		
		GameMsg.FOVAdjust(35f, 500f*Time.deltaTime);
	}
	
	void OnTriggerExit(Collider collider)
	{
		if( SlowMo.Instance == null )
			return;
		
		// Only react to projectiles that aren't our own.
		Projectile projectile = collider.gameObject.GetComponent<Projectile>();
		if( projectile == null || projectile.launcher.transform.parent == transform.parent )
			return;
		
		SlowMo.Instance.timer = SlowMo.Instance.duration;
		
		GameMsg.FOVAdjust(60f, 500f*Time.deltaTime);
	}
}

