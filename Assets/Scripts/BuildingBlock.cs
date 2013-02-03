using UnityEngine;
using System.Collections;

public class BuildingBlock : MonoBehaviour 
{		
	// Health
	float mHealth = 100f;
	public float health {
		get { return mHealth; }
		set {
			mHealth = value;
			if( mHealth <= 0f )
				Destroy(gameObject);
		}
	}
}
