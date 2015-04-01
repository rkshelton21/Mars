using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Fade : MonoBehaviour {
	public float Speed = 0;
	public float Increment = 0;
	float _waitTime = 0;
	Image _image;
	bool fadingOut = true;
	
	// Use this for initialization
	void Start () {
		_image = transform.GetComponent<Image>();			
	}
	
	// Update is called once per frame
	void Update () {
		if(_waitTime <= 0)
		{
			_waitTime = Speed;
			var c = _image.color;
			if(fadingOut)
			{
				c.a -= Increment;
				if(c.a <= 0f)
				{
					fadingOut = false;
				}
			}
			else
			{
				c.a += Increment;
				if(c.a >= 1f)
				{
					fadingOut = true;
				}
			}
			_image.color = c;
		}
		else
		{
			_waitTime -= Time.deltaTime;
		}
	}
}
