using UnityEngine;
using System.Collections;

public class MuzzleFlare : MonoBehaviour {
	public Sprite UpMuzzleFlash;
	public Sprite UpwardMuzzleFlash;
	public Sprite NormalMuzzleFlash;
	public Sprite DownwardMuzzleFlash;
	public Sprite DownMuzzleFlash;

	private SpriteRenderer _sprite;
	private float _displayTimer;
	private float _maxDisplayTimer = 0.1f;

	private Vector3[] _FlashPositions = new Vector3[5]{
		//UP
		new Vector3(0.08796406f, 0.3709102f, 0f),
		//UPWARD
		new Vector3(0.1940975f, 0.2913104f, 0f),
		//NORMAL
		new Vector3(0.3134972f, -0.08678728f, 0f),
		//DOWNWARD
		new Vector3(0.1410313f, -0.2526218f, 0f),
		//DOWN
		new Vector3(0.008365631f, -0.5312206f, 0f)
	};

	public void Fire(Vector2 direction)
	{
		bool moveX = Mathf.Abs(direction.x) > 0.01f;
		bool upY = direction.y > 0.01f;
		bool downY = direction.y < -0.01f;

		//Up
		if(!moveX && upY)
		{
			_sprite.sprite = UpMuzzleFlash;
			transform.localPosition = _FlashPositions[0];
		}
		//Upward
		else if(moveX && upY)
		{
			_sprite.sprite = UpwardMuzzleFlash;
			transform.localPosition = _FlashPositions[1];
		}
		//Normal
		else if(moveX && !upY && !downY)
		{
			_sprite.sprite = NormalMuzzleFlash;
			transform.localPosition = _FlashPositions[2];
		}
		//Downward
		else if(moveX && downY)
		{
			_sprite.sprite = DownwardMuzzleFlash;
			transform.localPosition = _FlashPositions[3];
		}
		//Down
		else//(!moveX && downY)
		{
			_sprite.sprite = DownMuzzleFlash;
			transform.localPosition = _FlashPositions[4];
		}

		_displayTimer = _maxDisplayTimer;
		_sprite.enabled = true;
	}

	// Use this for initialization
	void Start () 
	{
		_sprite = GetComponent<SpriteRenderer>();
		_displayTimer = 0;
		_sprite.enabled = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		_displayTimer -= Time.deltaTime;
		if(_displayTimer <= 0)
		{
			_sprite.enabled = false;
		}
	}
}
