using UnityEngine;
using System.Collections;

public class Oblongabomb : MonoBehaviour 
{
	void Awake() 
	{
		rigidbody.maxAngularVelocity = 300f;
		rigidbody.AddTorque(new Vector3(0f, 0f, -300f)/Time.timeScale, ForceMode.Impulse);
	}
}
