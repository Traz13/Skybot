using UnityEngine;
using System.Collections;

public class Exploder : MonoBehaviour
{
	public delegate void DidExplode(Exploder exploder);
	public event DidExplode didExplode;
	
	public float force = 1f;
	public float radius = 1f;
	public float upwardsModifier = 1f;
	public bool explodeNow = false;
	
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	
	void Update()
	{
		if( explodeNow )
		{
			Explode();
			explodeNow = false;
		}
	}
	
	
	/// <summary>
	/// Cause a physical explosion, knocking back players. (Note: does not cause damage)
	/// </summary>

	public void Explode()
	{
		// Cause a knock-back effect on all of the players.
		// TODO: We may want to expand this to knock-back NPC objects.
		foreach( Player player in Game.Instance.players )
			player.rigidbody.AddExplosionForce(force, transform.position, radius, upwardsModifier);
		
		if( didExplode != null )
			didExplode(this);
	}
}

