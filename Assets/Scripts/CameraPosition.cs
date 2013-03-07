using UnityEngine;
using System.Collections;

public class CameraPosition : StaticInstance<CameraPosition>
{
	public readonly float defaultSpeed = 2f;
	public readonly float defaultHeight = 5f;
	
	GameObject[] focusObjects;
	public float speed = 2f;
	public float height = 5f;
	
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	
	void Update()
	{
		if( focusObjects == null || Camera.main == null )
			return;
		
		// Strip out any dead objects, and automatically stop if none are alive.
		ArrayList livingObjects = new ArrayList();
		foreach( GameObject go in focusObjects )
		{
			if( go != null )
				livingObjects.Add(go);
		}
		
		focusObjects = livingObjects.ToArray(typeof(GameObject)) as GameObject[];
		
		if( focusObjects.Length == 0 )
		{
			Stop();
			return;
		}			
		
		int blah = 0;
		if( Input.GetKeyDown(KeyCode.Space) )
			blah = 1;
		
		// Find the points farthest up/right and down/left in screen space.
		Bounds focusBoundsInWorld;
		if( focusObjects[0].renderer != null )
			focusBoundsInWorld = new Bounds(focusObjects[0].renderer.bounds.center, focusObjects[0].renderer.bounds.size);
		else
			focusBoundsInWorld = new Bounds(focusObjects[0].transform.position, Vector3.one);
		
		for( int i = 1; i < focusObjects.Length; i++ )
		{
			Bounds objectBounds;
			if( focusObjects[i].renderer != null )
				objectBounds = new Bounds(focusObjects[i].renderer.bounds.center, focusObjects[i].renderer.bounds.size);
			else
				objectBounds = new Bounds(focusObjects[i].transform.position, Vector3.one);
			
			focusBoundsInWorld.Encapsulate(objectBounds);
		}
		
		// Pan the camera so that it's in the center of the focus bounds.
		Vector3 targetPosition = new Vector3(focusBoundsInWorld.center.x, focusBoundsInWorld.center.y+height, Camera.main.transform.position.z);
		Vector3 positionDiff = targetPosition - Camera.main.transform.position;
		Camera.main.transform.position += positionDiff * Time.deltaTime * speed;
		
		/*Vector3 focusCenterInScreen = Camera.main.WorldToScreenPoint(focusBoundsInWorld.center);
		Vector3 focusMaxInScreen = Camera.main.WorldToScreenPoint(new Vector3(focusBoundsInWorld.center.x + focusBoundsInWorld.size.x/2,
																	 		     focusBoundsInWorld.center.y + focusBoundsInWorld.size.y/2,
																			     0));
		Vector3 focusMinInScreen = Camera.main.WorldToScreenPoint(new Vector3(focusBoundsInWorld.center.x - focusBoundsInWorld.size.x/2,
																	 		     focusBoundsInWorld.center.y - focusBoundsInWorld.size.y/2,
																			     0));
		Vector3 focusSizeInScreen = focusMaxInScreen - focusMinInScreen;
		
		Bounds focusBoundsInScreen = new Bounds(focusCenterInScreen, focusSizeInScreen);*/
		
		// Define our virtual camera edge a little smaller than the real
		// one to keep everything well within bounds.
		/*Vector3 cameraMax = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width-50, Screen.height-50, 0));
		
		// Find the difference between the virtual camera edges and
		// the focus bounds, which will serve as our weight to zoom by.
		Vector3 diff = focusBoundsInWorld.max - cameraMax;
		float zoomWeight = diff.magnitude;
		
		// Adjust the FOV to try bringing all of the points into frame.
		Camera.main.fov += zoomWeight * 0.1f;*/
	}
	
	
	/*public void FocusOn(Vector3 point, float focusSpeed, float focusHeight)
	{
		GameObject oldFocusPoint = GameObject.Find("CameraPosition->FocusPoint");
		if( oldFocusPoint )
			Destroy(oldFocusPoint);
		
		GameObject focusObject = new GameObject("CameraPosition->FocusPoint");
		focusObject.transform.position = point;
		
		FocusOn(focusObject, focusSpeed, focusHeight);
	}
	
	
	public void FocusOn(Vector3 point, float focusSpeed)
	{
		FocusOn(point, focusSpeed, defaultHeight);
	}
	
	
	public void FocusOn(Vector3 point)
	{
		FocusOn(point, defaultSpeed, defaultHeight);
	}*/
	
	
	public void FocusOn(Component[] components, float focusSpeed, float focusHeight)
	{
		ArrayList gameObjects = new ArrayList();
		foreach( Component com in components )
		{
			if( !gameObjects.Contains(com.gameObject) )
				gameObjects.Add(com.gameObject);
		}
		
		FocusOn(gameObjects, focusSpeed, focusHeight);
	}
	
	
	public void FocusOn(Component[] components, float focusSpeed)
	{
		FocusOn(components, focusSpeed, defaultHeight);
	}
	
	
	public void FocusOn(Component[] components)
	{
		FocusOn(components, defaultSpeed, defaultHeight);
	}
	
	
	/// <summary>
	/// Focuses on the game objects.
	/// </summary>
	
	public void FocusOn(GameObject[] gameObjects, float focusSpeed, float focusHeight)
	{		
		focusObjects = gameObjects;
		speed = focusSpeed;
		height = focusHeight;
	}
	
	
	public void FocusOn(GameObject[] gameObjects, float focusSpeed)
	{
		FocusOn(gameObjects, focusSpeed, defaultHeight);
	}
	
	
	public void FocusOn(GameObject[] gameObjects)
	{
		FocusOn(gameObjects, defaultSpeed, defaultHeight);
	}
	
	
	/// <summary>
	/// Focuses on the game objects.
	/// </summary>
	
	public void FocusOn(ArrayList gameObjects, float focusSpeed, float focusHeight)
	{
		FocusOn(gameObjects.ToArray(typeof(GameObject)) as GameObject[], focusSpeed, focusHeight);
	}
	
	
	public void FocusOn(ArrayList gameObjects, float focusSpeed)
	{
		FocusOn(gameObjects.ToArray(typeof(GameObject)) as GameObject[], focusSpeed, defaultHeight);
	}
	
	
	public void FocusOn(ArrayList gameObjects)
	{
		FocusOn(gameObjects.ToArray(typeof(GameObject)) as GameObject[], defaultSpeed, defaultHeight);
	}
	
	
	/// <summary>
	/// Focuses on the game object.
	/// </summary>
	
	public void FocusOn(GameObject go, float focusSpeed, float focusHeight)
	{
		FocusOn(new GameObject[]{ go }, focusSpeed, focusHeight);
	}
	
	
	public void FocusOn(GameObject go, float focusSpeed)
	{
		FocusOn(new GameObject[]{ go }, focusSpeed, defaultHeight);
	}
	
	
	public void FocusOn(GameObject go)
	{
		FocusOn(new GameObject[]{ go }, defaultSpeed, defaultHeight);
	}
	
	
	public void Stop()
	{
		focusObjects = null;
	}
	
}

