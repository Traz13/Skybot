using UnityEngine;
using System.Collections;

public class DeathmatchRules : GameRules
{
	public override bool IsGameOver()
	{
		Game game = Game.instance;
		
		int livingPlayers = 0;
		foreach( Player player in game.players )
		{
			if( player.gameObject.activeSelf )
				livingPlayers++;
		}
		
		return (livingPlayers <= 1);
	}
}

