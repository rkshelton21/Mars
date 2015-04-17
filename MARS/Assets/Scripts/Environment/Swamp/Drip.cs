using UnityEngine;
using System.Collections;

public class Drip : SimpleAnimation 
{
	private Spawner _spawner;

	// Use this for initialization
	public override void Init() 
	{
		_spawner = transform.parent.gameObject.GetComponentInChildren<Spawner> ();
		if (_spawner == null)
			Debug.LogError ("Could not find spawner.");

		_spawner.StartSpawningOnRequest ();
	}
	
	public override void CycleComplete()
	{
		if (_spawner == null)
			Debug.LogError ("Could not find spawner.");

		bool spawned = _spawner.Spawn ();
		if (!spawned)
			Debug.LogError ("Couldn't spawn");
	}
}
