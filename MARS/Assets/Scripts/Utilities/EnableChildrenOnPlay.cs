using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnableChildrenOnPlay : MonoBehaviour {
	public List<string> ChildrenToEnable = new List<string>();

	// Use this for initialization
	void Start () {
		foreach(var name in ChildrenToEnable)
		{
			var child = transform.FindChild(name);
			if(child != null)
				child.gameObject.SetActive(true);
    	}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
