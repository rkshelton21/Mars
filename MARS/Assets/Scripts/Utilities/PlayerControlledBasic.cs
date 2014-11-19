using UnityEngine;
using System.Collections;

public class PlayerControlledBasic : MonoBehaviour {

	public float Speed = 10;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float move = Input.GetAxis ("Horizontal");
		rigidbody2D.velocity = new Vector2 (move * Speed, rigidbody2D.velocity.y);
		//rigidbody2D.AddForce(new Vector2 (move * Speed, 0f));
	}
	
	/*// Update is called once per frame
	void Update () {
		float move = Input.GetAxis ("Horizontal");
		rigidbody2D.velocity = new Vector2 (move * Speed, rigidbody2D.velocity.y);
		rigidbody2D.AddForce(new Vector2 (move * Speed, 0f));
	}*/
}
