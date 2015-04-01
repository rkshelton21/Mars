using UnityEngine;
using System.Collections;

[System.Serializable]
public class InventoryItem 
{
	public string ItemName = "";
	public string ItemType = "Item";
	public Sprite ItemSprite;
	public bool Equiped = false;

	private GameObject _gameObj;
	private int _count;
	private string _name;
	private bool _displayOn = false;
	
	public InventoryItem(GameObject gameObj)
	{
		_gameObj = gameObj;

		if(ItemSprite == null)
		{
			var i = _gameObj.GetComponent<SpriteRenderer>();
			ItemSprite = i.sprite;
		}
		
		if(ItemType.Contains("Gun"))
		{
			ItemType = "Weapon";
		}
		
		ItemName = gameObj.name;
		ItemName = ItemName.Replace("Consumable_", "");
		
		//Hide();
	}

	public int ItemCount
	{
		get
		{
			return _count;
		}
		set
		{
			_count = value;
			//var text = _gameObj.GetComponentInChildren<GUIText>();
			//text.text = value.ToString("D2");

			if(value > 0 && _displayOn)
			{
				//Show();
			}
			else
			{
				//Hide();
			}
		}
	}

	public void Destroy()
	{
		UnityEngine.GameObject.Destroy(_gameObj);

		//_name = "";
		_count = 0;
		//_texture = null;
	}
}
