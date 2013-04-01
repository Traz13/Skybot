using UnityEngine;
using System.Collections;

public class CameraFov : StaticInstance<CameraFov>
{
	public readonly float defaultSpeed = 1f;
	
	public float fov = 60f;
	public float speed = 1f;
	public bool on = false;
	
	
	void Update ()
	{
		if( !on )
			return;
		
		Camera cam = Camera.main;
		if( cam == null )
		{
			cam = Game.Instance.GetComponentInChildren<Camera>();
			if( cam == null )
				return; 	// No valid cameras.
		}
		
		float diff = fov - cam.fov;
		cam.fov += diff * speed * Time.deltaTime;
		
		if( Mathf.Approximately(cam.fov, fov) )
			on = false;
	}
	
	
	public void AdjustTo(float targetFov, float adjustmentSpeed)
	{
		fov = targetFov;
		speed = adjustmentSpeed;
		on = true;
	}
	
	
	public void AdjustTo(float targetFov)
	{
		AdjustTo(targetFov, defaultSpeed);
	}
	
	
	public void Stop()
	{
		on = false;
	}
}

