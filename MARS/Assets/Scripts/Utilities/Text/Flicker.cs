using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Flicker : MonoBehaviour {
	System.Random _r = new System.Random();
	public float Speed = 0;
	Text _myText;
	float _waitTime = 0;
	public bool Random = true;	
	public int MaxAlpha = 70;
	bool _on = true;
	// Use this for initialization
	void Start () {
		_myText = transform.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		if(_waitTime <= 0)
		{			
			_waitTime = Speed;
			if(Random)
			{
				var c = _myText.color;
				c.a = _r.Next(MaxAlpha) / 255f;			
				_myText.color = c;
			}
			else
			{
				if(!_on)
				{
					var c = _myText.color;
					c.a = MaxAlpha / 255f;			
					_myText.color = c;
					_on = true;
				}
				else
				{
					var c = _myText.color;
					c.a = 0;			
					_myText.color = c;
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
