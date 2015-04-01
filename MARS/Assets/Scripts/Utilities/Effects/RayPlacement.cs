using UnityEngine;
using System.Collections;

public class RayPlacement : MonoBehaviour {
	public LayerMask rayLayer;
	public float MaxRange = 10f;
	public Vector2 RayDirection = new Vector2(0f, -1f);
	private Transform _parent;
	public float Debug;
	public float ShadowDistance = 0.2f;
	
	// Use this for initialization
	void Start () {
		_parent = transform.parent.transform;	
	}
	
	// Update is called once per frame
	void Update () 
	{
		var hit = Physics2D.Raycast(_parent.position, RayDirection, MaxRange, rayLayer);
		if(hit.collider != null)
		{
			transform.position = hit.point;
			var r = Quaternion.identity;
			var n = hit.normal;
			var d = hit.distance - ShadowDistance;
			n.x *= _parent.localScale.x;
			var s = Mathf.Clamp(1f - d, 0f, 1f);
			transform.localScale = new Vector2(s, 1f);
			r.SetFromToRotation(Vector2.up, n);
			transform.rotation = r;
			
			Debug = 1f - d;
		}
		else
		{
			transform.localScale = new Vector2(0f, 1f);
		}
	}
}
