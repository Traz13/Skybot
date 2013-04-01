using UnityEngine;
using System.Collections;

public class UI : MonoBehaviour
{		
	public static UI Instance = null;
	
#region 	VARIABLES
	
	public UIMainMenu mainMenu;
	public UIBeginGame beginGameOverlay;
	public UIEndGame endGameOverlay;
	public UILabel phaseLabel;
	public UIJglButton endTurnButton; 
	
#endregion
	
	public delegate void DidLoad(UI ui);
	public static event DidLoad didLoad;
	
	void Awake()
	{
		if( Instance != null )
			throw new System.Exception("There can only be one instance of UI");
		
		Instance = this;
		
		if( didLoad != null )
			didLoad(this);
	}
}

