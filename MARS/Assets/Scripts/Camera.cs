using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {

	public Transform Target;
	private Transform Self;
	public int PixelsToUnits = 100;
	public int Zoom = 1;
	public bool ZoomIn = true;
	public bool Recalculate = false;

	// Use this for initialization
	void Start () {
		Self = transform;
		SetOrthoSize ();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (Recalculate) 
		{
			SetOrthoSize();
		}

		if (Target != null) {
			//Self.position = new Vector3(Target.position.x, Target.position.y, Self.position.z);
			var newPos = Vector3.Lerp (Self.position, Target.position, 1);
			newPos.z = Self.position.z;
			Self.position = newPos;
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
