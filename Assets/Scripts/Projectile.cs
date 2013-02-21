using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour 
{
	static int numCreated = 0;
	
	Launcher mLauncher;
	public Launcher launcher {
		get { return mLauncher; }
		set { mLauncher = value; }
	}
	
	public Exploder exploder;
	
#region CONSTRUCTORS
	
	
	/// <summary>
	/// Create a projectile, fired from the specified launcher.
	/// </summary>
	
	public static Projectile Create(Launcher launcher)
	{
		GameObject newObject = GameObject.Instantiate(launcher.projectileOriginal) as GameObject;
		newObject.gameObject.name = "projectile" + numCreated.ToString();
		newObject.transform.position = launcher.transform.position;
		
		Projectile projectile = newObject.GetComponent<Projectile>();
		projectile.launcher = launcher;
		
		if( projectile.exploder == null )
			projectile.exploder = projectile.GetComponent<Exploder>();
		
		// Make sure not to collide with the launcher's owner.
		Physics.IgnoreCollision(launcher.transform.parent.collider, projectile.collider);
		
		return projectile;
	}
	
	
	/// <summary>
	/// Create a projectile, fired from the specified launcher with given velocity.
	/// </summary>
	
	public static Projectile Create(Launcher launcher, Vector3 velocity)
	{
		Projectile projectile = Projectile.Create(launcher);
		projectile.rigidbody.velocity = velocity;
		
		return projectile;
	}
	
	
#endregion
#region UNITY_HOOKS
	
	
	/// <summary>
	/// Start this instance.
	/// </summary>
	
	void Start()
	{
		if( exploder != null )
			exploder.didExplode += didExplode;
	}
	
	
	/// <summary>
	/// Physics update.
	/// </summary>
	
	void FixedUpdate()
	{
		// For now, just explode a projectile as it comes to rest.
		if( rigidbody.IsSleeping() )
		{
			if( exploder != null )
				exploder.Explode();
		}
		
		
		// TODO: Calculate where we'll be in the next couple time steps
		// and test future collision with slowmo zones.
		//Vector3[] positions = Trajectory.PredictPositions(transform.position, rigidbody.velocity, 3, Time.fixedDeltaTime);
	}
	
	
#endregion
	
	
	/// <summary>
	/// Callback for Exploder.didExplode
	/// </summary>
	
	void didExplode(Exploder exploder)
	{
		ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
		if( particleSystem )
		{
			particleSystem.transform.parent = null;
			Destroy(particleSystem.gameObject, 1f);
		}
		
		Destroy(gameObject);
	}
}
