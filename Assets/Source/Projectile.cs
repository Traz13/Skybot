using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour 
{
	public delegate void DidExplode(Projectile projectile);
	public event DidExplode didExplode;
	
	static int numCreated = 0;
	
	public Launcher launcher;
	
	
	/// <summary>
	/// Create a projectile, fired from the specified launcher.
	/// </summary>
	
	public static Projectile Create(Launcher launcher)
	{
		GameObject newObject = GameObject.Instantiate(launcher.projectileOriginal) as GameObject;
		newObject.gameObject.name = "projectile" + numCreated.ToString();
		
		Projectile projectile = newObject.GetComponent<Projectile>();
		projectile.launcher = launcher;
		
		// Make sure not to collide with the launcher.
		Physics.IgnoreCollision(launcher.collider, projectile.collider);
		
		return projectile;
	}
}
