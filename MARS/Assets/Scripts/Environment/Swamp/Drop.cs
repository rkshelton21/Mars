﻿using UnityEngine;
using System.Collections;

public class Drop : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Ground") 
		{
			Destroy(gameObject);
		}
	}
}
