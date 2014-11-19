using UnityEngine;
using System.Collections;

public class Consumable : MonoBehaviour {

	private Transform _Player;
	private PlayerContoller2D _PlayerController;
	public string Name;

	void Start()
	{
		_Player = GameObject.Find("Player").transform;
		_PlayerController = _Player.GetComponent<PlayerContoller2D>();
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.tag == "Player")
		{
			_PlayerController.ConsumeItem(Name);
			Destroy(this.gameObject);
		}
	}
}
