using UnityEngine;
using System.Collections;

public class RepeateTile : MonoBehaviour {
	public Transform Partner;
	public Transform Target;
	public bool IsForwardTile = false;
	private RepeateTile _partner;
	
	private float _width = 0;
	
	// Use this for initialization
	void Start () {
		_width = GetComponent<SpriteRenderer>().sprite.bounds.size.x;
		_partner = Partner.GetComponent<RepeateTile>();
		if(IsForwardTile)
		{
			transform.position = new Vector3(transform.position.x + _width, transform.position.y, 0.0f);		
		}
	}
		
	// Update is called once per frame
	void Update () 
	{
		if(Target == null)
		{
			return;
		}
		
		if(IsForwardTile)
		{
			if(Target.position.x > transform.position.x + _width)
			{
				transform.position = new Vector3(transform.position.x + 2*_width, transform.position.y, 0.0f);
				_partner.IsForwardTile = false;
				IsForwardTile = true;
			}
			
			/*if(Target.position.x < transform.position.x - 2f*_width)
			{
				transform.position = new Vector3(transform.position.x - 2*_width, transform.position.y, 0.0f);
				_partner.IsForwardTile = true;
				IsForwardTile = false;
			}*/			
			
			if(Target.position.x < transform.position.x - 2f*_width)
			{
				transform.position = new Vector3(transform.position.x - 2*_width, transform.position.y, 0.0f);
				_partner.IsForwardTile = true;
				IsForwardTile = false;
			}
		}
		else
		{
			if(Target.position.x > transform.position.x + _width)
			{
				transform.position = new Vector3(transform.position.x + 2*_width, transform.position.y, 0.0f);
				_partner.IsForwardTile = false;
				IsForwardTile = true;
			}
			
			/*if(Target.position.x < transform.position.x)
			{
				transform.position = new Vector3(transform.position.x + 2*_width, transform.position.y, 0.0f);
				_partner.IsForwardTile = false;
				IsForwardTile = true;
			}*/
		}
	}
}
