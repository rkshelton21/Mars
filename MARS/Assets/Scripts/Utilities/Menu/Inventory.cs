using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

	public int Rows = 3;
	public int Columns = 8;

	public float FrameWidth = 0f;
	public float FrameHeight = 0f;

	public float WidthPercentageOfGrid = 1.0f;
	public float HeightPercentageOfGrid = 0.5f;
	public bool UpdatePositions = false;

	public Transform InventorySlot;
	private bool _inventoryShown = false;

	private Dictionary<string, InventoryItem> _ItemSlots = new Dictionary<string, InventoryItem>();
	public List<InventoryItem> AvailableItems = new List<InventoryItem>();
	public List<BulletType> AmmoStock = new List<BulletType>();

	public InventoryItem _primaryWeapon = null;
	public InventoryItem _secondaryWeapon = null;

	private HUD _Hud;

	// Use this for initialization
	void Start () 
	{
		_Hud = GameObject.Find("HUD").GetComponent<HUD>();	

		CalculatePositions();
		Hide ();
	}

	public void Toggle()
	{
		//Debug.Log(_inventoryShown);
		if(_inventoryShown)
		{
			Hide ();
		}
		else
		{
			Show ();
		}
	}

	private void Show()
	{
		var textures = GetComponentsInChildren<GUITexture>();
		foreach(var t in textures)
		{
			t.enabled = true;
		}
		_inventoryShown = true;

		ShowAllItems();
	}

	private void Hide()
	{
		var textures = GetComponentsInChildren<GUITexture>();
		foreach(var t in textures)
		{
			t.enabled = false;
		}
		_inventoryShown = false;

		HideAllItems();
	}

	//public void SetItemImage(string name, Texture newTexture)
	//{
	//	Items[name].ItemImage = newTexture;
	//}

	public void SetItemCount(string name, int count)
	{
		var itemMatch = _ItemSlots[name];
		if(itemMatch == null)
		{
			Debug.LogError("Item not supported in inventory.");
		}
		else
		{
			itemMatch.ItemCount = count;
		}
	}

	public void AddItem(string name)
	{
		if(!_ItemSlots.ContainsKey(name))
		{
			Debug.LogError("Item (" + name +  ") not supported in inventory.");
		}

		var itemMatch = _ItemSlots[name];
		itemMatch.ItemCount++;



		//Debug.Log("Name:" + _primaryWeapon.ItemName + ":" + name + ":" + itemMatch.ItemName);
	}

	public bool SwapWeapons()
	{
		if(_primaryWeapon == null || _secondaryWeapon == null)
			return false;

		if(_primaryWeapon.Equiped == false || _secondaryWeapon.Equiped == false)
			return false;

		var temp = _primaryWeapon;
		_primaryWeapon = _secondaryWeapon;
		_secondaryWeapon = temp;

		_Hud.Set(0, _primaryWeapon.ItemTexture);
		_Hud.Set(1, _secondaryWeapon.ItemTexture);

		return true;
	}

	public void Equip(int slot, string oldItem, string newItem)
	{
		if(!string.IsNullOrEmpty(oldItem))
		{
			if(!_ItemSlots.ContainsKey(oldItem))
			{
				Debug.LogError("Item (" + oldItem +  ") not supported in inventory.");
			}
			var oldItemMatch = _ItemSlots[oldItem];
			oldItemMatch.Equiped = false;
		}

		if(!_ItemSlots.ContainsKey(newItem))
		{
			Debug.LogError("Item (" + newItem +  ") not supported in inventory.");
		}

		var newItemMatch = _ItemSlots[newItem];
		newItemMatch.Equiped = true;

		if(slot == 0)
		{
			_primaryWeapon = newItemMatch;
		}
		else if(slot == 1)
		{
			_secondaryWeapon = newItemMatch;
		}
		else
		{
			Debug.LogError("Not enough slots");
		}

		_Hud.Set(slot, newItemMatch.ItemTexture);
		Debug.Log(newItem + " Equipped in slot " + slot);
	}

	public Texture GetTexture(string name)
	{
		foreach(var b in AvailableItems)
		{
			if(b.ItemName == name)
			{
				return b.ItemTexture;
			}
		}
		return null;
	}

	public void GetAmmo(string name, out Transform ammoTransform, out Bullet ammoClass)
	{
		ammoTransform = null;
		ammoClass = null;

		foreach(var b in AmmoStock)
		{
			if(b.Name == name)
			{
				ammoTransform = b.Bullet;
				ammoClass = b.Bullet.GetComponent<Bullet>();
				return;
			}
		}
		return;
	}

	private void CalculatePositions()
	{
		DestroyAllItems();

		int i = 0;
		//Set Text Position
		float a = WidthPercentageOfGrid;
		float b = HeightPercentageOfGrid;
		
		float x = a/(Columns*2f);
		float y = b/(Rows*2f);

		var itemCounter = InventorySlot.FindChild("ItemCount");
		itemCounter.localPosition = new Vector3(a/(Columns), -b/(Rows), itemCounter.position.z);
		var itemCounterGui = itemCounter.GetComponent<GUIText>();
		itemCounterGui.text = i.ToString("D2");

		//x = x + a/(Columns) * c;
		//y = y + b/(Rows) * r;
		//y = 1f - y;

		/*
		for(int c = 0; c < Columns; c++)
		{
			for(int r = 0; r < Rows; r++)
			{
				x = a/(Columns*2f) + a/(Columns) * c;
				y = b/(Rows*2f) + b/(Rows) * r;
				y = 1f - y;

				var p = new Vector3(x, y, 1);
				
				var newItemSlot = Instantiate(InventorySlot, p, Quaternion.identity);
				var newItemTransform = (Transform)newItemSlot;
				//newItemTransform.localScale = new Vector3(scaleX, scaleY, 1.0f);

				newItemTransform.parent = transform;
				newItemTransform.localPosition = p;


				Items.Add(new InventoryItem(newItemTransform.gameObject));			
				i++;
			}
		}
		*/

		foreach(var item in AvailableItems)
		{		
			int r = i / Columns;
			int c = i - r;
					
			x = a/(Columns*2f) + a/(Columns) * c;
			y = b/(Rows*2f) + b/(Rows) * r;
			y = 1f - y;
			
			var p = new Vector3(x, y, 1);
			
			var newItemSlot = Instantiate(InventorySlot, p, Quaternion.identity);
			var newItemTransform = (Transform)newItemSlot;
			//newItemTransform.localScale = new Vector3(scaleX, scaleY, 1.0f);
			
			newItemTransform.parent = transform;
			newItemTransform.localPosition = p;
			
			newItemSlot.name = item.ItemName;
			var itemCopy = new InventoryItem(newItemTransform.gameObject);
			itemCopy.ItemName = item.ItemName;
			itemCopy.ItemTexture = item.ItemTexture;
			itemCopy.ItemType = item.ItemType;

			var itemTexture = ((Transform)newItemSlot).GetComponentInChildren<GUITexture>();
			itemTexture.texture = itemCopy.ItemTexture;

			_ItemSlots.Add(item.ItemName, itemCopy);

			i++;
		}
	}

	private void DestroyAllItems()
	{
		if(_ItemSlots != null)
		{
			foreach(var item in _ItemSlots.Values)
			{
				item.Destroy();
			}
		}

		_ItemSlots.Clear();
	}

	private void ShowAllItems()
	{
		if(_ItemSlots != null)
		{
			foreach(var item in _ItemSlots.Values)
			{
				item.Show();
			}
		}
	}

	private void HideAllItems()
	{
		if(_ItemSlots != null)
		{
			foreach(var item in _ItemSlots.Values)
			{
				item.Hide();
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(UpdatePositions)
		{
			CalculatePositions();
			UpdatePositions = false;
		}
	}
}
