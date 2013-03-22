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
	}
	
	
	/// <summary>
	/// Follow the specified components' game objects.
	/// </summary>
	
	public void Follow(Component[] components, float focusSpeed, float focusHeight)
	{
		ArrayList gameObjects = new ArrayList();
		foreach( Component com in components )
		{
			if( !gameObjects.Contains(com.gameObject) )
				gameObjects.Add(com.gameObject);
		}
		
		Follow(gameObjects, focusSpeed, focusHeight);
	}
	
	
	public void Follow(Component[] components, float focusSpeed)
	{
		Follow(components, focusSpeed, defaultHeight);
	}
	
	
	public void Follow(Component[] components)
	{
		Follow(components, defaultSpeed, defaultHeight);
	}
	
	
	/// <summary>
	/// Focuses on the game objects.
	/// </summary>
	
	public void Follow(GameObject[] gameObjects, float focusSpeed, float focusHeight)
	{		
		focusObjects = gameObjects;
		speed = focusSpeed;
		height = focusHeight;
	}
	
	
	public void Follow(GameObject[] gameObjects, float focusSpeed)
	{
		Follow(gameObjects, focusSpeed, defaultHeight);
	}
	
	
	public void Follow(GameObject[] gameObjects)
	{
		Follow(gameObjects, defaultSpeed, defaultHeight);
	}
	
	
	/// <summary>
	/// Focuses on the game objects.
	/// </summary>
	
	public void Follow(ArrayList gameObjects, float focusSpeed, float focusHeight)
	{
		Follow(gameObjects.ToArray(typeof(GameObject)) as GameObject[], focusSpeed, focusHeight);
	}
	
	
	public void Follow(ArrayList gameObjects, float focusSpeed)
	{
		Follow(gameObjects.ToArray(typeof(GameObject)) as GameObject[], focusSpeed, defaultHeight);
	}
	
	
	public void Follow(ArrayList gameObjects)
	{
		Follow(gameObjects.ToArray(typeof(GameObject)) as GameObject[], defaultSpeed, defaultHeight);
	}
	
	
	/// <summary>
	/// Focuses on the game object.
	/// </summary>
	
	public void Follow(GameObject go, float focusSpeed, float focusHeight)
	{
		Follow(new GameObject[]{ go }, focusSpeed, focusHeight);
	}
	
	
	public void Follow(GameObject go, float focusSpeed)
	{
		Follow(new GameObject[]{ go }, focusSpeed, defaultHeight);
	}
	
	
	public void Follow(GameObject go)
	{
		Follow(new GameObject[]{ go }, defaultSpeed, defaultHeight);
	}
	
	
	public void Stop()
	{
		focusObjects = null;
	}
	
}

