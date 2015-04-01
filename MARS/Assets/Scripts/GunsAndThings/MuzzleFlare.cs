using UnityEngine;
using System.Collections;

public class MuzzleFlare : MonoBehaviour {
	public Sprite UpMuzzleFlash;
	public Sprite UpwardMuzzleFlash;
	public Sprite NormalMuzzleFlash;
	public Sprite DownwardMuzzleFlash;
	public Sprite DownMuzzleFlash;
	
	public Transform UpMuzzleFlashPosition;
	public Transform UpwardMuzzleFlashPosition;
	public Transform NormalMuzzleFlashPosition;
	public Transform DownwardMuzzleFlashPosition;
	public Transform DownMuzzleFlashPosition;
	
	private SpriteRenderer _sprite;
	private float _displayTimer;
	public float _maxDisplayTimer = 0.1f;

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
	
	void Start()
	{
		_FlashPositions[0] = UpMuzzleFlashPosition.localPosition;
		_FlashPositions[1] = UpwardMuzzleFlashPosition.localPosition;
		_FlashPositions[2] = NormalMuzzleFlashPosition.localPosition;
		_FlashPositions[3] = DownwardMuzzleFlashPosition.localPosition;
		_FlashPositions[4] = DownMuzzleFlashPosition.localPosition;
		
		_sprite = GetComponent<SpriteRenderer>();
		_displayTimer = 0;
		_sprite.enabled = false;
	}

	public void Fire(Vector2 direction)
	{
		bool moveX = Mathf.Abs(direction.x) > 0.01f;
		bool upY = direction.y > 0.01f;
		bool downY = direction.y < -0.01f;

		//Up
		if(!moveX && upY)
		{
			_sprite.sprite = UpMuzzleFlash;
			transform.localPosition = UpMuzzleFlashPosition.localPosition;
		}
		//Upward
		else if(moveX && upY)
		{
			_sprite.sprite = UpwardMuzzleFlash;
			transform.localPosition = UpwardMuzzleFlashPosition.localPosition;
		}
		//Normal
		else if(moveX && !upY && !downY)
		{
			_sprite.sprite = NormalMuzzleFlash;
			transform.localPosition = NormalMuzzleFlashPosition.localPosition;
		}
		//Downward
		else if(moveX && downY)
		{
			_sprite.sprite = DownwardMuzzleFlash;
			transform.localPosition = DownwardMuzzleFlashPosition.localPosition;
		}
		//Down
		else//(!moveX && downY)
		{
			_sprite.sprite = DownMuzzleFlash;
			transform.localPosition = DownMuzzleFlashPosition.localPosition;
		}

		_displayTimer = _maxDisplayTimer;
		_sprite.enabled = true;
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
