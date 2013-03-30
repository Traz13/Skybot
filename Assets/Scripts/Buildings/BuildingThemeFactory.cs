using UnityEngine;
using System.Collections;

public class BuildingThemeFactory : MonoBehaviour {
	
	public BlockByUse[] byUse = new BlockByUse[(int)BuildingUse.Count];
	public GameObject blockExplosion;
	public GameObject floorExplosion;
	
	public GameObject CreateBlock(BlockType type, BuildingUse use) {
		GameObject obj = byUse[(int)use][(int)type];
		if(obj != null)
			obj = Instantiate(obj) as GameObject;
		return obj;
	}
	
	public GameObject CreatBlockExplosion() {
		if(blockExplosion == null)
			return null;
		return Instantiate(blockExplosion) as GameObject;
	}
	
	public GameObject CreatFloorExplosion() {
		if(floorExplosion == null)
			return null;
		return Instantiate(floorExplosion) as GameObject;
	}
	
	[System.Serializable]
	public class BlockByUse{
		public GameObject[] byType = new GameObject[(int)BlockType.Count];
		public GameObject this [int index] {
	        get{
	            return byType[index];
	        }
	        set{
	            byType[index] = value;
	        }
		}
	}
}
