using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

	private Transform _inventoryGrid;
	public Transform InventorySlotTemplate;
	public List<Transform> SupportedItems = new List<Transform>();
	public List<Transform> SupportedAmmo = new List<Transform>();
	private bool _inventoryShown = false;

	private Dictionary<string, InventoryItemSlotScript> _ItemSlotScripts = new Dictionary<string, InventoryItemSlotScript>();
	private List<BulletType> AmmoStock = new List<BulletType>();

	public InventoryItem _primaryWeapon = null;
	public InventoryItem _secondaryWeapon = null;
	private PlayerContoller2D _player = null;
	private HUD _Hud;

	// Use this for initialization
	void Start () 
	{
		_Hud = GameObject.Find("HUD").GetComponent<HUD>();	
		_inventoryGrid = transform.FindChild("InventoryGrid");		
		GetPlayer();
		
		Hide ();
		
		Clear ();
	}
	
	private PlayerContoller2D GetPlayer()
	{
		if(_player == null)
		{
			_player = GameObject.Find("Player").GetComponent<PlayerContoller2D>();
		}
		return _player;
	}

	public void Toggle()
	{
		var c = transform.GetChild(0);
		c.gameObject.SetActive(!c.gameObject.activeInHierarchy); 
	}

	public void Show()
	{
		var c = transform.GetChild(0);
		c.gameObject.SetActive(true); 
	}
	
	private void UpdateSlots()
	{
		
	}
	
	public void Hide()
	{
		var c = transform.GetChild(0);
		c.gameObject.SetActive(false); 
	}
	
	public void Clear()
	{
		_primaryWeapon = null;
		_secondaryWeapon = null;
		
		_Hud.Set(0, null);
		_Hud.Set(1, null);
		
		foreach(var item in SupportedItems)
		{
			var name = item.name.Replace("_Bullet", "");
			name = name.Replace("Consumable_", "");
			
			if(!_ItemSlotScripts.ContainsKey(name))
			{
				var slotObj = (Transform)Instantiate(InventorySlotTemplate, new Vector3(), Quaternion.identity);
				var script = slotObj.GetComponent<InventoryItemSlotScript>();
				slotObj.SetParent(_inventoryGrid);			
			
				script.SetItem(new InventoryItem(item.gameObject));			
				script.SetCount(0);
				
				_ItemSlotScripts.Add(name, script);
			}
			else
			{
				var script = _ItemSlotScripts[name];
				script.SetItem(new InventoryItem(item.gameObject));			
				script.SetCount(0);
			}			
		}
		
		foreach(var item in SupportedAmmo)
		{
			var name = item.name.Replace("_Bullet", "");
			name = name.Replace("Consumable_", "");
			AmmoStock.Add(new BulletType(){ Name = name, Bullet = item });			
		}
	}
	
	public void SetItemCount(string name, int count)
	{
		var itemMatch = _ItemSlotScripts[name];
		if(itemMatch == null)
		{
			Debug.LogError("Item not supported in inventory.");
		}
		else
		{
			if(_primaryWeapon != null && _primaryWeapon.ItemName == name && count == 0)
			{
				Debug.Log("Cleared primary");
				_Hud.Set(0, null);
			}
			
			if(_secondaryWeapon != null && _secondaryWeapon.ItemName == name && count == 0)
			{
				_Hud.Set(1, null);
			}
			
			itemMatch.SetCount(count);
		}
	}

	public void AddItem(string name)
	{
		if(!_ItemSlotScripts.ContainsKey(name))
		{
			Debug.LogError("Item (" + name +  ") not supported in inventory.");
		}

		if(name.Contains("Gun") && (_primaryWeapon == null || _primaryWeapon.ItemName == ""))
		{
			Debug.Log("A");
			Equip(0, string.Empty, name);
			foreach(var b in AmmoStock)
			{
				if(b.Name == name)
				{
					GetPlayer().SetPrimaryAmmo(b.Bullet.GetComponent<Bullet>());					
				}
			}
						
		}
		
		var itemMatch = _ItemSlotScripts[name];
		itemMatch.IncrementCount();

		//Debug.Log("Name:" + _primaryWeapon.ItemName + ":" + name + ":" + itemMatch.ItemName);
	}
	
	public List<KeyValuePair<string, int>> GetItems()
	{
		var items = new List<KeyValuePair<string, int>>();
		var i = 0;
		foreach(var slot in _ItemSlotScripts)
		{
			items.Add(new KeyValuePair<string, int>(slot.Key, slot.Value.GetItem().ItemCount));
		}
		return items;		
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

		_Hud.Set(0, _primaryWeapon.ItemSprite);
		_Hud.Set(1, _secondaryWeapon.ItemSprite);

		return true;
	}

	public void Equip(int slot, string oldItem, string newItem)
	{
		if(!string.IsNullOrEmpty(oldItem))
		{
			if(!_ItemSlotScripts.ContainsKey(oldItem))
			{
				Debug.LogError("Item (" + oldItem +  ") not supported in inventory.");				
			}
			var oldItemMatch = _ItemSlotScripts[oldItem];
			oldItemMatch.GetItem().Equiped = false;
		}

		if(!_ItemSlotScripts.ContainsKey(newItem))
		{
			Debug.LogError("Item (" + newItem +  ") not supported in inventory.");
		}

		var newItemMatch = _ItemSlotScripts[newItem];
		newItemMatch.GetItem().Equiped = true;

		if(slot == 0)
		{
			_primaryWeapon = newItemMatch.GetItem();
		}
		else if(slot == 1)
		{
			_secondaryWeapon = newItemMatch.GetItem();
		}
		else
		{
			Debug.LogError("Not enough slots");
		}

		_Hud.Set(slot, newItemMatch.GetSprite());
		Debug.Log(newItem + " Equipped in slot " + slot);
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
		
		Debug.Log("Could not Find Ammo");		
		return;
	}

	private void DestroyAllItems()
	{
		if(_ItemSlotScripts != null)
		{
			foreach(var item in _ItemSlotScripts.Values)
			{
				//item.Item.Destroy();
			}
		}

		_ItemSlotScripts.Clear();
	}

	private void ShowAllItems()
	{
		if(_ItemSlotScripts != null)
		{
			foreach(var item in _ItemSlotScripts.Values)
			{
				//item.Show();
			}
		}
	}

	private void HideAllItems()
	{
		if(_ItemSlotScripts != null)
		{
			foreach(var item in _ItemSlotScripts.Values)
			{
				//item.Hide();
			}
		}
	}	
}
