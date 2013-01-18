using UnityEngine;
using System.Collections;

public class SlowMoZone : MonoBehaviour
{
	void OnTriggerEnter(Collider collider)
	{
		if( collider.gameObject.GetComponent<Projectile>() != null )
		{
			if( collider.gameObject != this.gameObject )
				Game.instance.slowMo = true;
		}
	}
	
	void OnTriggerExit(Collider collider)
	{
		if( collider.gameObject.GetComponent<Projectile>() != null )
		{
			if( collider.gameObject != this.gameObject )
				Game.instance.slowMoTimer = Game.instance.slowMoTimerMax;
		}
	}
}

