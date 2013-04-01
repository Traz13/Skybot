using UnityEngine;
using System.Collections;

public class TurnBasedRules : Rules
{
#region EVENTS

	public delegate void WillBeginTurn(Rules rules);
	public event WillBeginTurn willBeginTurn;
	
	public delegate void WillEndTurn(Rules rules);
	public event WillEndTurn willEndTurn;
	
#endregion
#region VARIABLES	
		
	// Maximum Rounds (0 == infinite)
	public readonly int maxRounds = 0;
	
	// Current Round
	protected int currentRound = 0;
	public int shotsCompleted = 0;
	
	public int shotsPerTurn = 1;
	
	public bool endTurnNow = false;
	bool endingTurn = false;
	
#endregion
#region UNITY_HOOKS
	
	
	/// <summary>
	/// Awake this instance.
	/// </summary>
	
	void Awake()
	{
		UI ui = UI.Instance;
		if( ui != null )
			ui.endTurnButton.didPress += endTurnPressed;
		else
		{
			UI.didLoad += delegate(UI uib) {
				uib.endTurnButton.didPress += endTurnPressed;
			};
		}
	}
	
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	
	void Update ()
	{
		if( playing )
		{
			// Check if we should end the game, or the turn.
			if( IsGameOver() )
				EndGame();
			else if( !endingTurn && IsTurnOver() )
				StartCoroutine(endTurn_delay());
		}
	}
	
	
#endregion
#region 	METHODS
	
	
	void endTurnPressed(UIJglButton button)
	{
		if( button.isDown )
			return;
			
		EndTurn();
	}
	
	
	/// <summary>
	/// Begins the game.
	/// </summary>
	
	public override void BeginGame()
	{
		currentRound = 0;
		base.BeginGame();
		
		BeginTurn();
	}
	
	
	protected override void EndGame()
	{
		base.EndGame();
		
		UI ui = UI.Instance;
		if( ui != null )
		{
			ui.phaseLabel.gameObject.SetActive(false);
			ui.endTurnButton.gameObject.SetActive(false);
		}
	}
	

	/// <summary>
	/// Begin the next turn.
	/// </summary>
	
	protected virtual void BeginTurn()		
	{		
		Debug.Log("Begin Turn");
		
		if( willBeginTurn != null )
			willBeginTurn(this);
		
		// Reset action points, and listen for actions.
		currentPlayer.shotsRemaining = shotsPerTurn;
		shotsCompleted = 0;
		
		currentPlayer.RefillFuel();
		
		Launcher launcher = currentPlayer.GetComponentInChildren<Launcher>();
		if( launcher != null )
			launcher.didFireProjectile += shotFired;
		
		focusOnPlayer(currentPlayer);
		
		UI ui = UI.Instance;
		if( ui != null )
		{
			ui.phaseLabel.gameObject.SetActive(true);
			ui.phaseLabel.text = "[FF0000]Attack[-]";
			
			ui.endTurnButton.gameObject.SetActive(true);
		}
	}
	
	
	void focusOnPlayer(Player player)
	{
		//CameraPosition.Instance.Follow(player.gameObject, 0.025f);
		Messenger.ReportCameraFollowEvent(player.gameObject, 0.025f);
		CameraFov.Instance.AdjustTo(60f);
	}
	
	
	IEnumerator endTurn_delay()
	{
		endTurnNow = false;
		endingTurn = true;
		
		yield return new WaitForSeconds(1f);
		
		EndTurn();
	}
	
	
	/// <summary>
	/// End the turn and start the next one.
	/// </summary>
	
	protected virtual void EndTurn()
	{
		Debug.Log("End Turn");
		
		if( willEndTurn != null )
			willEndTurn(this);
		
		// Stop listening for actions from the last player.
		Launcher launcher = currentPlayer.GetComponentInChildren<Launcher>();
		if( launcher != null )
			launcher.didFireProjectile -= shotFired;
		
		// Advance to the next player, as well as the round if we're on the last player.		
		int nextPlayerIndex = currentPlayer.index + 1;
		if( nextPlayerIndex >= Game.Instance.players.Length )
		{
			currentRound++;
			nextPlayerIndex = 0;
			
			// Shift the buildings.
			foreach( Player player in Game.Instance.players )
				player.rigidbody.WakeUp();
			
			foreach( Building building in GameObject.FindObjectsOfType(typeof(Building)) )
			{
				iTween.MoveTo(building.gameObject, new Hashtable() {
					{ "position", new Vector3(building.transform.localPosition.x, Random.Range(building.shiftMin, building.shiftMax), building.transform.localPosition.z) },
					{ "easetype", "easeInOutSine" },
					{ "time", 3f },
					{ "islocal", true },
					//{ "oncomplete", "focusOnPlayer" },
					//{ "oncompletetarget", gameObject }
				});
			}
		}
		
		currentPlayer = Game.Instance.players[nextPlayerIndex] as Player;
		
		BeginTurn();
		
		endingTurn = false;
	}
	
	
	/// <summary>
	/// Returns true if the end-of-turn conditions have been met.
	/// </summary>
	
	protected virtual bool IsTurnOver()	
	{
		if( endTurnNow )
			return true;
		
		// End the turn when the current player has finished all their actions.
		return( shotsCompleted >= shotsPerTurn && currentPlayer.fuel <= 0 );
	}
	
	
	/// <summary>
	/// Returns true if the game-over conditions have been met.
	/// </summary>
	
	protected override bool IsGameOver()	
	{
		// End the game if we've hit the round limit.
		return( maxRounds > 0 && currentRound > maxRounds );
	}
	
	
	/// <summary>
	/// Returns true if user action is allowed on the specified object.
	/// </summary>
	
	public override bool AllowUserAction(GameObject go)
	{
		if( currentPlayer == null )
			return false;
		
		// Only allow the current player to perform actions if we have action points.
		return( go.gameObject == currentPlayer.gameObject );
	}
	
	
	/// <summary>
	/// Callback for newly fired projectile. 
	/// </summary>
	
	void shotFired(Launcher launcher, Projectile projectile)
	{		
		if( projectile.exploder != null )
			projectile.exploder.didExplode += projectileDidExplode;
	}
	
	
	/// <summary>
	/// Callback for a newly exploded projectile.
	/// </summary>
	
	void projectileDidExplode(Exploder exploder)
	{		
		shotsCompleted++;
		
		if( SlowMo.Instance.on )
		{
			SlowMo.Instance.didStop += delegate {
				StartCoroutine(startMovePhase());
			};
		}
		else
			StartCoroutine(startMovePhase());
		
		// Make sure we only get notified once for each projectile.
		exploder.didExplode -= projectileDidExplode;
	}
	
	
	IEnumerator startMovePhase()
	{
		yield return new WaitForSeconds(1f);
		focusOnPlayer(currentPlayer);
		
		if( UI.Instance != null )
			UI.Instance.phaseLabel.text = "[0000FF]Move[-]";
	}
	
	
	/// <summary>
	/// Callback for player death.
	/// </summary>
	
	void playerWillDie(Damageable damageable)
	{			
		// Award a point if the dying player isn't the current one.
		if( damageable.gameObject != currentPlayer.gameObject )
			currentPlayer.score++;
	}
	
	
#endregion
}

