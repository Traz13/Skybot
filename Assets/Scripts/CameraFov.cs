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
		if( !on || Camera.main == null )
			return;
		
		float diff = fov - Camera.main.fov;
		Camera.main.fov += diff * speed * Time.deltaTime;
		
		if( Mathf.Approximately(Camera.main.fov, fov) )
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

