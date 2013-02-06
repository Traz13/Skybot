using UnityEngine;
using System.Collections;

[AddComponentMenu("Skybot/Launcher")]
public class Launcher : MonoBehaviour
{
	static int totalShotsFired = 0;
	
	public delegate void DidFireProjectile(Launcher launcher, Projectile projectile);
	public event DidFireProjectile didFireProjectile;
	
	public GameObject projectileOriginal;
	
	public int shotsRemaining = 0;
	
	
	/// <summary>
	/// Fire a projectile with the specified force.
	/// </summary>
	
	public Projectile FireProjectile(Vector3 force)
	{
		if( shotsRemaining <= 0 )
			return null;
		
		GameObject projectileObject = GameObject.Instantiate(projectileOriginal) as GameObject;
		projectileObject.name = "projectile" + totalShotsFired.ToString();
		projectileObject.transform.position = this.transform.position;
		projectileObject.rigidbody.velocity = force;// * Time.timeScale;
		//projectileObject.rigidbody.AddForce(force/Time.timeScale);
		
		Projectile projectile = projectileObject.AddComponent<Projectile>();
		projectile.launcher = this;
		
		// Make sure not to collide with the object we're launching from.
		Physics.IgnoreCollision(this.collider, projectileObject.collider);
		
		if( didFireProjectile != null )
			didFireProjectile(this, projectile);
		
		totalShotsFired++;
		
		return projectile;
	}
}

