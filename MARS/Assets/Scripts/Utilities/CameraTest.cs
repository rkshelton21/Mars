using UnityEngine;
using System.Collections;

public class CameraTest : MonoBehaviour {

	public Transform Target;
	private Transform Self;
	private float SizeModifier = 1;
	public int Zoom = 40;
	// Use this for initialization
	void Start () {
		Self = transform;
		camera.orthographicSize = Screen.height*SizeModifier / (2 * Zoom);
	}
	
	// Update is called once per frame
	void LateUpdate () {
		//Self.position = new Vector3(Target.position.x, Target.position.y, Self.position.z);
		var newPos = Vector3.Lerp (Self.position, Target.position, 10 * Time.deltaTime);
		newPos.z = Self.position.z;
		Self.position = newPos;
	}
}
