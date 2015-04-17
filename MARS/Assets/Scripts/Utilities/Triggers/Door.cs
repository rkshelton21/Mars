using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	public bool _enabled = false;
	public bool LoadPreviousLevel = false;
	
	// Use this for initialization
	void Start () {
	
	}

	void Update()
	{
		if (Input.GetButton("Jump") && _enabled) 
		{
			PlayerData.SaveGame();
			var i = Application.loadedLevel;
			if(!LoadPreviousLevel)
			{
				Application.LoadLevel(i+1);
			}
			else
			{
				Application.LoadLevel(i-1);
			}
		}
	}

	void OnTriggerExit2D(Collider2D collider)
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
