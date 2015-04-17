using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PlayerData : MonoBehaviour {
	private static PlayerData _data;
	private static PlayerInfo _info;
	
	// Use this for initialization
	void Awake () {
		if(_data == null)
		{
			_data = this;
			DontDestroyOnLoad(gameObject);		
		}
		else if(_data != this)
		{
			Destroy(gameObject);
		}		
	}
	
	public static void SaveGame()
	{
		_data.Save();
	}
	
	public static void NewGame()
	{
		
		_data.SaveNew();
	}
	
	public static void LoadGame()
	{
		_data.Load();
	}
	
	
	public void SaveNew()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = new FileStream(Application.persistentDataPath + "/playerInfo.dat", FileMode.Create);
		
		bf.Serialize(file, new PlayerInfo());
		file.Close();
	}
	
	public void Save()
	{
		GetPlayerData();
		
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = new FileStream(Application.persistentDataPath + "/playerInfo.dat", FileMode.Create);
		
		bf.Serialize(file, _info);
		file.Close();
	}
	
	public void Load()
	{
		if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
		{
			Debug.Log("Loading");
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = new FileStream(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
			_info = (PlayerInfo)bf.Deserialize(file);
			file.Close();
			
			SetPlayerData(_info);
		}
	}
	
	public void SetPlayerData(PlayerInfo info)
	{
		Debug.Log("Setting");
		var p = GameObject.Find("Player").GetComponent<PlayerContoller2D>();
		p.SetInfo(_info);
		
		var inv = GameObject.Find ("Inventory").GetComponent<Inventory>();
		
		if(info.items != null)
		{
			for(int i=0; i<info.items.Length; i++)
			{
				Debug.Log("Item: " + info.items[i] + ": " + info.itemCounts[i]);
				inv.AddItem(info.items[i]);
				inv.SetItemCount(info.items[i], info.itemCounts[i]);
			}
		}		
	}
	
	public void GetPlayerData()
	{
		var p = GameObject.Find("Player").GetComponent<PlayerContoller2D>();
		_info = p.GetInfo();
			
		var inv = GameObject.Find ("Inventory").GetComponent<Inventory>();		
		var items = inv.GetItems();
		
		_info.items = new string[items.Count];
		_info.itemCounts = new int[items.Count];
		int i = 0;
		foreach(var item in items)
		{
			_info.items[i] = item.Key;
			_info.itemCounts[i] = item.Value;
			i++;
		}
		
		if(inv._primaryWeapon != null)
			_info.PrimaryWeapon = inv._primaryWeapon.ItemName;
		if(inv._secondaryWeapon != null)
			_info.SecondaryWeapon  = inv._secondaryWeapon.ItemName;
	}
}

[System.Serializable]
public class PlayerInfo
{
	public float Health = 1.0f;
	public bool HasWeapon = false;
	public string PrimaryWeapon = "";
	public string SecondaryWeapon = "";
	
	public string[] items;
	public int[] itemCounts;
}