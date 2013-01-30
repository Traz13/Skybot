using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {
	
	private Vector3 bobOffset = new Vector3(0f, 3f, 0f);

	// Use this for initialization
	void Start () {
		StartCoroutine(delayStart());
		//BobUp();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	float RandomDelay() {
		return (float)Random.Range(1, 20)/10f;
	}
	
	IEnumerator delayStart() {
		yield return new WaitForSeconds(RandomDelay());
		if(Random.Range(0,2) == 1)
			BobUp();
		else
			BobDown();
		yield return null;
	}
	
	void BobUp() {
		Hashtable args = new Hashtable();
		args["amount"] = bobOffset;
		args["time"] = 5f;
		args["delay"] = RandomDelay();
		args["oncomplete"] = "BobDown";
		args["oncompletetarget"] = gameObject;
		args["easetype"] = iTween.EaseType.easeInOutCubic;
		iTween.MoveBy(gameObject, args);
	}
	
	void BobDown() {
		Hashtable args = new Hashtable();
		args["amount"] = bobOffset * -1f;
		args["time"] = 5f;
		args["delay"] = RandomDelay();
		args["oncomplete"] = "BobUp";
		args["oncompletetarget"] = gameObject;
		args["easetype"] = iTween.EaseType.easeInOutCubic;
		iTween.MoveBy(gameObject, args);
	}
}
