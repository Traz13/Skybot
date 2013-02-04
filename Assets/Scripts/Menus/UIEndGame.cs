using UnityEngine;
using System.Collections;

public class UIEndGame : UIMenu
{
	public UILabel winnerLabel;
	public GameObject background;
	
	float winnerColorTimer = 0f;
	int winnerColorIndex = 0;
	Color[] winnerColors = new Color[] { new Color(1f, 0f, 0f), new Color(1f, 1f, 0f), new Color(0f, 1f, 0f),
									     new Color(0f, 1f, 1f), new Color(0f, 0f, 1f), new Color(1f, 0f, 1f) };
	
	
	void Update()
	{
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
	/// Animate the menu onscreen.
	/// </summary>
	
	//public override void Show(bool animated)
	//{
	//	
	//}
	
	
	/// <summary>
	/// Animate the menu offscreen.
	/// </summary>
	
	//public override void Hide(bool animated)
	//{
	//	
	//}
}

