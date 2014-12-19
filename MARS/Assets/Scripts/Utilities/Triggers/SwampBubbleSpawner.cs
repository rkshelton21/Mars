using UnityEngine;
using System.Collections;

public class SwampBubbleSpawner : MonoBehaviour {

	public Transform Spawner;
	private Spawner _spawner = null;
	public float XVariant = 0;

	// Use this for initialization
	void Start () {
		if (Spawner == null)
			Debug.LogError ("Spawner not attached.");
		_spawner = Spawner.GetComponent<Spawner> ();
		if (_spawner == null)
			Debug.LogError ("Spawner not attached.");

		_spawner.SetRandomX (XVariant);
		_spawner.StartSpawning ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
