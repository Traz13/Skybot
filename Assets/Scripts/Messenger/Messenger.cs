using UnityEngine;
using System.Collections;

public class Messenger : StaticInstance<Messenger>{

	void Awake(){
		DontDestroyOnLoad(this);
	}
	
	public enum CameraEvents{PositionStop, PositionFollow};
	
	public delegate void CameraEvent(CameraEvents sceneEvent, ArrayList objs);
	public static event CameraEvent CameraEventOccurred;
	
	public static void ReportCameraEvent(CameraEvents camEvent, ArrayList objs){
		if (CameraEventOccurred != null){
			CameraEventOccurred(camEvent, objs);	
		}
	}
	
	public static void ReportCameraStopEvent(){
		ReportCameraEvent(CameraEvents.PositionStop, null);
	}
	
	public static void ReportCameraFollowEvent(GameObject obj){
		ArrayList lst = new ArrayList();
		lst.Add(obj);
		ReportCameraEvent(CameraEvents.PositionFollow, lst);
	}
	
	public static void ReportCameraFollowEvent(GameObject obj, float speed){
		ArrayList lst = new ArrayList();
		lst.Add(obj);
		lst.Add(speed);
		ReportCameraEvent(CameraEvents.PositionFollow, lst);
	}
	
	public static void ReportCameraFollowEvent(GameObject obj, float speed, float height){
		ArrayList lst = new ArrayList();
		lst.Add(obj);
		lst.Add(speed);
		lst.Add(height);
		ReportCameraEvent(CameraEvents.PositionFollow, lst);
	}
	
	public static void ReportCameraFollowEvent(GameObject[] objs){
		ArrayList lst = new ArrayList();
		lst.Add(objs);
		ReportCameraEvent(CameraEvents.PositionFollow, lst);
	}
	
	public static void ReportCameraFollowEvent(GameObject[] objs, float speed){
		ArrayList lst = new ArrayList();
		lst.Add(objs);
		lst.Add(speed);
		ReportCameraEvent(CameraEvents.PositionFollow, lst);
	}
	
	public static void ReportFollowEvent(GameObject[] objs, float speed, float height){
		ArrayList lst = new ArrayList();
		lst.Add(objs);
		lst.Add(speed);
		lst.Add(height);
		ReportCameraEvent(CameraEvents.PositionFollow, lst);
	}
}
