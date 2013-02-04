using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
	public static Game instance;
	
#region 	EVENTS
	
	public delegate void WillLoad(Game game);
	public static event WillLoad willLoad;
	
	public delegate void DidLoad(Game game);
	public static event DidLoad didLoad;
	
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
	
	// Maximum Rounds
	public readonly int maxRounds = 0;	// (0 == infinite)
	
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
	
	public Camera mainCamera;
	
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
		
		if( willLoad != null )
			willLoad(this);
		
		// Gather and sort players by index.
		Player[] playerComps = GetComponentsInChildren<Player>(false);
		if( playerComps.Length == 0 )
			throw new System.Exception("Must have at least one Player!");
		
		players = new Player[playerComps.Length];
		foreach( Player player in playerComps )
		{
			players[player.playerIndex] = player;
			Launcher launcher = player.GetComponent<Launcher>();
			if( launcher == null )
				Debug.Log(player.name + " doesn't have a Launcher component!");
			else
				launcher.didFireProjectile += shotFired;
		}
		
		mCurrentPlayer = players[0];
		
		// Setup UI callbacks.
		UI.instance.beginGameOverlay.didShow += delegate(UIMenu menu)
		{
			StartCoroutine(beginGameOverlayDidShow(menu));
		};
		
		if( didLoad != null )
			didLoad(this);
	}
	

	IEnumerator beginGameOverlayDidShow(UIMenu menu)
	{
		// Let the menu stay onscreen for a moment, then hide it.
		yield return new WaitForSeconds(2f);
		
		menu.Hide();
	}
	
	
	/// <summary>
	/// Start this instance.
	/// </summary>
	
	protected virtual void Start()
	{
		// Just automatically begin the game for now.
		//StartCoroutine(BeginGame());
	}
	
	
#endregion
#region 	METHODS

	
	/// <summary>
	/// Initialize game data and begin the first turn.
	/// </summary>
	
	public virtual void BeginGame()		
	{		
		StartCoroutine(beginGame());
	}
	
	IEnumerator beginGame()
	{
		if( gameWillBegin != null )
			gameWillBegin(this);
		
		yield return new WaitForSeconds(1f);
		
		// Show beginning of game UI overlay.
		UI.instance.beginGameOverlay.Show();		
		
		yield return new WaitForSeconds(0.2f);
		
		BeginTurn();
	}
	
	
	/// <summary>
	/// End the game.
	/// </summary>
	
	protected virtual void EndGame() 		
	{		
		if( gameWillEnd != null )
			gameWillEnd(this);
	}
	
	
	/// <summary>
	/// Begin the next turn.
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
	}
	
	
	/// <summary>
	/// End the turn and start the next one.
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
		
		// Check if the GameOver conditions have been met, and
		// end the game automatically if they have.
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
		return true;//(shotsCompleted >= shotsPerTurn);
	}
	
	
	/// <summary>
	/// Have the conditions been met for the end of the game?
	/// </summary>
	
	protected virtual bool IsGameOver()	
	{
		return (maxRounds > 0 && currentRound > maxRounds);
	}
	
	
	/// <summary>
	/// Gets the winners (players with highest score, by default).
	/// </summary>
	
	public virtual Hashtable GetWinners()
	{
		Hashtable winners = new Hashtable();
		
		int highscore = 0;
		foreach( Player player in players )
		{
			if( player.score > highscore )
			{
				winners.Clear();
				winners.Add(player.name, player);
				highscore = player.score;
			}
			else if( player.score == highscore )
				winners.Add(player.name, player);
		}
		
		return winners;
	}
	
	
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
	
	
	void playerWillDie(Damageable damageable)
	{
		Player livingPlayer = null;
		foreach( Player player in players )
		{
			Damageable dmgable = player.GetComponent<Damageable>();
			if( dmgable == null || dmgable.health > 0f )
			{
				// If we have more than one living player, don't do anything.
				if( livingPlayer != null )
					return;
				
				livingPlayer = player;
			}
		}
		
		EndGame();
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

