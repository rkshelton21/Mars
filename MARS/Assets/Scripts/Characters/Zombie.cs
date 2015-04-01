using UnityEngine;
using System.Collections;


public class Zombie : MonoBehaviour {

	public Sprite[] sprites;
	public float framesPerSecond;
	public float moveSpeed;
	public float turnSpeed;

	private SpriteRenderer spriteRenderer;
	private Vector3 moveDirection;
	public Transform Target;

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<Renderer>() as SpriteRenderer;
		moveDirection = Vector3.right;
	}
	
	// Update is called once per frame
	void Update () {
		if(sprites.Length > 0)
		{
			int index = (int)(Time.timeSinceLevelLoad * framesPerSecond);
			index = index % sprites.Length;
			spriteRenderer.sprite = sprites[ index ];
		}

		// 1
		Vector3 currentPosition = transform.position;
		// 2
		if(Target == null)
		{
			if( Input.GetButton("Fire1") ) {
				// 3
				Vector3 moveToward = GetComponent<UnityEngine.Camera>().ScreenToWorldPoint( Input.mousePosition );
				// 4
				moveDirection = moveToward - currentPosition;
				moveDirection.z = 0; 
				moveDirection.Normalize();
			}
		}
		else
		{
			Vector3 moveToward = Target.position;
			// 4
			moveDirection = moveToward - currentPosition;
			moveDirection.z = 0; 
			moveDirection.Normalize();		
		}

		Vector3 target = moveDirection * moveSpeed + currentPosition;
		transform.position = Vector3.Lerp( currentPosition, target, Time.deltaTime );

		float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
		transform.rotation = 
			Quaternion.Slerp( transform.rotation, 
			                 Quaternion.Euler( 0, 0, targetAngle ), 
			                 turnSpeed * Time.deltaTime );
	}
}
