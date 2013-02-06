using UnityEngine;
using System.Collections;

public class UIMainMenu : MonoBehaviour
{
	public UILabel titleLabel;
	public UIJglButton startButton;
	
	float titleColorTimer = 0f;
	int titleColorIndex = 0;
	Color[] titleColors = new Color[] { new Color(1f, 0f, 0f), new Color(1f, 1f, 0f), new Color(0f, 1f, 0f),
									    new Color(0f, 1f, 1f), new Color(0f, 0f, 1f), new Color(1f, 0f, 1f) };
	
	void Awake()
	{
		startButton.didPress += startButtonDidPress;
	}
	
	
	void Update()
	{		
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
	
	
	void startButtonDidPress(UIJglButton button)
	{
		if( button.isDown )
			return;
		
		Application.LoadLevel(1);
		
		Game.didLoad += delegate {
			StartCoroutine(moveToGameAndBegin());
		};
		
		//gameObject.SetActive(false);
	}
	
	
	IEnumerator moveToGameAndBegin()
	{
		// Pan the camera down to the playing area.
		Vector3[] path = new Vector3[3];
		path[0] = Camera.main.transform.position;
		path[1] = new Vector3(path[0].x, path[0].y-10, Game.instance.mainCamera.transform.position.z);
		path[2] = Game.instance.mainCamera.transform.position;
		
		iTween.MoveTo(Camera.main.gameObject, iTween.Hash(
			"path", path,
			"easetype", iTween.EaseType.easeInOutSine,
			"time", 2.5f
		));
		
		/*iTween.RotateTo(MainCamera.instance.gameObject, iTween.Hash(
			"rotation", Game.instance.mainCamera.transform.rotation ,
			"easetype", iTween.EaseType.easeInOutSine,
			"time", 2.5f
		));*/
		
		yield return new WaitForSeconds(2.5f);
		
		gameObject.SetActive(false);
		
		Game.instance.BeginGame();
	}
}

