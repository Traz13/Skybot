using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
	public static Game instance;
	
#region 	EVENTS
	
	
	public delegate void TurnWillBegin(Game game);
	public event TurnWillBegin turnWillBegin;
	
	public delegate void TurnWillEnd(Game game);
	public event TurnWillEnd turnWillEnd;
	
	public delegate void GameWillBegin(Game game);
	public event GameWillBegin gameWillBegin;
	
	public delegate void GameWillEnd(Game game);
	public event GameWillEnd gameWillEnd;
	
	
#endregion
#region 	VARIABLES
	
	
	/// <summary>
	/// The maximum number of rounds (0 == infinite).
	/// </summary>
	public readonly int maxRounds = 0;
	
	// Current Round
	protected int currentRound = 0;
	
	// Shots per Turn
	public int shotsPerTurn = 3;
	
	// Current Player
	Player mCurrentPlayer;
	public Player currentPlayer {
		get { return mCurrentPlayer; }
	}
	
	// Maximum Velocities
	public float maxFireVelocity = 35f;
	public float maxMoveVelocity = 15f;
	
	// Players
	[HideInInspector]
	public Player[] players;
	
	// Logs/Counters
	int shotsFired = 0;
	int shotsCompleted = 0;
	
	bool playing = false;
	
#endregion
#region UNITY_HOOKS
	
	
	/// <summary>
	/// Awake this instance.
	/// </summary>
	
	protected virtual void Awake()
	{
		if( instance != null )
			throw new System.Exception("Only one instance of Game allowed!");
		
		instance = this;
		
		// Gather and sort players by index.
		Player[] playerComps = GetComponentsInChildren<Player>(false);
		if( playerComps.Length == 0 )
			throw new System.Exception("Must have at least one Player!");
		
		players = new Player[playerComps.Length];
		foreach( Player player in playerComps )
		{
			players[player.playerIndex] = player;
			player.GetComponent<Launcher>().didFireProjectile += shotFired;
		}
		
		mCurrentPlayer = players[0];
	}
	
	
	/// <summary>
	/// Start this instance.
	/// </summary>
	
	protected virtual void Start()
	{
		// Just automatically begin the game for now.
		BeginGame();
	}
	
	
#endregion
#region 	GAME_RULES

	
	/// <summary>
	/// Initialize game data and begin the first turn.
	/// </summary>
	
	public virtual void BeginGame()		
	{
		if( playing )
		{
			Debug.LogWarning("Calling BeginGame() when it's already started!");
			return;
		}
		
		if( gameWillBegin != null )
			gameWillBegin(this);
		
		BeginTurn();
		
		playing = true;
	}
	
	
	/// <summary>
	/// Ends the game.
	/// </summary>
	
	protected virtual void EndGame() 		
	{
		if( !IsGameOver() )
		{
			Debug.LogWarning("Trying to end game prematurely!");
			return;
		}
		
		if( gameWillEnd != null )
			gameWillEnd(this);
		
		playing = false;
	}
	
	
	/// <summary>
	/// Begins the turn.
	/// </summary>
	
	protected virtual void BeginTurn()		
	{		
		if( turnWillBegin != null )
			turnWillBegin(this);
		
		// Refill their shots.
		currentPlayer.GetComponent<Launcher>().shotsRemaining = shotsPerTurn;
		
		// Disable the current player's slowmo zone, and enable the opponents'.
		currentPlayer.GetComponentInChildren<SlowMoZone>().gameObject.SetActive(false);
		foreach( Player player in players )
		{
			if( player != currentPlayer )
				player.GetComponentInChildren<SlowMoZone>().gameObject.SetActive(true);
		}
		
		// Pan/zoom the camera to the current player.
		FocusOnCurrentPlayer();
	}
	
	
	/// <summary>
	/// Ends the turn and starts the next one.
	/// </summary>
	
	protected virtual void EndTurn()
	{
		if( !IsTurnOver() )
		{
			Debug.LogWarning("Trying to end turn prematurely!");
			return;
		}
		
		if( turnWillEnd != null )
			turnWillEnd(this);
		
		// Advance to the next player, as well as the round if we're on the last player.
		int nextPlayerIndex = currentPlayer.playerIndex + 1;
		if( nextPlayerIndex >= players.Length )
		{
			currentRound++;
			nextPlayerIndex = 0;
		}
		
		if( IsGameOver() )
		{
			EndGame();
			return;
		}
		
		BeginTurn();
	}
	
	
	/// <summary>
	/// Have the conditions been met for the end of a turn?
	/// </summary>
	
	protected virtual bool IsTurnOver()	
	{
		return (shotsCompleted >= shotsPerTurn);
	}
	
	
	/// <summary>
	/// Have the conditions been met for the end of the game?
	/// </summary>
	
	protected virtual bool IsGameOver()	
	{
		return (maxRounds > 0 && currentRound > maxRounds);
	}
	
	
#endregion
#region 	METHODS
	
	
	/// <summary>
	/// Focuses the main camera on the current player.
	/// </summary>
	
	public virtual void FocusOnCurrentPlayer()
	{
		// Animate the camera to the player's x,y
		/*Vector3 playerPos = GetCurrentPlayer().transform.position;
		Vector3 camPos = mainCamera.transform.position;
		SpringPosition.Begin(mainCamera.gameObject, new Vector3(playerPos.x, camPos.y, camPos.z), 3f).worldSpace = true;
		TweenFOV.Begin(mainCamera.gameObject, 1.5f, 40f);*/
	}
	
	
	/// <summary>
	/// Callback for newly fired projectile. 
	/// </summary>
	
	void shotFired(Launcher launcher, Projectile projectile)
	{
		// Log every projectile fired, so that we can be sure when they've all exploded.		
		shotsFired++;
		
		projectile.didExplode += shotCompleted;
	}
	
	
	/// <summary>
	/// Callback for a newly exploded projectile.
	/// </summary>
	
	void shotCompleted(Projectile projectile)
	{
		shotsCompleted++;
	}
	
	
#endregion
}

