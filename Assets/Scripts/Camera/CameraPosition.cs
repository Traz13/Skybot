using UnityEngine;
using System.Collections;

public class CameraPosition : MonoBehaviour//StaticInstance<CameraPosition>
{
	public float defaultSpeed = 1f;
	public float defaultHeight = 0f;
	
	GameObject[] focusObjects;
	float speed = 1f;
	float height = 5f;
	
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	
	void Update()
	{
		if( focusObjects == null || Game.Instance == null )
			return;
		
		Camera cam = Camera.main;
		if( cam == null )
		{
			cam = Game.Instance.GetComponentInChildren<Camera>();
			if( cam == null )
				return; 	// No valid cameras.
		}
		
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
		Vector3 targetPosition = new Vector3(focusBoundsInWorld.center.x, focusBoundsInWorld.center.y+height, cam.transform.position.z);
		Vector3 positionDiff = targetPosition - cam.transform.position;
		cam.transform.position += positionDiff * Mathf.Clamp01(speed);
	}
	
	
	/// <summary>
	/// Follow the specified components' game objects.
	/// </summary>
	
	/*public void Follow(Component[] components, float focusSpeed, float focusHeight)
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
	}*/
	
	
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
	
	/*public void Follow(ArrayList gameObjects, float focusSpeed, float focusHeight)
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
	}*/
	
	
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
	
	
	void OnEnable() {
		EnableEventSubscriptions();
	}
	
	void OnDisable() {
		DisableEventSubscriptions();
	}
	
	virtual protected void EnableEventSubscriptions () {
		Messenger.CameraEventOccurred += HandleCameraEventOccurred;
	}
	
	virtual protected void DisableEventSubscriptions () {
		Messenger.CameraEventOccurred -= HandleCameraEventOccurred;
	}
	
	#region Camera Events
	virtual protected void HandleCameraEventOccurred ( Messenger.CameraEvents camEvent, ArrayList args) {
		switch (camEvent) {
			
		case Messenger.CameraEvents.PositionStop:
			StartCoroutine(PositionStop());
			break;
		case Messenger.CameraEvents.PositionFollow:
			StartCoroutine(PositionFollow(args));
			break;
		default:
			break;
		}
	
	}
	
	virtual protected IEnumerator PositionStop () {
		yield return null;
		Stop ();
	}
	
	virtual protected IEnumerator PositionFollow(ArrayList args) {
		yield return null;
		if( args[0] == null )
			yield break;
		
		bool isArr = !args[0].GetType().Equals(typeof(GameObject));
		switch(args.Count) {
		case 1:
			if(isArr == true)
				Follow (args[0] as GameObject[]);
			else
				Follow (args[0] as GameObject);
			break;
		case 2:
			if(isArr == true)
				Follow (args[0] as GameObject[], (float)args[1]);
			else
				Follow (args[0] as GameObject, (float)args[1]);
			break;
		case 3:
			if(isArr == true)
				Follow (args[0] as GameObject[], (float)args[1], (float)args[2]);
			else
				Follow (args[0] as GameObject, (float)args[1], (float)args[2]);
			break;
		};
	}
	#endregion
}

