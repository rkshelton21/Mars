using UnityEngine;
using System.Collections;

[System.Serializable]
public class InventoryItem 
{
	public string ItemName = "";
	public string ItemType = "Item";
	public Texture ItemTexture;
	public bool Equiped = false;

	private GameObject _gameObj;
	private int _count;
	private string _name;
	private bool _displayOn = false;

	public InventoryItem(GameObject gameObj)
	{
		_gameObj = gameObj;

		if(ItemTexture != null)
		{
			var texture = _gameObj.GetComponentInChildren<GUITexture>();
			texture.texture = ItemTexture;
		}

		Hide();
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
			var text = _gameObj.GetComponentInChildren<GUIText>();
			text.text = value.ToString("D2");

			if(value > 0 && _displayOn)
			{
				Show();
			}
			else
			{
				Hide();
			}
		}
	}

	public void Show()
	{
		_displayOn = true;
		if(_count > 0)
		{
			var text = _gameObj.GetComponentInChildren<GUIText>();
			var texture = _gameObj.GetComponentInChildren<GUITexture>();
			if(texture != null)
			{
				texture.enabled = true;
				text.enabled = true;
			}
		}
	}

	public void Hide()
	{
		_displayOn = false;
		if(_gameObj != null)
		{
			var text = _gameObj.GetComponentInChildren<GUIText>();
			var texture = _gameObj.GetComponentInChildren<GUITexture>();
			if(texture != null)
			{
				texture.enabled = false;
				text.enabled = false;
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
