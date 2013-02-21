using UnityEngine;
using System.Collections;

/// <summary>
/// Attach this to a camera to let the user drag in the X and Y anytime
/// they're not dragging from their avatar started aiming.
/// 
/// TODO: Add momentum, and snap-back at the edges.
/// </summary>

public class DragCamera : MonoBehaviour
{
	public Vector2 speed = new Vector2(4f, 4f);
	
	void Update()
	{
		if( Game.Instance != null )
		{
			// Don't drag the camera if we're aiming.
			Rules rules = Game.Instance.rules;
			if( rules != null && rules.currentPlayer != null && rules.currentPlayer.aiming )
				return;
		}
		
		if( Input.GetMouseButton(0) )
		{
			Vector3 camPos = transform.localPosition;
			camPos.x -= Input.GetAxis("Mouse X") * speed.x;
			camPos.y -= Input.GetAxis("Mouse Y") * speed.y;
			transform.localPosition = camPos;
		}
	}
}

