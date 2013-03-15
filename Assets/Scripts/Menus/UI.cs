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
	
#endregion
	
	void Awake()
	{
		if( Instance != null )
			throw new System.Exception("There can only be one instance of UI");
		
		Instance = this;
	}
}

