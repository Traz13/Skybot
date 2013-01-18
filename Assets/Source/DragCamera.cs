using UnityEngine;
using System.Collections;

public class DragCamera : MonoBehaviour
{
	void Update()
	{
		if( Game.instance.GetCurrentPlayer().aiming )
			return;
		
		if( Input.GetMouseButton(0) )
		{
			Vector3 camPos = transform.localPosition;
			camPos.y -= Input.GetAxis("Mouse Y") * 4f;
			camPos.x -= Input.GetAxis("Mouse X") * 4f;
			transform.localPosition = camPos;
		}
	}
}

