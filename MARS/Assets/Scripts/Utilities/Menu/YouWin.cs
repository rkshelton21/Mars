using UnityEngine;
using System.Collections;

public class YouWin : MonoBehaviour {

	public Transform Boss;
	public Transform _Menu;
	
	// Use this for initialization
	void Start () {
		_Menu.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Boss == null)
		{
			_Menu.gameObject.SetActive(true);
		}
	}
}
