using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Building : MonoBehaviour 
{
#region 	VARIABLES

	
	public GameObject fullBlockPrefab;
	
	public Vector3 blockScale = new Vector3(2f, 2f, 2f);
	public float shiftMin = -3f;
	public float shiftMax = 7f;
	public int columns = 4;
	public int rows = 10;
	public bool regenerateNow = false;

	
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
		
		for( int i = 0; i < rows; i++ )
		{
			GameObject floor = new GameObject("floor" + i.ToString());
			floor.transform.parent = transform;
			floor.transform.localScale = Vector3.one;
			floor.transform.localPosition = Vector3.zero;
			
			for( int j = 0; j < columns; j++ )
			{
				GameObject newBlock = GameObject.Instantiate(fullBlockPrefab) as GameObject;
				newBlock.name = "block" + j.ToString();
				newBlock.transform.parent = floor.transform;
				newBlock.transform.localScale = blockScale;
				newBlock.transform.localPosition = new Vector3(j*blockScale.x, i*blockScale.y , 0f);;
			}
		}
	}
	
	
#endregion
}
