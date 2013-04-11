using UnityEngine;
using System.Collections;

enum CameraEvent {
	PositionStop,
	PositionFollow,
	
	FovStop,
	FovAdjust,
};

enum GameEvent {
	Begin,
	End,
}

/// <summary>
/// Game message.
/// This system is an easy abstraction from the base Messsenger system.
/// The reason for this class is to help make sure we are using the
/// correct object types for each message.
/// </summary>
static internal class GameMsg {
	public static void CameraStop(){
		Messenger.Broadcast(CameraEvent.PositionStop);
	}
	
	private static void CameraFollow(ArrayList lst) {
		Messenger.Broadcast<ArrayList>(CameraEvent.PositionFollow, lst);
	}
	
	public static void CameraFollow(GameObject obj){
		ArrayList lst = new ArrayList();
		lst.Add(obj);
		CameraFollow(lst);
	}
	
	public static void CameraFollow(GameObject obj, float speed){
		ArrayList lst = new ArrayList();
		lst.Add(obj);
		lst.Add(speed);
		CameraFollow(lst);
	}
	
	public static void CameraFollow(GameObject obj, float speed, float height){
		ArrayList lst = new ArrayList();
		lst.Add(obj);
		lst.Add(speed);
		lst.Add(height);
		CameraFollow(lst);
	}
	
	public static void CameraFollow(GameObject[] objs){
		ArrayList lst = new ArrayList();
		lst.Add(objs);
		CameraFollow(lst);
	}
	
	public static void CameraFollow(GameObject[] objs, float speed){
		ArrayList lst = new ArrayList();
		lst.Add(objs);
		lst.Add(speed);
		CameraFollow(lst);
	}
	
	public static void CameraFollow(GameObject[] objs, float speed, float height){
		ArrayList lst = new ArrayList();
		lst.Add(objs);
		lst.Add(speed);
		lst.Add(height);
		CameraFollow(lst);
	}
	
	public static void FOVStop() {
		Messenger.Broadcast(CameraEvent.FovStop);
	}
	
	public static void FOVAdjust(float fov) {
		ArrayList lst = new ArrayList();
		lst.Add(fov);
		Messenger.Broadcast<ArrayList>(CameraEvent.FovAdjust, lst);
	}
	
	public static void FOVAdjust(float fov, float speed) {
		ArrayList lst = new ArrayList();
		lst.Add(fov);
		lst.Add(speed);
		Messenger.Broadcast<ArrayList>(CameraEvent.FovAdjust, lst);
	}
	
	public static void BeginGame() {
		Messenger.Broadcast(GameEvent.Begin);
	}
	
	public static void EndGame() {
		Messenger.Broadcast(GameEvent.End);
	}
	
}
