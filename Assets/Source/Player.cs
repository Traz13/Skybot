using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour 
{	
#region 	EVENTS
	
	
	public delegate void DidDie(Player player);
	public event DidDie didDie;
	
	
#endregion
#region 	VARIABLES
	

	public int playerIndex = 0;
	
	public int points = 0;

	float mHealth = 100f;
	public float health {
		get { return mHealth; }
		set {
			mHealth = value;
			if( mHealth <= 0f )
			{
				if( didDie != null )
					didDie(this);
				
				gameObject.SetActive(false);
			}
		}
	}
	
	
	public bool isMyTurn {
		get { return (this == Game.instance.GetCurrentPlayer()); }
	}
	
	[HideInInspector]
	public Launcher launcher;
	
	LineRenderer lineRenderer;
	public bool aiming = false;
	Vector3 downPoint;
	Vector3 aim;
	float strength = 0f;
	
#endregion
#region 	UNITY_HOOKS
	
	
	/// <summary>
	/// Awake this instance.
	/// </summary>
	
	void Awake()
	{
		lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.SetPosition(0, Vector3.zero);
		lineRenderer.SetPosition(1, Vector3.zero);
		lineRenderer.SetColors(Color.red, new Color(1f, 0f, 0f, 0.1f));
		lineRenderer.SetWidth(0.25f, 0.25f);
		
		launcher = gameObject.GetComponent<Launcher>();
	}
	
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	
	void Update()
	{
		if( aiming )
		{
			lineRenderer.SetPosition(0, transform.position);
			lineRenderer.SetPosition(1, transform.position + aim*strength*0.1f);
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
		
		Game.instance.FocusOnCurrentPlayer();
	}
	
	
	/// <summary>
	/// Collider mouse drag event.
	/// </summary>
	
	void OnMouseDrag()
	{
		if( !isMyTurn )
			return;
		
		aim = downPoint - Input.mousePosition;
		strength = aim.magnitude;
		
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
		
		launcher.FireProjectile(aim * strength * 10f);
		
		aiming = false;
		aim = Vector3.zero;
		strength = 0f;
		
		lineRenderer.SetPosition(0, Vector3.zero);
		lineRenderer.SetPosition(1, Vector3.zero);
	}
	
	
#endregion
}
