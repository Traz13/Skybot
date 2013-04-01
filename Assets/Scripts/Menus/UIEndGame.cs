using UnityEngine;
using System.Collections;

public class UIEndGame : UIMenu
{
	public UILabel winnerLabel;
	public UIJglButton mainMenuButton;
	public UIJglButton rematchButton;
	public GameObject background;
	
	float winnerColorTimer = 0f;
	int winnerColorIndex = 0;
	Color[] winnerColors = new Color[] { new Color(1f, 0f, 0f), new Color(1f, 1f, 0f), new Color(0f, 1f, 0f),
									     new Color(0f, 1f, 1f), new Color(0f, 0f, 1f), new Color(1f, 0f, 1f) };

	
	/// <summary>
	/// Awake this instance.
	/// </summary>
	
	void Awake()
	{
		mainMenuButton.didPress += mainMenuButtonDidPress;
		rematchButton.didPress += rematchButtonDidPress;
	}
	
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	
	protected override void Update()
	{
		base.Update();
		
		// Shift the color of the winner label gradually over time through our palette.
		int nextIndex = winnerColorIndex + 1;
		if( nextIndex >= winnerColors.Length )
			nextIndex = 0;
		
		winnerLabel.color = Color.Lerp(winnerColors[winnerColorIndex], winnerColors[nextIndex], winnerColorTimer);
		
		winnerColorTimer += Time.deltaTime * 10;
		if( winnerColorTimer >= 1f )
		{
			winnerColorTimer = 0f;
			winnerColorIndex++;
			if( winnerColorIndex >= winnerColors.Length )
				winnerColorIndex = 0;
		}
	}
	
	
	/// <summary>
	/// Callback for main menu button didPress
	/// </summary>
	
	void mainMenuButtonDidPress(UIJglButton button)
	{
		if( button.isDown )
			return;
		
		UI.Instance.mainMenu.gameObject.SetActive(true);
		
		Transform mainMenuCameraTransform = UI.Instance.mainMenu.cameraTransform;
		
		// Pan the camera away from the game area, and back up to the main menu.
		Vector3[] path = new Vector3[3];
		path[0] = Camera.main.transform.position;
		path[1] = new Vector3(path[0].x, path[0].y, Camera.main.transform.position.z - 5);
		path[2] = mainMenuCameraTransform.position;
		
		iTween.MoveTo(Camera.main.gameObject, iTween.Hash(
			"path", path,
			"easetype", iTween.EaseType.easeInOutSine,
			"time", 2.5f,
			"oncomplete", "cameraMovedToMainMenu",
			"oncompletetarget", gameObject
		));
		
		iTween.RotateTo(Camera.main.gameObject, iTween.Hash(
			"rotation", mainMenuCameraTransform.eulerAngles,
			"easetype", iTween.EaseType.easeInOutSine,
			"time", 2.5f
		));
		
		//CameraPosition.Instance.Stop();
		Messenger.ReportCameraStopEvent();
		CameraFov.Instance.AdjustTo(60f);
		
		Hide();
	}
	
	
	void cameraMovedToMainMenu()
	{
		
	}
	
	
	void rematchButtonDidPress(UIJglButton button)
	{
		if( button.isDown )
			return;
		
		// Reload game.
		Application.LoadLevel(Application.loadedLevelName);
		
		Hide();
	}
}

