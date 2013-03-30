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
		
		// Make sure not to collide with the launcher's owner, or any of its children.
		foreach( Collider collider in launcher.transform.parent.GetComponentsInChildren<Collider>() )
		{
			if( collider.enabled && collider.gameObject.activeSelf )
				Physics.IgnoreCollision(collider, projectile.collider);
		}
		
		return projectile;
	}
	
	
	/// <summary>
	/// Create a projectile, fired from the specified launcher with given velocity.
	/// </summary>
	
	public static Projectile Create(Launcher launcher, Vector3 velocity)
	{
		Projectile projectile = Projectile.Create(launcher);
		projectile.rigidbody.velocity = ProjectileVelOffset(velocity);
		
		return projectile;
	}
			
	private static float ProjectileVelOffset(float val)
	{
		float offset = (float)Random.Range(0, (int)ProjectileInfo.ShotVariance) * 0.01f * val;
		return Random.Range(0,2) == 0 ?  val - offset : val + offset;
	}
		
	private static Vector3 ProjectileVelOffset(Vector3 val)
	{
			return new Vector3(ProjectileVelOffset(val.x), ProjectileVelOffset(val.y), 0f);
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
