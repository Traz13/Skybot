using UnityEngine;
using System.Collections;

public class Oblongabomb : MonoBehaviour 
{
	void Awake() 
	{
		rigidbody.AddTorque(new Vector3(0f, 0f, 50f));
	}
}
