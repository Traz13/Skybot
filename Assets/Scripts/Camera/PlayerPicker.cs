using UnityEngine;
using System.Collections;

public class PlayerPicker : StaticInstance<PlayerPicker>
{
	void Update()
	{
		if( !Input.GetMouseButtonDown(0) )
			return;
		
		Camera cam = Camera.main;
		if( cam == null )
		{
			cam = Game.Instance.GetComponentInChildren<Camera>();
			if( cam == null )
				return; 	// No valid cameras.
		}
		
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		
		if( !Physics.Raycast(ray, out hit) )
			return;
		
		Transform trans = hit.collider.transform;
		Player playerHit = null;
		for( int i = 0; i < 5; i++ )
		{
			if( trans == null )
				break;
			
			playerHit = trans.gameObject.GetComponent<Player>();
			if( playerHit != null )
				break;
			else
				trans = trans.parent;
		}
		
		if( playerHit == null )
			return;
		
		playerHit.SendMessage("MouseDown");
	}
}

