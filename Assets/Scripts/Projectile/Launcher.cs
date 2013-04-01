using UnityEngine;
using System.Collections;

public class Launcher : MonoBehaviour
{	
	public delegate void DidFireProjectile(Launcher launcher, Projectile projectile);
	public event DidFireProjectile didFireProjectile;
	
	public GameObject projectileOriginal;
	
	
	/// <summary>
	/// Fire a projectile with the specified force.
	/// </summary>
	
	public Projectile FireProjectile(Vector3 force)
	{		
		Projectile projectile = Projectile.Create(this, force);
		
		if( didFireProjectile != null )
			didFireProjectile(this, projectile);
		
		return projectile;
	}
	
	
	public Projectile FireProjectile(Vector3 force, Vector2 randomness)
	{
		Vector3 randMin = -randomness * 0.5f;
		Vector3 randMax = -randMin;
		Vector3 offset = new Vector3(Random.Range(randMin.x, randMax.x), Random.Range(randMin.y, randMax.y), 0f);
		
		return FireProjectile(force - offset);
	}
	
	
	public Projectile FireProjectile(Vector3 force, float randomness)
	{
		return FireProjectile(force, new Vector2(randomness, randomness));
	}
}

