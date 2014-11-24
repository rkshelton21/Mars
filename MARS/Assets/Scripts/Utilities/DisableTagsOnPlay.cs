﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DisableTagsOnPlay : MonoBehaviour {
	public List<string> TagsToDisable = new List<string>();
	public bool Clean = false;

	// Use this for initialization
	void Start () {

	}

	private void Purge()
	{
		foreach(var name in TagsToDisable)
		{
			Debug.Log(name);
			var taggedObjects = GameObject.FindGameObjectsWithTag(name);
			Debug.Log(taggedObjects.Length);
			foreach(var obj in taggedObjects)
			{
				obj.gameObject.SetActive(false);
			}
		}
		Clean = false;
	}

	// Update is called once per frame
	void Update () {
		if(Clean) Purge();
	}
}
