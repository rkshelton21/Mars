using UnityEngine;
using System.Collections;

public class Parallax : MonoBehaviour {

	private float _camInitialX;
	private float _camInitialY;
	private float _initialX;
	private float _initialY;

	public float Offset;
	public bool FollowCamera = false;
	public Transform BackgroundSprite;

	private Transform _spriteA;
	private Transform _spriteB;
	private bool _spriteAIsActive = true;
	private float _spriteWidth;
	public bool Repeat = false;
	public Vector2 StartPoint = new Vector2();
	public Vector2 EndPoint = new Vector2();
	private bool ignoreBounds = false;

	// Use this for initialization
	void Start () 
	{
		_initialX = transform.position.x;
		_initialY = transform.position.y;
		_spriteA = BackgroundSprite;

		if(_spriteA != null)
		{
			_spriteWidth = _spriteA.GetComponent<SpriteRenderer>().bounds.size.x;
		

			if(Repeat)
			{
				var p = _spriteA.position;
				p.x += _spriteWidth;

				var clone = Instantiate(_spriteA, p, _spriteA.rotation);
				_spriteB = (Transform)clone;
				_spriteB.parent = _spriteA.parent;
			}
		}

		_camInitialX = camera.transform.position.x;

		if(StartPoint.x == 0 && EndPoint.x == 0)
		{
			ignoreBounds = true;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		var p = transform.position;
		if(FollowCamera)
		{
			p.x = (camera.transform.position.x - _camInitialX) / Offset + _initialX;
			p.y = (camera.transform.position.y - _camInitialY) / Offset + _initialY;
		}
		else
		{
			p.x = (_camInitialX - camera.transform.position.x) / Offset + _initialX;		
			p.y = (_camInitialY - camera.transform.position.y) / Offset + _initialY;		
		}
		var newPos = p;
		if(!ignoreBounds)
		{
			if(newPos.x < StartPoint.x)
				return;
			if(newPos.x > EndPoint.x)
				return;
		}

		//transform.position = Vector3.Lerp( transform.position, p, Time.deltaTime );
		transform.position = p;

		if(_spriteB != null)
		{
			Transform activesprite = _spriteAIsActive ? _spriteA : _spriteB;
			Transform inactivesprite = _spriteAIsActive ? _spriteB : _spriteA;

			var diff = camera.transform.position.x - activesprite.transform.position.x;

			//if left needs scrolling
			if(diff < -(_spriteWidth / 8))
			{
				//if the sprite hasn't been moved to the left yet
				if(inactivesprite.position.x > activesprite.position.x)
				{
					var spriteP = activesprite.position;
					spriteP.x -= _spriteWidth;
					inactivesprite.position = spriteP;
				}
			}

			//if right needs scrolling
			if(diff > (_spriteWidth / 8))
			{
				//if the sprite hasn't been moved to the right yet
				if(inactivesprite.position.x < activesprite.position.x)
				{
					var spriteP = activesprite.position;
					spriteP.x += _spriteWidth;
					inactivesprite.position = spriteP;
				}
			}

			if(diff < -_spriteWidth / 2)
			{
				_spriteAIsActive = !_spriteAIsActive;
			}
			if(diff > _spriteWidth / 2)
			{
				_spriteAIsActive = !_spriteAIsActive;
			}
		}
	}
}
