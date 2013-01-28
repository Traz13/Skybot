using UnityEngine;
using System.Collections;

public class Deathmatch : Game
{	
	int killLimit = 5;
	
	/// <summary>
	/// Awake this instance.
	/// </summary>
	
	protected override void Awake()
	{
		base.Awake();
		
		// Setup player callbacks.
		foreach( Player player in players )
			player.didDie += playerDidDie;
	}
	
	
	/// <summary>
	/// Callback for player death.
	/// </summary>
	
	void playerDidDie(Player player)
	{			
		// Award a point if the dying player isn't the current one.
		if( player != currentPlayer )
			currentPlayer.score++;
	}
	
	
	/// <summary>
	/// Determines whether this instance is game over.
	/// </summary>
	
	protected override bool IsGameOver()
	{
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
}

