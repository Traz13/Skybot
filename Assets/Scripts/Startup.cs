using UnityEngine;
using System.Collections;

public class Startup : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		Object.DontDestroyOnLoad(UI.instance);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

