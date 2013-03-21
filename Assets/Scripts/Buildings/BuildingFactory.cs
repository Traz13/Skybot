using UnityEngine;
using System.Collections;

public class BuildingFactory : StaticInstance<BuildingFactory> {
	
	public GameObject defaultBlock;
	public GameObject defaultBlockExplosion;
	public GameObject defaultFloorExplosion;
	
	public BuildingThemeFactory[] themeFactories = new BuildingThemeFactory[(int)BuildingTheme.Count];
	
	public GameObject CreateBlock(BuildingTheme theme, BlockType type, BuildingUse use) {
		GameObject obj = themeFactories[(int)theme].CreateBlock(type, use);
		if(obj == null)
			return defaultBlock;
		return obj;
	}
	
	public GameObject CreateFloorExplosion(BuildingTheme theme) {
		return themeFactories[(int)theme].CreatFloorExplosion();
	}
	
	public GameObject CreateBlockExplosion(BuildingTheme theme) {
		return themeFactories[(int)theme].CreatBlockExplosion();
	}
	
	//public BlockByTheme[] byTheme = new BlockByTheme[(int)BuildingTheme.Count];
	
	/*[System.Serializable]
	public class BlockByTheme{
		public BlockByUse[] byUse = new BlockByUse[(int)BuildingUse.Count];
		public BlockByUse this [int index] {
	        get{
	            return byUse[index];
	        }
	        set{
	            byUse[index] = value;
	        }
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
	}*/
}
