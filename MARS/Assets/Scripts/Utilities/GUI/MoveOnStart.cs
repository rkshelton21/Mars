using UnityEngine;
using System.Collections;

public class MoveOnStart : MonoBehaviour {
	public Vector2 StartPosition;
	
	void Awake()
	{
		transform.position = StartPosition;
	}	
}
