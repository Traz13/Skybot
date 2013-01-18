using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour 
{
	// Singleton
	public static Game instance;
	
#region 	EVENTS
	
	
	public delegate void DidFinishTurn(Game game);
	public event DidFinishTurn didFinishTurn;
	
	
	public delegate void DidComplete(Game game);
	public event DidComplete didComplete;
	
	
#endregion
#region 	VARIABLES
	
	
	public Camera mainCamera;
	
	[HideInInspector]
	public Player[] players;
	
	public GameRules rules = new DeathmatchRules();
	
	public int shotsPerTurn = 3;
	int turnsTaken = 0;
	
	Dictionary<string,Projectile> projectilesFired = new Dictionary<string,Projectile>();
	
	
#endregion
#region 	UNITY_HOOKS
	
	
	/// <summary>
	/// Awake this instance.
	/// </summary>
	
	void Awake()
	{
		if( instance != null )
			throw new System.Exception("Only one instance of Game allowed!");
		
		Physics.gravity = new Vector3(0f, -20f, 0f);
		
		instance = this;
		
		// Gather and sort players by index.
		Player[] playerComps = GetComponentsInChildren<Player>(false);
		players = new Player[playerComps.Length];
		
		foreach( Player player in playerComps )
		{
			players[player.playerIndex] = player;
			player.GetComponent<Launcher>().didFireProjectile += projectileFired;
			player.didDie += playerDidDie;
		}
	}
	
	
	/// <summary>
	/// Start this instance.
	/// </summary>
	
	void Start()
	{
		BeginGame();
	}
	
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	
	public bool slowMo = false;
	public float slowMoTimer = 0f;
	public readonly float slowMoTimerMax = 0.2f;
	float slowMoAmount = 0f;
	
	void Update()
	{
		//if( Input.GetKeyDown(KeyCode.Space) )
		//	slowMo = true;
		
		if( slowMo )
		{
			slowMoTimer += Time.deltaTime;
			
			if( slowMoTimer < 0.5f )
			{
				Time.fixedDeltaTime = 0.003f;//Time.fixedDeltaTime/10f;//Mathf.Lerp(0.02f, 0.002f, slowMoAmount / 0.5f);
				Time.timeScale = 0.15f;//Time.timeScale/10f;//Mathf.Lerp(1f, 0.1f, slowMoAmount / 0.5f);
				//slowMoAmount += Time.deltaTime;
			}
			else if( slowMoTimer > slowMoTimerMax )
			{
				Time.fixedDeltaTime = 0.02f;//Time.fixedDeltaTime*10f;//Mathf.Lerp(0.002f, 0.02f, slowMoAmount / 0.5f);
				Time.timeScale = 1f;//Time.timeScale*10f;//Mathf.Lerp(0.1f, 1f, slowMoAmount / 3f);
				//slowMoAmount -= Time.deltaTime;
				slowMo = false;
				slowMoTimer = 0f;
			}
			
			/*if( slowMoAmount <= 0f )
			{
				slowMo = false;
				slowMoAmount = 0f;
			}*/
		}
	}
	
	
#endregion
#region METHODS
	
	
	/// <summary>
	/// Begins the game.
	/// </summary>
	
	public void BeginGame()
	{
		// TODO: Show exciting intro graphics.
		
		BeginTurn();
	}
	
	
	/// <summary>
	/// Ends the game.
	/// </summary>
	
	public void EndGame()
	{
		// TODO: Show winner and game stats, with the option to replay.
	}
	
	
	/// <summary>
	/// Begins the turn.
	/// </summary>
	
	public void BeginTurn()
	{
		GetCurrentPlayer().GetComponent<Launcher>().shotsRemaining = shotsPerTurn;
		FocusOnCurrentPlayer();
	}
	
	
	/// <summary>
	/// Ends the turn.
	/// </summary>
	
	public void EndTurn()
	{
		turnsTaken++;
		if( rules != null )
		{
			if( rules.IsGameOver() )
			{
				EndGame();
				return;
			}
		}
		
		BeginTurn();
	}
	
	
	/// <summary>
	/// Gets the current player.
	/// </summary>
	
	public Player GetCurrentPlayer()
	{
		return players[turnsTaken % 4];
	}
	
	
	/// <summary>
	/// Focuses the main camera on the current player.
	/// </summary>
	
	public void FocusOnCurrentPlayer()
	{
		// Animate the camera to the player's x,y
		/*Vector3 playerPos = GetCurrentPlayer().transform.position;
		Vector3 camPos = mainCamera.transform.position;
		SpringPosition.Begin(mainCamera.gameObject, new Vector3(playerPos.x, camPos.y, camPos.z), 3f).worldSpace = true;
		TweenFOV.Begin(mainCamera.gameObject, 1.5f, 40f);*/
	}
	
	
#endregion
#region 	CALLBACKS
	
	
	/// <summary>
	/// Callback for a newly fired projectile.
	/// </summary>
	
	void projectileFired(Launcher launcher, Projectile projectile)
	{
		// Log every projectile fired, so that we can be sure when they've all exploded.
		projectile.didExplode += projectileExploded;
		projectilesFired.Add(projectile.name, projectile);
	}
	
	
	/// <summary>
	/// Callback for a newly exploded projectile.
	/// </summary>
	
	void projectileExploded(Projectile projectile)
	{
		// TODO: Make sure that the turn doesn't end until all projectiles have exploded.
	}
	
	
	/// <summary>
	/// Callback for player death.
	/// </summary>
	
	void playerDidDie(Player player)
	{	
		// Award a point to the current player.
		GetCurrentPlayer().points++;
		
		
	}
	
	
#endregion
}
