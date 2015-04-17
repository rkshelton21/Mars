using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {

	Transform _Player;
	public Transform _Menu;
	private bool _PlayerExists = false;

	// Use this for initialization
	void Start () {
		var p = GameObject.Find("Player");	
		if(p != null)
		{
			_Player = p.transform;				
		}
		_Menu.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(_Player == null)
		{
			_Menu.gameObject.SetActive(true);
			
			var p = GameObject.Find("Player");	
			if(p != null)
			{
				_Player = p.transform;				
			}			
		}
		else
		{
			_Menu.gameObject.SetActive(false);
		}
	}
}
