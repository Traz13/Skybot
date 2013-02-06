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

	
#endregion
#region 	UNITY_HOOKS
	
	
	/// <summary>
	/// Start this instance.
	/// </summary>
	
	void Start()
	{
		if( Application.isPlaying )
		{
			iTween.MoveTo(gameObject, new Hashtable() {
				{ "position", new Vector3(transform.position.x, Random.Range(-3f, 7f), transform.position.z) },
				{ "easetype", "easeInOutSine" },
				{ "time", 3f }
			});
		}
		
		//Game.instance.turnWillBegin += gameTurnWillBegin;
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
	
	
	void gameTurnWillBegin(Game game)
	{
		iTween.MoveTo(gameObject, new Hashtable() {
			{ "position", new Vector3(transform.position.x, 0f, transform.position.z) },
			{ "easetype", "easeInOutSine" },
			{ "time", 3f }
		});
	}
	
	
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
				
				Vector3 position = new Vector3(j*blockScale.x, i*blockScale.y , 0f);
				position.x += j*padding.x*blockScale.x;
				position.y += i*padding.y*blockScale.y;
				
				newBlock.transform.localPosition = position;
			}
		}
	}
	
	
#endregion
}
