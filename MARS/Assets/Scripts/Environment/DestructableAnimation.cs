using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DestructableAnimation : MonoBehaviour {
	public List<Sprite> SpriteList;
	public bool Loop = false;
	public float SpriteDuration = 1.0f;
	private int _spriteIndex = 0;
	private float _timeTillNextSprite;
	private SpriteRenderer _renderer;
	public float StartDelay = 0.0f;
	
	public virtual void CycleComplete(){
	}
	
	public virtual void Init(){
	}
	
	// Use this for initialization
	void Start () {
		_timeTillNextSprite = SpriteDuration + StartDelay;	
		_renderer = this.GetComponent<SpriteRenderer> ();
		if (_renderer == null)
			Debug.LogError ("Could not find sprite renderer to animate.");
		
		Init ();
	}
	
	// Update is called once per frame
	void Update () {
		_timeTillNextSprite -= Time.deltaTime;
		if (_timeTillNextSprite <= 0) 
		{
			_timeTillNextSprite = SpriteDuration;
			_spriteIndex++;
			
			if(_spriteIndex >= SpriteList.Count)
			{
				if(!Loop)
				{
					DestroyObject(gameObject);
					return;
				}
				else
				{
					_spriteIndex = 0;
				}
				CycleComplete();
			}
			_renderer.sprite = SpriteList[_spriteIndex];
		}
	}
}
