using UnityEngine;
using System.Collections;

public class MovementTest : MonoBehaviour {

	private bool right = true;
	public float speed = 30f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (right) {
				if (transform.position.x > 200) {
						right = false;
				} else {
				transform.Translate (new Vector3 (speed * Time.deltaTime, 0f, 0f));
				}
		}
		else {
			if (transform.position.x < -100) {
				right = true;
			}else{
				transform.Translate (new Vector3 (-speed * Time.deltaTime, 0f, 0f));
			}
		}
	}
}
