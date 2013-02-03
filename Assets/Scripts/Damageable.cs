using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Damageable : MonoBehaviour
{
	public delegate void WillDie(Damageable damageable);
	public event WillDie willDie;
	
	public float health = 100f;
	
	public void TakeDamage(float damage)
	{
		health -= damage;
		
		if( health <= 0f )
		{
			if( willDie != null )
				willDie(this);
		}
	}
}

