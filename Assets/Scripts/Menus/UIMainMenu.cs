using UnityEngine;
using System.Collections;

public class UIMainMenu : UIMenu
{
#region 	VARIABLES
	
	public UILabel titleLabel;
	public UIJglButton startButton;
	public string levelName = "Sandbox (lars)";
	
	float titleColorTimer = 0f;
	int titleColorIndex = 0;
	Color[] titleColors = new Color[] { new Color(1f, 0f, 0f), new Color(1f, 1f, 0f), new Color(0f, 1f, 0f),
									    new Color(0f, 1f, 1f), new Color(0f, 0f, 1f), new Color(1f, 0f, 1f) };
	
#endregion
#region 	UNITY_HOOKS
	
	
	/// <summary>
	/// Awake this instance.
	/// </summary>
	
	void Awake()
	{
		startButton.didPress += startButtonDidPress;
	}
	
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	
	protected override void Update()
	{		
		// Shift the color of the title gradually over time through our palette.
		int nextIndex = titleColorIndex + 1;
		if( nextIndex >= titleColors.Length )
			nextIndex = 0;
		
		titleLabel.color = Color.Lerp(titleColors[titleColorIndex], titleColors[nextIndex], titleColorTimer);
		
		titleColorTimer += Time.deltaTime * 0.1f;
		if( titleColorTimer >= 1f )
		{
			titleColorTimer = 0f;
			titleColorIndex++;
			if( titleColorIndex >= titleColors.Length )
				titleColorIndex = 0;
		}
	}
	
	
	void OnLevelWasLoaded()
	{
		moveToGameAndBegin();
		
		foreach( Camera cam in GameObject.FindObjectsOfType(typeof(Camera)))
		{
			if( cam.gameObject != Camera.main.gameObject )
			{				
				AudioListener audioListener = cam.GetComponent<AudioListener>();
				if( audioListener )
					Destroy(audioListener);
				
				// Disable any non-UI camera.
				UICamera uiCam = cam.GetComponent<UICamera>();
				if( uiCam == null )
					cam.gameObject.SetActive(false);
			}
		}
		
	}
	
	
#endregion
#region 	METHODS
	
	
	/// <summary>
	/// Callback for start button didPress event.
	/// </summary>
	
	void startButtonDidPress(UIJglButton button)
	{
		if( button.isDown )
			return;
		
		Application.LoadLevel(levelName);
	}
	
	
	/// <summary>
	/// Animate the camera away from the menus into the game area, and begin the game.
	/// </summary>
	
	void moveToGameAndBegin()
	{
		// Pan the camera back from the menu and down toward the playing area.
		Vector3[] path = new Vector3[3];
		path[0] = Camera.main.transform.position;
		path[1] = new Vector3(path[0].x, path[0].y-10, Game.Instance.CameraTransform.position.z);
		path[2] = Game.Instance.CameraTransform.position;
		
		iTween.MoveTo(Camera.main.gameObject, iTween.Hash(
			"path", path,
			"easetype", iTween.EaseType.easeInOutSine,
			"time", 2.5f,
			"oncomplete", "cameraMovedToGame",
			"oncompletetarget", gameObject
		));
		
		iTween.RotateTo(Camera.main.gameObject, iTween.Hash(
			"rotation", Game.Instance.CameraTransform.eulerAngles,
			"easetype", iTween.EaseType.easeInOutSine,
			"time", 2.5f
		));
	}
	
	
	void cameraMovedToGame()
	{
		// Hide the main menu when we're finished animating.
		gameObject.SetActive(false);
		
		Game.Instance.rules.BeginGame();
	}
	
	
#endregion
}

