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
	
	public void Save()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = new FileStream(Application.persistentDataPath + "/playerInfo.dat", FileMode.Create);
		
		bf.Serialize(file, _info);
		file.Close();
	}
	
	public void Load()
	{
		if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = new FileStream(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
			_info = (PlayerInfo)bf.Deserialize(file);
			file.Close();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

[System.Serializable]
class PlayerInfo
{
	public float Health;
	public bool HasWeapon = false;
}