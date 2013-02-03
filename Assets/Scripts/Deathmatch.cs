using UnityEngine;
using System.Collections;

public class Deathmatch : Game
{	
	int killLimit = 1;
	
	/// <summary>
	/// Awake this instance.
	/// </summary>
	
	protected override void Awake()
	{
		base.Awake();
		
		// Setup player callbacks.
		foreach( Player player in players )
		{
			Damageable damageable = player.GetComponent<Damageable>();
			if( damageable == null )
				Debug.Log(player.name + " doesn't have a Damageable component!");
			else
				damageable.willDie += playerWillDie;
		}
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
	
	
	/// <summary>
	/// Determines whether the GameOver conditions have been met.
	/// </summary>
	
	protected override bool IsGameOver()
	{
		// Base GameOver conditions override child conditions.
		if( base.IsGameOver() )
			return true;
		
		// Check each player to see if anyone has reached the kill limit.
		foreach( Player player in Game.instance.players )
		{
			if( player.score >= killLimit )
				return true;
		}
		
		return false;
	}
	
	
	public override Hashtable GetWinners()
	{
		Hashtable winners = new Hashtable();
		
		foreach( Player player in players )
		{
			if( player.score >= killLimit )
				winners.Add(player.name, player);
		}
		
		return winners;
	}
	
	
	protected override void EndGame()
	{
		base.EndGame();
		
		Hashtable winners = GetWinners();
		if( winners.Count == 0 )
			throw new System.Exception("No winners found in EndGame()!");
		
		string message = "";
		foreach( Player player in winners.Values )
			message += (message.Length == 0) ? player.name : " & "+player.name;
		
		message += (winners.Count > 1) ? " Wins!" : " Win!";
		
		// Show end of game UI overlay.
		
	}
}

