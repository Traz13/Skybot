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
	
	// Action Points
	public int actionsPerTurn = 3;
	public int actionsRemaining = 0;
	public int actionsCompleted = 0;
	
#endregion
#region UNITY_HOOKS
	
	
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
			else if( IsTurnOver() )
			{
				EndTurn();
				BeginTurn();
			}
		}
	}
	
	
#endregion
#region 	METHODS
	
	
	/// <summary>
	/// Begins the game.
	/// </summary>
	
	public override void BeginGame()
	{
		base.BeginGame();
		
		BeginTurn();
	}
	

	/// <summary>
	/// Begin the next turn.
	/// </summary>
	
	protected virtual void BeginTurn()		
	{		
		Debug.Log("Begin Turn");
		
		if( willBeginTurn != null )
			willBeginTurn(this);
		
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
				{ "oncomplete", "focusOnPlayer" },
				{ "oncompletetarget", gameObject }
			});
		}
		
		// Reset action points, and listen for actions.
		actionsRemaining = actionsPerTurn;
		actionsCompleted = 0;
		
		Launcher launcher = currentPlayer.GetComponentInChildren<Launcher>();
		if( launcher != null )
			launcher.didFireProjectile += shotFired;
	}
	
	
	void focusOnPlayer()
	{
		CameraPosition.Instance.FocusOn(currentPlayer.gameObject, 2f);
		CameraFov.Instance.AdjustTo(35f);
	}
	
	
	/// <summary>
	/// End the turn and start the next one.
	/// </summary>
	
	protected virtual void EndTurn()
	{
		Debug.Log("End Turn");
		
		if( !IsTurnOver() )
		{
			Debug.LogWarning("Trying to end turn prematurely!");
			return;
		}
		
		if( willEndTurn != null )
			willEndTurn(this);
		
		// Stop listening for actions from the last player.
		Launcher launcher = currentPlayer.GetComponentInChildren<Launcher>();
		if( launcher != null )
			launcher.didFireProjectile -= shotFired;
		
		// Advance to the next player, as well as the round if we're on the last player.		
		int nextPlayerIndex = currentPlayer.playerIndex + 1;
		if( nextPlayerIndex >= Game.Instance.players.Count )
		{
			currentRound++;
			nextPlayerIndex = 0;
		}
		
		currentPlayer = Game.Instance.players[nextPlayerIndex] as Player;
	}
	
	
	/// <summary>
	/// Returns true if the end-of-turn conditions have been met.
	/// </summary>
	
	protected virtual bool IsTurnOver()	
	{
		// End the turn when the current player has finished all their actions.
		return( actionsCompleted >= actionsPerTurn );
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
		// Only allow the current player to perform actions (if we have action points).
		return( go == currentPlayer.gameObject && actionsRemaining > 0 );
	}
	
	
	/// <summary>
	/// Callback for newly fired projectile. 
	/// </summary>
	
	void shotFired(Launcher launcher, Projectile projectile)
	{
		// Log every action
		actionsRemaining--;
		
		if( actionsRemaining < 0 )
			Debug.LogWarning("Not enough action points left!");
		
		if( projectile.exploder != null )
			projectile.exploder.didExplode += projectileDidExplode;
	}
	
	
	/// <summary>
	/// Callback for a newly exploded projectile.
	/// </summary>
	
	void projectileDidExplode(Exploder exploder)
	{		
		actionsCompleted++;
		
		// Make sure we only get notified once for each projectile.
		exploder.didExplode -= projectileDidExplode;
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

