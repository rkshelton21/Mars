using UnityEngine;
using System.Collections;

public class SimpleParallax : MonoBehaviour {

	private float _camInitialX;
	private float _camInitialY;
	private float _initialX;
	private float _initialY;

	public float Offset;
	public bool FollowCamera = false;
	public bool LockY = false;
	public Transform Cam;

	// Use this for initialization
	void Start () 
	{
		

	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if (Cam == null)
		{	
			Cam = GameObject.Find ("Main Camera").transform;
			_initialX = transform.position.x;
			_initialY = transform.position.y;
			
			_camInitialX = Cam.position.x;
			_camInitialY = Cam.position.x;						
		}
		
		var p = transform.position;
		if(FollowCamera)
		{
			if (Offset + _initialX != 0)
			{
				p.x = (Cam.position.x - _camInitialX) / Offset + _initialX;
			}

			if (Offset + _initialY != 0 && !LockY)
			{
				p.y = (Cam.position.y - _camInitialY) / Offset + _initialY;
			}
		}
		else
		{
			if (Offset + _initialX != 0)
			{
				p.x = (_camInitialX - Cam.position.x) / Offset + _initialX;		
			}
			if (Offset + _initialY != 0 && !LockY)
			{
				p.y = (_camInitialY - Cam.position.y) / Offset + _initialY;		
			}
		}

		//var newPos = p;
		//transform.position = Vector3.Lerp( transform.position, p, 0.02f );
		transform.position = p;

		//var diff = camera.transform.position.x - activesprite.transform.position.x;
	}
}
