using UnityEngine;
using System.Collections;

public class BasicSpawnTrigger : MonoBehaviour {

	public Transform Spawner;
	private Spawner _spawner = null;

	// Use this for initialization
	void Start () {
		if (Spawner == null)
			Debug.LogError ("Spawner not attached.");
		_spawner = Spawner.GetComponent<Spawner> ();

		if (_spawner == null)
			_spawner = Spawner.GetComponentInChildren<Spawner> ();

		if (_spawner == null)
			Debug.LogError ("Spawner not attached.");
	}
	
	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Player") 
		{
			if(_spawner == null)
				Debug.Log("What?");
			_spawner.StartSpawning();
		}
	}
}
