using UnityEngine;
using System.Collections;

public class DontDestroyOnLoad : MonoBehaviour
{
	void Awake()
	{
		Object.DontDestroyOnLoad(gameObject);
	}
}

