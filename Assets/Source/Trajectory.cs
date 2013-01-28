using UnityEngine;
using System.Collections;

public class Trajectory
{	
	/// <summary>
	/// Return an array of positions that are predicted with the given normalized
	/// direction, velocity, and sample count.
	/// </summary>
	
	public static Vector3[] PredictPositions(Vector3 position, Vector3 direction, float velocity, int sampleCount, float timeStep)
	{
		// Make the first predicted position the same as our starting position (t=0).
		Vector3[] predictedPositions = new Vector3[sampleCount];
		predictedPositions[0] = position;
		
		// Calculate starting velocity.
		float angle = Vector3.Angle(direction, Vector3.right) * Mathf.Deg2Rad;
		if( direction.y < 0f )
			angle = -angle;
		
		Vector3 v0 = new Vector3();
		v0.x = velocity*Mathf.Cos(angle);
		v0.y = velocity*Mathf.Sin(angle);
		
		// Iterate through each time step, calculting the velocity and position
		// at each step.
		float gravity = Physics.gravity.y;
		for( int i = 1; i < sampleCount; i++ )
		{
			float t = i*timeStep;
			
			// Calculate new velocity.
			// TODO Account for drag.
			Vector3 newPosition = new Vector3();
			newPosition.x = position.x + v0.x*t;
			newPosition.y = position.y + v0.y*t + 0.5f*gravity*t*t;
			
			// Calculate the new position by adding the new velocity to
			// the previous position.
			predictedPositions[i] = newPosition;
		}
		
		return predictedPositions;
	}
	
	
	/// <summary>
	/// Return an array of positions that are predicted with the given starting position,
	/// velocity vector, and sample count.
	/// </summary>
	
	public static Vector3[] PredictPositions(Vector3 position, Vector3 velocity, int sampleCount, float timeStep)
	{		
		float magnitude = velocity.magnitude;
		Vector3 direction = velocity.normalized;
		return Trajectory.PredictPositions(position, direction, magnitude, sampleCount, timeStep);
	}
}

