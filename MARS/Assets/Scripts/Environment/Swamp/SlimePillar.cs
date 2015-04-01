using UnityEngine;
using System.Collections;

public class SlimePillar : MonoBehaviour {

	public float FallRate = 10f;
	public float RiseRate = 10f;
	public float RiseDistance = 0;
	public float HoldTime = 1f;

	private float _originalRiseDistance = 0;
	private float _originalPosition = 0;
	private bool _onTheRise = true;
	private int _id;

	// Use this for initialization
	void Start () 
	{
		_originalPosition = transform.position.y;
		_originalRiseDistance = RiseDistance;
		_id = GetInstanceID ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(_originalRiseDistance != RiseDistance)
		{
			_originalRiseDistance = RiseDistance;
			var returnPosition = transform.position;
			returnPosition.y = _originalPosition;
			transform.position = returnPosition;
		}

		if (transform.position.y < _originalPosition + RiseDistance && _onTheRise) 
		{
			var newY = Mathf.Lerp (transform.position.y, _originalPosition + RiseDistance, Time.deltaTime * RiseRate);
			var newPos = transform.position;
			newPos.y = newY;
			transform.position = newPos;

			var diff = Mathf.Abs(transform.position.y - (_originalPosition + RiseDistance));
			if(diff < 0.01)
				_onTheRise = false;
		} 
		else 
		{
			HoldTime -= Time.deltaTime;
			_onTheRise = false;

			if(HoldTime < 0)
			{
				if (transform.position.y > _originalPosition) 
				{
					var newY = Mathf.Lerp (transform.position.y, _originalPosition, Time.deltaTime * RiseRate);
					var newPos = transform.position;
					newPos.y = newY;
					transform.position = newPos;

					var diff = Mathf.Abs(transform.position.y - _originalPosition);
					if(diff < 0.01)
						Destroy(gameObject);
				}
				else
				{
					Destroy(gameObject);
				}
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Player") 
		{
			var rigid = collider.transform.GetComponentInParent<Rigidbody2D>();
			rigid.AddForce(new Vector2(0.0f, 100.0f));

			collider.transform.parent.gameObject.SendMessage("ApplyDamage", new DamageDescription(){
				AttackDamage = 0.5f,
				AttackDirectionIsRight = true,
				AttackerId = _id,
				AttackForce = new Vector2(0, 200)
			});
		}
	}
}
