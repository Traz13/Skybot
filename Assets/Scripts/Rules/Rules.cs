using UnityEngine;
using System.Collections;

public class Rules : MonoBehaviour
{
#region 	EVENTS
	
	public delegate void WillBeginGame(Rules rules);
	public event WillBeginGame willBeginGame;
	
	public delegate void WillEndGame(Rules rules);
	public event WillEndGame willEndGame;
	
#endregion
#region 	VARIABLES
	
	// Playing
	public bool playing = false;
	
	// Current Player
	Player mCurrentPlayer;
	public Player currentPlayer {
		get { return mCurrentPlayer; }
		set { mCurrentPlayer = value; }
	}
	
	// Maximum Velocities
	public float maxFireVelocity = 35f;
	public float maxMoveVelocity = 15f;
	
	// Camera Transform
	public Transform cameraTransform;
	
#endregion
#region METHODS
	
	
	/// <summary>
	/// Initialize game data and begin the first turn.
	/// </summary>
	
	public virtual void BeginGame()		
	{		
		Debug.Log("Begin Game!");
		
		if( Game.Instance.players.Count == 0 )
			throw new System.Exception("Must have at least one Player!");
		
		if( willBeginGame != null )
			willBeginGame(this);
		
		// Show the "begin game" overlay.
		if( UI.Instance != null )
			UI.Instance.beginGameOverlay.Show();
		
		// Set the first player in the array as the starting player.
		currentPlayer = Game.Instance.players[0] as Player;
		
		playing = true;
	}
	
	
	/// <summary>
	/// End the game.
	/// </summary>
	
	protected virtual void EndGame() 		
	{
		if( SlowMo.Instance.on )
			SlowMo.Instance.didStop += slowMoDidStop;
		else
			StartCoroutine(endGame());
		
		playing = false;
	}
	
	
	void slowMoDidStop(SlowMo slowmo)
	{
		SlowMo.Instance.didStop -= slowMoDidStop;
		StartCoroutine(endGame());
	}
	
	
	IEnumerator endGame()
	{
		Debug.Log("Game Over!");
		
		if( willEndGame != null )
			willEndGame(this);
		
		yield return new WaitForSeconds(0.5f);
		
		Hashtable winners = GetWinners();
		if( winners.Count == 0 )
			throw new System.Exception("No winners found in EndGame()!");
		
		string message = "";
		foreach( Player player in winners.Values )
			message += (message.Length == 0) ? player.name : " & "+player.name;
		
		message += (winners.Count > 1) ? " Win!" : " Wins!";
		
		// Show "end game" overlay.
		if( UI.Instance != null )
		{
			UI.Instance.endGameOverlay.winnerLabel.text = message;
			UI.Instance.endGameOverlay.Show();
		}
		
		playing = false;
	}
	
	
	/// <summary>
	/// Have the conditions been met for the end of the game?
	/// </summary>
	
	protected virtual bool IsGameOver()	
	{
		return false;
	}
	
	
	/// <summary>
	/// Gets the winners (players with highest score, by default).
	/// </summary>
	
	public virtual Hashtable GetWinners()
	{
		Hashtable winners = new Hashtable();
		
		int highscore = 0;
		foreach( Player player in Game.Instance.players )
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
	
	
	/// <summary>
	/// Returns true if user action is allowed on the specified object.
	/// </summary>
	
	public virtual bool AllowUserAction(GameObject go)
	{
		return true;
	}
	
	
	/// <summary>
	/// Callback for collision trigger exit.
	/// </summary>
	
	void OnTriggerExit(Collider collider)
	{
		// Explode the projectile automatically if it exits the game world.
		Projectile projectile = collider.GetComponent<Projectile>();
		if( projectile != null )
		{
			Exploder exploder = projectile.GetComponent<Exploder>();
			if( exploder != null )
				exploder.Explode();
		}
	}
	
	
#endregion
}

