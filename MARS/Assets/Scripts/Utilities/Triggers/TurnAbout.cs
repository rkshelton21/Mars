using UnityEngine;
using System.Collections;

public class TurnAbout : MonoBehaviour {

	public bool SendRightOnly = false;
	public bool SendLeftOnly = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		//Debug.Log("TURN! " + name + " " + collider.name);
		if (collider.tag == "Enemy") 
		{
			//Debug.Log("BABY TURN!");
			collider.gameObject.SendMessage("Turn", new bool[2] {SendLeftOnly, SendRightOnly});
		}
	}
}
