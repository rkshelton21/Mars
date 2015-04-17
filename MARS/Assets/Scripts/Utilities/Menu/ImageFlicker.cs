using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ImageFlicker : MonoBehaviour {
	System.Random _r = new System.Random();
	public float Speed = 0;
	Image _image;
	float _waitTime = 0;
	public bool Random = true;	
	public int MinAlpha = 0;
	public int MaxAlpha = 255;
	bool _on = true;
	// Use this for initialization
	void Start () {
		_image = transform.GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		if(_waitTime <= 0)
		{			
			_waitTime = Speed;
			if(Random)
			{
				var c = _image.color;
				c.a = _r.Next(MinAlpha, MaxAlpha) / 255f;			
				_image.color = c;
			}
			else
			{
				if(!_on)
				{
					var c = _image.color;
					c.a = MaxAlpha / 255f;			
					_image.color = c;
					_on = true;
				}
				else
				{
					var c = _image.color;
					c.a = 0;			
					_image.color = c;
					_on = false;
				}
			}
		}
		else
		{
			_waitTime -= Time.deltaTime;
		}
	}
}
