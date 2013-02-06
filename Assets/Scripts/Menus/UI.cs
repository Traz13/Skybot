using UnityEngine;
using System.Collections;

public class UI : MonoBehaviour
{
	// Singleton
	public static UI instance;
	
#region 	VARIABLES
	
	public UIMainMenu mainMenu;
	public UIBeginGame beginGameOverlay;
	public UIEndGame endGameOverlay;
	
#endregion
#region UNITY_HOOKS
	
	
	/// <summary>
	/// Awake this instance.
	/// </summary>
	
	void Awake()
	{
		if( instance != null )
			throw new System.Exception("Only one instance of UI is allowed!");
		
		instance = this;
	}
	
	
#endregion
}

