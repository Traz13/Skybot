using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour 
{	
	public enum PlayerMode {
		Move,
		Fire,
	}

	
#region 	VARIABLES
	
	
	// Player Index
	public int index = 0;
	
	// Score
	public int score = 0;
	
	// Player Mode (eg. Fire, Move)
	PlayerMode mMode = PlayerMode.Fire;
	public PlayerMode mode {
		get { return mMode; }
		set { SetMode(value); }
	}
	
	// Color
	public Color color;
	
	// Launcher
	public Launcher launcher;
	
	// Jetpack
	public float fuel = 0f;
	float fuelCapacity = 1f;
	
	
	// Shots
	public int shotsRemaining = 0;
	
	// Headshot Collider
	public Collider headshotCollider;
	
	// Aiming Stuff
	public bool aiming = false;	
	public int trajectorySamples = 50;
	Vector3 downPoint;
	Vector3 aim;
	float velocity = 0f;
	LineRenderer lineRenderer;
	
	
#endregion
#region 	UNITY_HOOKS
	
	
	/// <summary>
	/// Awake this instance.
	/// </summary>
	
	void Awake()
	{
		// HACK: Make sure the game is initialized.
		Game game = Game.Instance;
		if( game == null )
			throw new System.Exception("Game doesn't exist");
		
		// Line Renderer
		lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.SetVertexCount(trajectorySamples);
		lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
		lineRenderer.SetColors(Color.red, new Color(1f, 0f, 0f, 0.0f));
		lineRenderer.SetWidth(0.2f, 0.2f);
		
		Damageable damageable = gameObject.GetComponent<Damageable>();
		if( damageable )
			damageable.willDie += willDie;
		
		// Set the material color
		gameObject.renderer.material.color = color;
	}
	
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	
	void Update()
	{
		// Show aim trajectory.
		if( aiming )
		{
			Vector3[] points = Trajectory.PredictPositions(transform.position, aim, velocity, trajectorySamples, Time.fixedDeltaTime/Time.timeScale);
			
			lineRenderer.SetVertexCount(trajectorySamples);
			for( int i = 0; i < points.Length; i++ )
				lineRenderer.SetPosition(i, points[i]);
		}
		
		// Movement
		Vector3 thrust = Vector3.zero;
		if( Game.Instance.rules.AllowUserAction(gameObject) && shotsRemaining <= 0 && fuel > 0 )
		{
			if( Input.GetKey(KeyCode.W) )
				thrust.y += 3000f * Time.deltaTime;
			if( Input.GetKey(KeyCode.A) )
				thrust.x -= 500f * Time.deltaTime;
			if( Input.GetKey(KeyCode.D) )
				thrust.x += 500f * Time.deltaTime;
			
			fuel -= (Mathf.Abs(thrust.x) + Mathf.Abs(thrust.y)) * 0.00005f;
			
			rigidbody.AddForce(thrust);
		}
		
		LightFlash lightFlash = GetComponent<LightFlash>();
		if( lightFlash != null )
		{
			light.enabled = (thrust.y > 0);
			
			float fuelSpentRatio = (fuelCapacity - fuel) / fuelCapacity;
			light.color = new Color(1f-fuelSpentRatio/2, 1f-fuelSpentRatio, 1f-fuelSpentRatio);
			lightFlash.On = light.enabled;
			if( !light.enabled )
				lightFlash.ResetTimer();
		}
			
	}
	
	
	/// <summary>
	/// Collider mouse down event.
	/// </summary>
	
	void OnMouseDown()
	{
		// Only allow the current player to aim.
		if( shotsRemaining <= 0 || !Game.Instance.rules.AllowUserAction(gameObject) )
			return;
		
		aiming = true;
		downPoint = Input.mousePosition;
	}
	
	
	/// <summary>
	/// Collider mouse drag event.
	/// </summary>
	
	void OnMouseDrag()
	{
		if( !aiming )
			return;
		
		Rules rules = Game.Instance.rules;
		
		float maxVelocity = (mode == PlayerMode.Fire) ? rules.maxFireVelocity : rules.maxMoveVelocity;
		
		aim = downPoint - Input.mousePosition;
		velocity = Mathf.Min(0.5f * aim.magnitude, maxVelocity);
		aim.Normalize();
	}
	
	
	/// <summary>
	/// Collider mouse up event.
	/// </summary>
	
	void OnMouseUp()
	{
		if( !aiming )
			return;
		
		// If there's little or no velocity, don't do anything.
		if( velocity < 0.001f )
			return;
		else
		{
			if( launcher != null )
			{
				Projectile projectile = launcher.FireProjectile(aim*velocity);
				Messenger.ReportCameraFollowEvent(projectile.gameObject, 10f);
				//CameraPosition.Instance.Follow(projectile.gameObject, 10f);
			}
			else
			{
				Debug.LogError("No Launcher found in " + name);
				return;
			}
		}
		
		aiming = false;
		aim = Vector3.zero;
		velocity = 0f;
		shotsRemaining--;
		
		// Clear trajectory lines.
		for( int i = 0; i < trajectorySamples; i++ )
			lineRenderer.SetPosition(i, Vector3.zero);
	}
	
	
	/// <summary>
	/// Collision enter event.
	/// </summary>
	
	void OnCollisionEnter(Collision collision)
	{
		if( collision.gameObject.GetComponent<Projectile>() )
		{
			foreach( ContactPoint contact in collision.contacts )
			{
				if( contact.thisCollider == headshotCollider )
				{
					Damageable damageable = GetComponent<Damageable>();
					if( damageable != null )
						damageable.TakeDamage(100, collision);
					
					Debug.Log("HEADSHOT!!!");
					break;
				}
				else
				{
					Damageable damageable = GetComponent<Damageable>();
					if( damageable != null )
						damageable.TakeDamage(50, collision);
					
					break;
				}
			}
		}
	}
	
	
#endregion
#region 	METHODS
	
	
	/// <summary>
	/// Refills the fuel.
	/// </summary>
	
	public void RefillFuel()
	{
		fuel = fuelCapacity;
	}
	
	
	/// <summary>
	/// Callback for Damageable component.
	/// </summary>
	
	void willDie(Damageable damageable)
	{
		// Remove rigidbody constraints to allow the player to tumble and fall.
		rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | 
								RigidbodyConstraints.FreezeRotationY | 
								RigidbodyConstraints.FreezePositionZ;
		
		// Make sure the force gets to the rigidbody correctly now that 
		// we've removed our constraints.
		if( damageable.lastCollision != null )
			rigidbody.AddForce(damageable.lastCollision.relativeVelocity);
	}
	
	
	/// <summary>
	/// Sets the player mode (eg. Fire, Move).
	/// </summary>
	
	public void SetMode(PlayerMode _mode)
	{
		// Change the trajectory line color depending on our mode.
		// TODO: Show this in the player himself with a fresnel glow. Or maybe 
		//		 remove mode-switching entirely, using rocket jumps instead to move.
		mMode = _mode;
		if( mode == PlayerMode.Fire )
			lineRenderer.SetColors(Color.red, new Color(1f, 0f, 0f, 0f));
		else if( mode == PlayerMode.Move )
			lineRenderer.SetColors(Color.blue, new Color(0f, 0f, 1f, 0f));
	}
	

#endregion
}
