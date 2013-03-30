using UnityEngine;
using System.Collections;

public class BuildingBlock : MonoBehaviour 
{		
	float mHealth 					= 100f;
	Building building 				= null;
	public Collectable collectable 	= null;
	public Material destroyedMat	= null;
	private Material backupMat		= null;
	private bool mDestroyed 		= false;
	public bool isDestroyed() 		{ return mDestroyed; }
	
	
	void Start() {
		//building = transform.parent.parent.gameObject.GetComponent<Building>();
		building = NGUITools.FindInParents<Building>(gameObject);
	}
	
	void OnCollisionEnter(Collision coll) {
		if(coll.gameObject.tag.Equals("Projectile")) {
			StartCoroutine(ProjectilCollision(coll));
			lightUp = true;
			lightTimer = 0;
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
	
	public void Repair()
	{
		if(mDestroyed == false)
			return;
		
		renderer.material = backupMat;
	}
	
	private IEnumerator ProjectilCollision(Collision coll)
	{
		yield return null;
		
		if(mDestroyed == false)
		{
			mDestroyed = true;
			backupMat = renderer.material;
			renderer.material = destroyedMat;
		}
		
		GameObject explosion = Game.Instance.buildingFactory.CreateBlockExplosion(building.theme);
		explosion.transform.position = transform.position;
		//GameObject.Instantiate(explosion, transform.position, Quaternion.identity);
		
		// Check for floor destruction
		int numDestroyed = 1;
		for(int i =  0; i < transform.parent.childCount; i++) {
			Transform child = transform.parent.GetChild(i);
			if(child == transform)
				continue;
			
			if(!child.GetComponent<BuildingBlock>().isDestroyed())
				continue;
			
			numDestroyed++;
			/*	break;
				
			if(i == transform.parent.childCount-1) {
				//Building building = NGUITools.FindInParents<Building>(gameObject);
				
				//	TODO floor explosion
				building.DestroyFloor(transform.parent.gameObject);
			}*/
		}
		
		float fVal = ((float)numDestroyed/(float)transform.parent.childCount);
		if(fVal >= 0.5f)
			StartCoroutine(building.DestroyFloor(transform.parent.gameObject));
	}
	
	
	bool lightUp = false;
	float lightTimer = 0;
	float lightDuration = 0.5f;
	
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	
	void Update()
	{
		if( lightUp )
		{
			lightTimer += Time.deltaTime;
			
			float lightAmount = 0;
			if( lightTimer > lightDuration )
				lightUp = false;
			else
			{			
				// Adjust the light smoothly up and down along a single sin wave.
				lightAmount = Mathf.Sin(Mathf.Lerp(0, Mathf.PI, lightTimer / lightDuration));
			}
			
			// HACK: This will currently only work on vertex lit materials.
			renderer.material.SetColor("_Emission", new Color(lightAmount, lightAmount, lightAmount, 1));
		}
	}
}
