using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour 
{
	public delegate void DidExplode(Projectile projectile);
	public event DidExplode didExplode;
	
	static int numCreated = 0;
	
	public Launcher launcher;
	
	
#region CONSTRUCTORS
	
	
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
	
	
#endregion
#region UNITY_HOOKS
	
	
	/// <summary>
	/// Physics update.
	/// </summary>
	
	void FixedUpdate()
	{
		// TODO: Calculate where we'll be in the next couple time steps
		// and test future collision with slowmo zones.
		//Vector3[] positions = Trajectory.PredictPositions(transform.position, rigidbody.velocity, 3, Time.fixedDeltaTime);
		
	}
	
	
	/// <summary>
	/// Callback for trigger exit.
	/// </summary>
	
	void OnTriggerExit(Collider otherCollider)
	{
		// Destroy the projectile automatically when it exits the game world.
		if( otherCollider.gameObject == Game.instance.gameObject )
			Destroy(gameObject);
	}
	
	
#endregion
}
