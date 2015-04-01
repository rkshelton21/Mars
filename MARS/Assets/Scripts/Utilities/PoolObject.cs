using UnityEngine;
using System.Collections;

public class PoolObject
{
	public GameObject GameObj;
	public Block BlockScript;
	
	public PoolObject(GameObject obj)
	{
		GameObj = obj;
		BlockScript = GameObj.transform.GetComponent<Block>();
	}	
}
