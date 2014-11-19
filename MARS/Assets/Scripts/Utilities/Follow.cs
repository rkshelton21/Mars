using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {
	public Transform Target;
	private Transform self;

	// Use this for initialization
	void Start () {
		self = transform;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		self.position = Target.position;
	}
}
