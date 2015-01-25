using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {

	public Transform Target;
	private Transform Self;
	public int PixelsToUnits = 100;
	public int Zoom = 1;
	public bool ZoomIn = true;
	public bool Recalculate = false;
	
	public bool Shake = false;
	private Vector2 PreShakePosition;
	public Vector2 ShakeRange = new Vector2(0.1f, 0.1f);
	public float ShakeDuration = 2f;
	public float ShakeSpeed_Max = 1f;
	private float ShakeTimer = 0f;
	private float ShakeSpeed = 0f;
	private bool FirstShake = false;
	private Vector2 ShakeOffset;
	// Use this for initialization
	void Start () {
		Self = transform;
		SetOrthoSize ();
		
		Shake = false;
		ShakeTimer = ShakeDuration;
		ShakeSpeed = ShakeSpeed_Max;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (Recalculate) 
		{
			SetOrthoSize();
		}

		if (Target != null) {
			//Self.position = new Vector3(Target.position.x, Target.position.y, Self.position.z);
			var newPos = Vector3.Lerp (Self.position, Target.position, Time.deltaTime * 10f);
			newPos.z = Self.position.z;
			Self.position = newPos;
		}
		
		if(Shake)
		{
			if(FirstShake)
			{
				FirstShake = false;
				ShakeOffset = Vector2.Scale(SmoothRandom.GetVector2(ShakeSpeed), ShakeRange);
			}
			
			ShakeTimer -= Time.deltaTime;
			var shakePos = PreShakePosition + Vector2.Scale(SmoothRandom.GetVector2(ShakeSpeed), ShakeRange) - ShakeOffset;
			Self.position = new Vector3(shakePos.x, shakePos.y, Self.position.z);
			
			//ShakeSpeed = ShakeSpeed / 2f;
			//ShakeSpeed *= -1f;
			//ShakeRange.x *= -1f;
			
			if(ShakeTimer < 0)
			{
				Shake = false;
				ShakeTimer = ShakeDuration;
				ShakeSpeed = ShakeSpeed_Max;
			}
		}
		else
		{
			PreShakePosition = Self.position;			
			ShakeTimer = ShakeDuration;
			ShakeSpeed = ShakeSpeed_Max;
			FirstShake = true;
		}
	}

	private void SetOrthoSize()
	{
		int h = Screen.height;// / PixelsToUnits * Zoom;
		//camera.pixelRect = new Rect (0, 0, Screen.width, h);

		if(ZoomIn)
		{
			camera.orthographicSize = (h / (2.0f * PixelsToUnits)) / Zoom;
		}
		else
		{
			camera.orthographicSize = (h / (2.0f / PixelsToUnits)) * Zoom;
		}
		Recalculate = false;
	}
}
