using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Building : MonoBehaviour 
{
#region 	VARIABLES

	
	public GameObject fullBlockPrefab;
	
	public Vector3 blockScale = new Vector3(3f, 3f, 3f);
	public float shiftMin = -3f;
	public float shiftMax = 7f;
	public int columns = 3;
	public int rows = 7;
	public bool regenerateNow = false;
	
	
	public BuildingUse use = 0;
	public BuildingTheme theme = 0;
	public bool alternating = false;
	//private BuildingStyle style;

	
#endregion
#region 	UNITY_HOOKS
	
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	
	void LateUpdate()
	{
		if( regenerateNow )
		{
			Regenerate();
			regenerateNow = false;
		}
	}
	
	
#endregion
#region 	METHODS
	
	
	/// <summary>
	/// Regenerate the building (if it's already empty).
	/// </summary>

	public void Regenerate()
	{
		// Don't mess up an already existing building in editor mode.
		// Force them to manually clear it out instead, just to be safe.
		if( !Application.isPlaying && transform.childCount > 0 )
			return;
		
		foreach(Transform child in transform)
			Destroy(child.gameObject);
		
		for( int i = 0; i < rows; i++ )
		{
			bool isBottom = (i == 0) ? true : false;
			bool isTop = (i == rows-1) ? true : false;
			if(isBottom) 
			{
				//	TODO
			} else if(isTop)
			{ 
				//	TODO
			}
				
			
			GameObject floor = new GameObject("floor" + i.ToString());
			floor.transform.parent = transform;
			floor.transform.localScale = Vector3.one;
			floor.transform.localPosition = Vector3.zero;
			
			for( int j = 0; j < columns; j++ )
			{
				if(j == 0 || j == columns - 1) 
				{
					//	TODO EDGE
					InitBlock(BlockType.Edge, isTop, isBottom, i, j, floor);
				} else
				{
					InitBlock(BlockType.Middle, isTop, isBottom, i, j, floor);
				}
			}
		}
	}
					
	private void InitBlock(BlockType bt, bool isTop, bool isBottom, int row, int column, GameObject floor)
	{
		BuildingFactory bf = Game.Instance.buildingFactory;
		GameObject newBlock = bf.CreateBlock(theme, bt, use) as GameObject;
		//GameObject newBlock = GameObject.Instantiate(fullBlockPrefab) as GameObject;
		newBlock.name = "block" + column.ToString();
		newBlock.transform.parent = floor.transform;
		newBlock.transform.localScale = blockScale;
		newBlock.transform.localPosition = new Vector3(column*blockScale.x, row*blockScale.y , 0f);
		if(isBottom)
			Destroy (newBlock.rigidbody);
	}
	
	public IEnumerator DestroyFloor(GameObject floor) 
	{
		yield return new WaitForSeconds(0.5f);

		GameObject explosion = Game.Instance.buildingFactory.CreateFloorExplosion(theme);
		explosion.transform.position = floor.transform.position;
		
		foreach(Transform trans in floor.transform)
		{
			Destroy(trans.gameObject);
		}
		Destroy (floor);
		
		yield return null;
	}
	
	
#endregion
}