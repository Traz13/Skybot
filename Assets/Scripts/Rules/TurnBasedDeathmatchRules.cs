using UnityEngine;
using System.Collections;

public class TurnBasedDeathmatchRules : TurnBasedRules
{	
	public int killLimit = 1;
	
	
	/// <summary>
	/// Callback for player death.
	/// </summary>
	
	void playerWillDie(Damageable damageable)
	{			
		// Award a point if the dying player isn't the current one.
		if( damageable.gameObject != currentPlayer.gameObject )
			currentPlayer.score++;
		
		if( IsGameOver() )
			EndGame();
	}
	
	
	/// <summary>
	/// Determines whether the GameOver conditions have been met.
	/// </summary>
	
	protected override bool IsGameOver()
	{
		// Base GameOver conditions override child conditions.
		if( base.IsGameOver() )
			return true;
		
		// Check each player to see if anyone has reached the kill limit.
		foreach( Player player in Game.Instance.players )
		{
			if( player.score >= killLimit )
				return true;
		}
		
		return false;
	}
	
	
	/// <summary>
	/// Gets the winning players.
	/// </summary>
	
	public override Hashtable GetWinners()
	{
		Hashtable winners = new Hashtable();
		
		foreach( Player player in Game.Instance.players )
		{
			if( player.score >= killLimit )
				winners.Add(player.name, player);
		}
		
		return winners;
	}
	
	
	/// <summary>
	/// Begins the game.
	/// </summary>
	
	public override void BeginGame()
	{
		base.BeginGame();
		
		// Setup player callbacks.
		foreach( Player player in Game.Instance.players )
		{
			Damageable damageable = player.GetComponent<Damageable>();
			if( damageable == null )
				Debug.Log(player.name + " doesn't have a Damageable component!");
			else
				damageable.willDie += playerWillDie;
		}
	}
}

