using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : StaticInstance<Game>
{
#region EVENTS
	
	public delegate void WillLoad(Game game);
	public static event WillLoad willLoad;
	
	public delegate void DidLoad(Game game);
	public static event DidLoad didLoad;
	
#endregion
#region 	VARIABLES
	
	// Players
	public Player[] players;
	
	// Rules
	public Rules rules;
	
	// Building Creator - need a reference to the prefab
	public BuildingFactory buildingFactory = null;
	
#endregion
#region UNITY_HOOKS
	
	
	/// <summary>
	/// Enable this instance.
	/// </summary>
	
	void OnEnable()
	{
		LoadGame();
		
		// If we have no menus, just automatically begin the game.
		if( UI.Instance == null || UI.Instance.mainMenu == null )
			rules.BeginGame();
	}
	
	
	void OnLevelWasLoaded()
	{
		LoadGame();
	}
	
	
	public void LoadGame()
	{
		// Send event.
		if( willLoad != null )
			willLoad(this);
		
		// Gather all the players.
		Player[] playersInScene = GameObject.FindObjectsOfType(typeof(Player)) as Player[];
		players = new Player[playersInScene.Length];
		for( int i = 0; i < playersInScene.Length; i++ )
		{
			Player player = playersInScene[i];
			if( players[player.index] != null )
			{
				Debug.LogError(player.name + " index (" + player.index + ") is the same as " + players[player.index].name + "!");
				
				for( int j = i; j < playersInScene.Length; j++ )
				{
					if( players[j] == null )
					{
						player.index = j;
						break;
					}
				}
			}
			
			players[player.index] = player;
		}
		
		// Get our rules, or create defaults if we find none.
		//rules = GameObject.FindObjectOfType(typeof(Rules)) as Rules;
		if( rules == null )
		{
			Debug.LogWarning("No rules found! Creating defaults...");
			
			GameObject rulesObject = new GameObject("DefaultRules");
			rules = rulesObject.AddComponent<Rules>();
			
			BoxCollider rulesCollider = rules.gameObject.AddComponent<BoxCollider>();
			rulesCollider.center = new Vector3(0, 0, 0);
			rulesCollider.size = new Vector3(500, 250, 500);
			rulesCollider.isTrigger = true;
			
			foreach( Player player in players )
				player.shotsRemaining = 9999;
		}
		else
		{
			rules = Instantiate(rules) as Rules;
		}
		
		// Send event.
		if( didLoad != null )
			didLoad(this);
		
		Debug.Log("Game Loaded");
	}
	
	void Reset() {
		if(buildingFactory == null)
			buildingFactory = (Instantiate(Resources.Load("Prefabs/BuildingFactory")) as GameObject).GetComponent<BuildingFactory>();
	}
	
	
#endregion
}

