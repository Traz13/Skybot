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
}

