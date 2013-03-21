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
	public ArrayList players;
	
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
		players = new ArrayList();
		Player[] playersInScene = GameObject.FindObjectsOfType(typeof(Player)) as Player[];
		for( int i = 0; i < playersInScene.Length; i++ )
		{
			Player player = playersInScene[i];
			player.playerIndex = i;
			players.Add(player);
		}
		
		// Look for game rules.
		//if( rules != null )
		//	Destroy(rules.gameObject);
		
		rules = GameObject.FindObjectOfType(typeof(Rules)) as Rules;
		if( rules == null )
		{
			Debug.LogWarning("No rules found! Creating defaults...");
			GameObject rulesObject = new GameObject("DefaultRules");
			rules = rulesObject.AddComponent<Rules>();
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

