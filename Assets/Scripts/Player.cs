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
	int mPlayerIndex = 0;
	public int playerIndex {
		get { return mPlayerIndex; }
		set { mPlayerIndex = value; }
	}
	
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
		if( aiming )
		{
			Vector3[] points = Trajectory.PredictPositions(transform.position, aim, velocity, trajectorySamples, Time.fixedDeltaTime/Time.timeScale);
			
			lineRenderer.SetVertexCount(trajectorySamples);
			for( int i = 0; i < points.Length; i++ )
				lineRenderer.SetPosition(i, points[i]);
		}
	}
	
	
	/// <summary>
	/// Collider mouse down event.
	/// </summary>
	
	void OnMouseDown()
	{
		// Only allow the current player to aim.
		if( !Game.Instance.rules.AllowUserAction(gameObject) )
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
		
		aim = downPoint - Input.mousePosition;
		velocity = Mathf.Min(0.5f * aim.magnitude, (mode == PlayerMode.Fire) ? rules.maxFireVelocity : rules.maxMoveVelocity);
		aim.Normalize();
	}
	
	
	/// <summary>
	/// Collider mouse up event.
	/// </summary>
	
	void OnMouseUp()
	{
		if( !aiming )
			return;
		
		// If there's little or no velocity, count it as a tap and switch modes.
		if( velocity < 0.001f )
			mode = (mode == PlayerMode.Fire) ? PlayerMode.Move : PlayerMode.Fire;
		else
		{
			if( mode == PlayerMode.Fire )
				launcher.FireProjectile(aim*velocity);
			else if( mode == PlayerMode.Move )
				rigidbody.velocity = aim*velocity;
		}
		
		aiming = false;
		aim = Vector3.zero;
		velocity = 0f;
		
		// Clear trajectory lines.
		for( int i = 0; i < trajectorySamples; i++ )
			lineRenderer.SetPosition(i, Vector3.zero);
	}
	
	
#endregion
#region 	METHODS
	
	
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
