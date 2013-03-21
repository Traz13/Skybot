using UnityEngine;
using System.Collections;

public class BuildingBlock : MonoBehaviour 
{		
	float mHealth = 100f;
	Building building = null;
	public Material destroyedMat;
	private bool mDestroyed = false;
	public bool isDestroyed() { return mDestroyed; }
	
	
	void Start() {
		//building = transform.parent.parent.gameObject.GetComponent<Building>();
		building = NGUITools.FindInParents<Building>(gameObject);
	}
	
	void OnCollisionEnter(Collision coll) {
		if(coll.gameObject.tag.Equals("Projectile")) {
			StartCoroutine(ProjectilCollision(coll));
		}
	}
	
	// Health
	public float health {
		get { return mHealth; }
		set {
			mHealth = value;
			if( mHealth <= 0f )
				Destroy(gameObject);
		}
	}
	
	private void Destroy() {
		
	}
	
	private IEnumerator ProjectilCollision(Collision coll) {
		yield return null;
		
		mDestroyed = true;
		renderer.material = destroyedMat;
		
		GameObject explosion = BuildingFactory.Instance.CreateBlockExplosion(building.theme);
		explosion.transform.position = transform.position;
		//GameObject.Instantiate(explosion, transform.position, Quaternion.identity);
		
		// Check for floor destruction
		for(int i =  0; i < transform.parent.childCount; i++) {
			Transform child = transform.parent.GetChild(i);
			if(child == transform)
				continue;
			
			if(!child.GetComponent<BuildingBlock>().isDestroyed())
				break;
				
			if(i == transform.parent.childCount-1) {
				//Building building = NGUITools.FindInParents<Building>(gameObject);
				
				//	TODO floor explosion
				//building.DestroyFloor(transform.parent.gameObject);
			}
		}
	}
}
