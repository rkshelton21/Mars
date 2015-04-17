using UnityEngine;
using System.Collections.Generic;

public class HUD : MonoBehaviour 
{
	public int NumberOfSlots = 2;
	public Transform HudSlot;
	private SpriteRenderer[] _hudSlotSprites;
	private Transform[] _hudSlotObject;
	private float _slotWidth = 0.125f;
	private float CalculatedRatio = 0;
	private Transform _grid;
	private Canvas _equipmentCanvas;
	
	// Use this for initialization
	void Start () 
	{
		_hudSlotSprites = new SpriteRenderer[NumberOfSlots];
		_hudSlotObject = new Transform[NumberOfSlots];
		
		CalculatedRatio = (float)Screen.height / (float)Screen.width;

		var cam = GameObject.Find("Main Camera").GetComponent<Camera>();
		var eqp = transform.FindChild("Equipment");
		_equipmentCanvas = eqp.GetComponent<Canvas>();		
		
		_grid = eqp.FindChild("InventoryGrid");
		
		for(int i=0; i<NumberOfSlots; i++)
		{
			var slot = _grid.FindChild("Slot " + i);
			var item = slot.FindChild("Item");
			
			_hudSlotObject[i] = slot;
			_hudSlotSprites[i] = item.GetComponent<SpriteRenderer>();
		}
	}

	public void Show()
	{
		if(_equipmentCanvas.worldCamera == null)
			_equipmentCanvas.worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
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
		_hudSlotObject[i].gameObject.SetActive(s != null);
	}
}
