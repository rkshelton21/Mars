using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuEvent : MonoBehaviour {
	private Transform _Player;
	private float _swapSides = 1f;
	private Transform _redBase;
	private Transform _blueBase;
	public Transform Creature;
	public bool TeamSpawn;
	
	void Start()
	{
		_Player = GameObject.Find("Player").transform;
		_redBase = GameObject.Find("RedBase").transform;
		_blueBase = GameObject.Find("BlueBase").transform;
	}

	public void Spawn()
	{
		if(_Player != null)
		{
			var newChar = Instantiate(Creature, _Player.position + new Vector3(2f * _swapSides, 0, 0), Quaternion.identity);

			if(TeamSpawn && Creature.tag == "Enemy")
			{
				if(_swapSides > 0)
				{
					//((Transform)newChar).SendMessage("SetTeam", "Red");
					((Transform)newChar).gameObject.layer = 12;
					((Transform)newChar).SendMessage("SetTarget", _blueBase);
					//((Transform)newChar).name = "Red" + ((Transform)newChar);					
					((Transform)newChar).SendMessage("Flip");					
				}
				if(_swapSides < 0)
				{
					//((Transform)newChar).SendMessage("SetTeam", "Blue");
					((Transform)newChar).gameObject.layer = 13;
					((Transform)newChar).SendMessage("SetTarget", _redBase);
					//((Transform)newChar).name = "Blue" + ((Transform)newChar);					
				}
			}

			_swapSides = -_swapSides;
		}
	}
}
