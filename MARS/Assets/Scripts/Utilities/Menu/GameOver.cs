using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {

	Transform _Player;
	public Transform _Menu;

	// Use this for initialization
	void Start () {
		_Player = GameObject.Find("Player").transform;	
		_Menu.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(_Player == null)
		{
			_Menu.gameObject.SetActive(true);
		}
	}
}
