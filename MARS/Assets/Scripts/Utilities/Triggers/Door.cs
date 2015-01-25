using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	private bool _enabled = false;
	// Use this for initialization
	void Start () {
	
	}

	void Update()
	{
		if (Input.GetKey (KeyCode.Z) && _enabled) 
		{
			var i = Application.loadedLevel;
			Application.LoadLevel(i+1);
		}
	}

	void OnTriggerLeave2D(Collider2D collider)
	{
		if (collider.tag == "Player") 
		{
			_enabled = false;
		}
	}

	void OnTriggerStay2D(Collider2D collider)
	{
		if (collider.tag == "Player") 
		{
			_enabled = true;
		}
	}
}
