using UnityEngine;
using System.Collections;

public class Damageable : MonoBehaviour
{
	public delegate void WillDie(Damageable damageable);
	public event WillDie willDie;
	
	public float health = 100f;
	public Collision lastCollision;
	
	public void TakeDamage(float damage, Collision collision)
	{
		health -= damage;
		
		lastCollision = collision;
		
		if( health <= 0f )
		{
			if( willDie != null )
				willDie(this);
		}
	}
}

