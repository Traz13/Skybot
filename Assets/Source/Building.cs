using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Building : MonoBehaviour 
{
#region 	VARIABLES

	
	public GameObject fullBlockPrefab;
	
	public Vector3 blockScale = new Vector3(2f, 2f, 2f);
	public Vector3 padding = new Vector2(0f, 0f);
	public int columns = 4;
	public int rows = 10;
	public bool regenerateNow = false;
	
	Vector3 originalPosition;
	
	//float timer = 0f;
	//float speed = 1f;
	
	
#endregion
#region 	UNITY_HOOKS
	
	
	void Awake()
	{
		//timer = Random.value;
		//speed = Random.Range(0.5f, 2f);
		originalPosition = transform.localPosition;
	}
	
	
	void Update()
	{
		if( !Application.isPlaying )
			return;
		
		//timer += Time.deltaTime * speed;
		
		/*Vector3 position = originalPosition;
		position.y += Mathf.Sin(timer) * 15f;
		transform.localPosition = position;*/
	}
	
	
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
	/// Regenerate this instance.
	/// </summary>

	public void Regenerate()
	{
		// Don't mess up an already existing building in editor mode.
		// Force them to manually clear it out instead, to be safe.
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
				
				Vector3 position = new Vector3(j*blockScale.x, i*blockScale.y , 0f);
				position.x += j*padding.x*blockScale.x;
				position.y += i*padding.y*blockScale.y;
				
				newBlock.transform.localPosition = position;
			}
		}
	}
	
	
#endregion
}
