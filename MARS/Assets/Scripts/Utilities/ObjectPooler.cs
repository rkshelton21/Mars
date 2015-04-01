using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPooler : MonoBehaviour {

	public static ObjectPooler Current;
	
	public List<GameObject> PrefabObjects = new List<GameObject>();
	private Dictionary<string, List<GameObject>> _ObjectPools = new Dictionary<string, List<GameObject>>();
	private Dictionary<string, GameObject> _ObjectPrefabs = new Dictionary<string, GameObject>();
	public List<int> InitialObjectCounts = new List<int>();
	public int PoolSize = 0;
	public bool GrowOnDemand = false;
	
	void Awake()
	{
		Current = this;
		if(InitialObjectCounts.Count != PrefabObjects.Count)
		{
			Debug.LogError("Object pool counts and objects should match!");
		}
		
		for(int i=0; i < PrefabObjects.Count; i++)
		{
			var list = new List<GameObject>();
			_ObjectPrefabs.Add(PrefabObjects[i].name, PrefabObjects[i]);
			_ObjectPools.Add(PrefabObjects[i].name, list);
			for(int j=0; j < InitialObjectCounts[i]; j++)
			{
				var obj = (GameObject)Instantiate(PrefabObjects[i]);
				obj.transform.SetParent(this.transform);
				obj.SetActive(false);				
				list.Add(obj);
			}
		}
	}
	
	// Use this for initialization
	void Start () 
	{		
	}
	
	public void Initialize(string s, int n, Vector2 pos, object parameters)
	{
		int x = 0;
		var prefabList = _ObjectPools[s];
		for(int i=0; i < prefabList.Count; i++)
		{
			if(!prefabList[i].gameObject.activeInHierarchy)
			{
				var obj = prefabList[i].gameObject;
				obj.transform.position = pos;				
				obj.SetActive(true);				
				prefabList[i].SendMessage("Init", parameters);
				x++;
				
				if(x == n)
				{
					return;
				}
			}					
		}
		
		if(GrowOnDemand && x < n)
		{
			var pre = _ObjectPrefabs[s];
			while(x < n)
			{			
				GameObject obj = (GameObject)Instantiate(pre);
				obj.transform.SetParent(this.transform);
				var po = new PoolObject(obj);
				obj.transform.position = pos;				
				obj.SetActive(true);				
				po.GameObj.SendMessage("Init", parameters);
				prefabList.Add(obj);
				PoolSize++;
				x++;
			}
		}	
	}
}
