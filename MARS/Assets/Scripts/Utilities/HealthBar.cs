using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HealthBar : MonoBehaviour {
	//public float Health = 1.0f;

	public List<Sprite> Sprites;
	public Transform Bar;

	private int _maxSpriteIndex = 0;
	private SpriteRenderer _barRenderer;
	// Use this for initialization
	void Start () {

		_barRenderer = Bar.GetComponent<SpriteRenderer>();

		if(Sprites != null)
		{
			_maxSpriteIndex = Sprites.Count - 1;
			_barRenderer.sprite = Sprites[_maxSpriteIndex];
		}
	}

	//void Update(){
	//	SetHealth(Health);
	//}

	public void SetHealth(float percentage)
	{
		var index = (int)(_maxSpriteIndex * percentage);
		if(index < 0)
		{
			index = 0;
		}
		if(index > _maxSpriteIndex)
		{
			index = _maxSpriteIndex;
		}

		_barRenderer.sprite = Sprites[index];
	}
}
