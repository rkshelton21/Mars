using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicSpawnTrigger : MonoBehaviour {

	public Transform Spawner;
	private List<Spawner> _spawners = null;
	public bool SpawnOnEntry = false;
	public List<Transform> AdditionalSpawners;

	// Use this for initialization
	void Start () {
		_spawners = new List<Spawner> ();
		if (Spawner == null)
			Debug.LogError ("Spawner not attached.");
		_spawners.Add(Spawner.GetComponent<Spawner> ());

		if (AdditionalSpawners != null) 
		{
			if(AdditionalSpawners.Count > 0)
			{
				foreach(var s in AdditionalSpawners)
				{
					_spawners.Add(Spawner.GetComponent<Spawner> ());
				}
			}
		}


		if (SpawnOnEntry) 
		{
			foreach(var s in _spawners)
			{
				s.StartSpawningOnRequest();
			}
		}
	}
	
	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Player") 
		{
			if(!SpawnOnEntry)
			{
				foreach(var s in _spawners)
				{
					s.StartSpawning();
				}
			}
			else
			{
				foreach(var s in _spawners)
				{
					s.Spawn();
				}
			}
		}
	}
}
