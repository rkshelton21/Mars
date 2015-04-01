using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InventoryItemSlotScript : MonoBehaviour {
	private Image _mySpriteRenderer;
	private bool _enabled = true;
	private InventoryItem Item;
	
	public InventoryItem GetItem()
	{
		return Item;
	}
	
	public void SetItem(InventoryItem item)
	{
		Item = item;
		SetSprite(Item.ItemSprite);
	}
	
	public void SetSprite(Sprite newSprite)
	{
		if(_mySpriteRenderer == null)
		{
			_mySpriteRenderer = transform.GetComponent<Image>();
		}
		_mySpriteRenderer.sprite = newSprite;
	
	}
	
	public Sprite GetSprite()
	{
		return Item.ItemSprite;		
	}
	
	public void SetCount(int n)
	{
		if(n > 0 && !_enabled)
		{
			gameObject.SetActive(true);
			_enabled = true;
		}
		
		if(n <= 0 && _enabled)
		{
			gameObject.SetActive(false);
			_enabled = false;
		}
		
		Item.ItemCount = n;
	}	
	
	public void IncrementCount()
	{
		if(!_enabled)
		{
			gameObject.SetActive(true);
			_enabled = true;
		}
		
		Item.ItemCount++;		
	}	
}
