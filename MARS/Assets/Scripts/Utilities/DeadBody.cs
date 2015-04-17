using UnityEngine;
using System.Collections;

public class DeadBody : MonoBehaviour {
	private SpriteRenderer _myRenderer;
	private float _imageLockCountDown = 0;
	private bool _lockActivated = false;
	private SpriteRenderer _otherRenderer;
	
	void Start()
	{
		_myRenderer = transform.GetComponent<SpriteRenderer>();	
		_myRenderer.enabled = false;
	}
	
	void FixedUpdate()
	{
		if(_lockActivated)
		{
			_imageLockCountDown -= Time.deltaTime;
			
			if(_imageLockCountDown <= 0)
			{
				_myRenderer.sprite = _otherRenderer.sprite;
				this.enabled = false;
				transform.position = _otherRenderer.transform.position;
				Destroy(_otherRenderer.gameObject);
				_myRenderer.enabled = true;
			}
		}
	}
	
	public void Init(DeadBodyParams data)
	{
		_imageLockCountDown = data.Countdown;
		_lockActivated = true;
		_otherRenderer = data.BodyRenderer;
	}
}
