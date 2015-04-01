using UnityEngine;
using System.Collections.Generic;

public class HUD : MonoBehaviour 
{
	public int NumberOfSlots = 2;
	public Transform HudSlot;
	private SpriteRenderer[] _hudSlotSprites;
	private Transform[] _itemTransforms;
	private float _slotWidth = 0.125f;
	private float CalculatedRatio = 0;

	// Use this for initialization
	void Start () 
	{
		_hudSlotSprites = new SpriteRenderer[NumberOfSlots];
		_itemTransforms = new Transform[NumberOfSlots];

		CalculatedRatio = (float)Screen.height / (float)Screen.width;

		var x = 0.0625f;
		var y = 0.0625f;
		for(int i=0; i<NumberOfSlots; i++)
		{
			var p = new Vector3(x, y, 1);
			
			var newHudSlot = Instantiate(HudSlot, p, Quaternion.identity);
			var newHudTransform = (Transform)newHudSlot;
			var item = newHudTransform.FindChild("ItemSlot");

			var scale = item.transform.localScale;
			scale.x = scale.x * CalculatedRatio;
			item.transform.localScale = scale;

			var itemRenderer = item.GetComponentInChildren<SpriteRenderer>();
			newHudTransform.parent = transform;
			_itemTransforms[i] = item.transform;
			_hudSlotSprites[i] = itemRenderer;
			x += _slotWidth;
		}
	}

	public void Resize()
	{
		CalculatedRatio = (float)Screen.height / (float)Screen.width;
		for(int i=0; i<NumberOfSlots; i++)
		{
			var item = _itemTransforms[i];
			var scale = item.localScale;
			scale.x = scale.x * CalculatedRatio;
			item.localScale = scale;
		}
	}
	
	public void Show()
	{
		transform.gameObject.SetActive(true);
	}
	
	public void Hide()
	{
		transform.gameObject.SetActive(false);
	}
	
	public void Set(int i, Sprite s)
	{
		if(i < 0 || i >= NumberOfSlots)
			return;

		_hudSlotSprites[i].sprite = s;
	}

	// Update is called once per frame
	void Update () 
	{
		var currentRatio = (float)Screen.height / (float)Screen.width;
		if(currentRatio != CalculatedRatio)
		{
			Resize();
		}
	}
}
