using UnityEngine;
using System.Collections;

public abstract class StaticInstance<T> : MonoBehaviour where T : Component
{
    private static T _instance = null;

    public static T Instance
    {
        get
        {
            if ((object)_instance == null)
            {
               _instance = (T)FindObjectOfType(typeof(T));
                if ((object)_instance == null)
                {
                    GameObject sys = GameObject.Find("SystemInternals");
                    if (sys == null)
					{
                        sys = new GameObject("SystemInternals");
						Object.DontDestroyOnLoad(sys);
					}
                    GameObject go = new GameObject(typeof(T).Name);
                    go.transform.parent = sys.transform;
                    Debug.Log("Creating singleton of type " + typeof(T).Name, go);
                    _instance = go.AddComponent<T>();
                }
            }
            return _instance;
        }
    }
	
	void OnEnable(){
		T[] componenetArray = GameObject.FindObjectsOfType(typeof(T)) as T[];	
		if (componenetArray.Length > 1){
			DestroyObject(this.gameObject);
		}

	}
}
