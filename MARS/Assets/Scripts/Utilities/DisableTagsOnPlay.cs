using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DisableTagsOnPlay : MonoBehaviour {
	public List<string> TagsToDisable = new List<string>();
	public bool Clean = false;

	// Use this for initialization
	void Start () {
		Purge ();
	}

	private void Purge()
	{
		foreach(var name in TagsToDisable)
		{
			var taggedObjects = GameObject.FindGameObjectsWithTag(name);
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
