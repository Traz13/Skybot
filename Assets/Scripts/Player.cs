using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour 
{	
	public enum PlayerMode {
		Move,
		Fire,
	}
	
#region 	EVENTS
	
	
	public delegate void DidDie(Player player);
	public event DidDie didDie;
	
	
#endregion
#region 	VARIABLES
	
	
	// Player Index
	public int playerIndex = 0;
	
	// Score
	public int score = 0;
	
	// Whether it's my turn or not.
	public bool isMyTurn {
		get { return (this == Game.Instance.currentPlayer); }
	}
	
	// Player Mode (eg. Fire, Move)
	PlayerMode mMode = PlayerMode.Fire;
	public PlayerMode mode {
		get { return mMode; }
		set { SetMode(value); }
	}
	
	[HideInInspector]
	public Launcher launcher;
	
	[HideInInspector]
	public bool aiming = false;	
	
	public int trajectorySamples = 50;
	
	LineRenderer lineRenderer;
	Vector3 downPoint;
	Vector3 aim;
	float velocity = 0f;
	
	
#endregion
#region 	UNITY_HOOKS
	
	
	/// <summary>
	/// Awake this instance.
	/// </summary>
	
	void Awake()
	{
		// Line Renderer
		lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.SetVertexCount(trajectorySamples);
		lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
		lineRenderer.SetColors(Color.red, new Color(1f, 0f, 0f, 0.0f));
		lineRenderer.SetWidth(0.2f, 0.2f);
		
		// Grab our launcher instance (if we have one).
		launcher = gameObject.GetComponent<Launcher>();
		
		Damageable damageable = gameObject.GetComponent<Damageable>();
		if( damageable )
			damageable.willDie += willDie;
	}
	
	
	/// <summary>
	/// Start this instance.
	/// </summary>
	
	void Start()
	{
		Game.Instance.turnWillBegin += gameTurnWillBegin;
	}
	
	
	void willDie(Damageable damageable)
	{
		rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;
		rigidbody.AddForce(damageable.lastCollision.relativeVelocity);
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
		if( !isMyTurn )
			return;
		
		aiming = true;
		downPoint = Input.mousePosition;
		
		Game.Instance.FocusOnCurrentPlayer();
	}
	
	
	/// <summary>
	/// Collider mouse drag event.
	/// </summary>
	
	void OnMouseDrag()
	{
		if( !isMyTurn )
			return;
		
		Game rules = Game.Instance;
		
		aim = downPoint - Input.mousePosition;
		velocity = Mathf.Min(0.5f * aim.magnitude, (mode == PlayerMode.Fire) ? rules.maxFireVelocity : rules.maxMoveVelocity);
		aim.Normalize();
	}
	
	
	/// <summary>
	/// Collider mouse up event.
	/// </summary>
	
	void OnMouseUp()
	{
		if( !isMyTurn )
		{
			// TODO: Fade in player info with a timer to fade back out.
			return;
		}
		
		// If there's little or no velocity, count it as a tap and switch modes.
		if( velocity < 0.001f )
		{
			mode = (mode == PlayerMode.Fire) ? PlayerMode.Move : PlayerMode.Fire;
		}
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
	
	
	void OnCollisionEnter(Collision collision)
	{
		// TODO : Take damage on collision with a projectile.
	}
	
	
#endregion
#region 	METHODS
	
	
	/// <summary>
	/// Games the turn will begin.
	/// </summary>
	
	void gameTurnWillBegin(Game game)
	{
		// Keep players from intersecting buildings during their shift.
		rigidbody.WakeUp();
	}
	
	
	/// <summary>
	/// Sets the player mode (eg. Fire, Move).
	/// </summary>
	
	public void SetMode(PlayerMode _mode)
	{
		mMode = _mode;
		if( mode == PlayerMode.Fire )
			lineRenderer.SetColors(Color.red, new Color(1f, 0f, 0f, 0f));
		else if( mode == PlayerMode.Move )
			lineRenderer.SetColors(Color.blue, new Color(0f, 0f, 1f, 0f));
	}
	

#endregion
}
