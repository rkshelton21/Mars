using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;
using System.Xml;
using System.Text;

[CustomEditor(typeof(LevelSetup))]
public class LevelImporter  : Editor
{
	private Dictionary<string, Transform> _supportedObjects;
	
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector ();
		//base.OnInspectorGUI();
		
		if (GUILayout.Button ("Fix Level")) 
		{
			FixLevel();		
		}
		
		if (GUILayout.Button ("Import Objects")) 
		{
			_supportedObjects = new Dictionary<string, Transform>();
			 var ls = (LevelSetup)target;
			 foreach(var item in ls.PrefabDatabase)
			 {
			 	if(item != null)
			 	{
			 		_supportedObjects.Add(item.name, item);
		 		}
			 }
			 
			Import();		
		}		
	}
	
	private void Import()
	{
		var sb = new StringBuilder();
		var basePath = "D:\\CApps (x86)\\Tiled2Unity\\Levels\\";
		var levelName = "Level03";
		var extension = ".tmx";
		
		var addedTypes = new Dictionary<string, GameObject>();
		var levelObjects = new GameObject("LevelObjects");
		var xReader = XmlReader.Create(basePath + levelName + extension);
		while (xReader.Read())
		{
			switch (xReader.NodeType)
			{
			case XmlNodeType.Element:
				//sb.AppendLine("<" + xReader.Name + ">");
				if(xReader.Name == "object")
				{
					sb.AppendLine(xReader.Name);
					var type = "";
					var name = "";
					Vector2 pos = new Vector2();
					bool flippedX = false;
					bool flippedY = false;
					
					while (xReader.MoveToNextAttribute()) 
					{
						//Debug.Log(xReader.Name + ": " + xReader.Value);
						switch(xReader.Name.ToLower())
						{
							case "type":
								type = xReader.Value;
								break;
							case "name":
								name = xReader.Value;
								break;
							case "x":
								pos.x = (int)float.Parse(xReader.Value);
								break;
							case "y":
								pos.y = (int)float.Parse(xReader.Value);
								break;
							case "gid":
								flippedX = (uint.Parse(xReader.Value) & 0x80000000) > 0;
								flippedY = (uint.Parse(xReader.Value) & 0x40000000) > 0;
								break;
							default:
								sb.AppendLine(xReader.Name + ": " + xReader.Value);						
								break;
						}																
					}
					
					if(type != "" && name != "")
					{
						if(!addedTypes.ContainsKey(type))
						{
							var g = new GameObject(type);
							addedTypes.Add(type, g);
							g.transform.SetParent(levelObjects.transform);							
						}
						
						//Debug.Log(type + ": " + name + ": (" + pos.x + ", " + pos.y + ")");
						var newObj = Create (type, name, pos, flippedX, flippedY);					
						newObj.transform.SetParent(addedTypes[type].transform);							
					}
					else
					{
						Debug.Log("Object in level with no type or name: (" + type + ", " + name + ").");
					}										
				}
				break;
			case XmlNodeType.Text:
				//sb.AppendLine("#" + xReader.Value + "#");
				break;
			case XmlNodeType.EndElement:
				//sb.AppendLine("</>");
				break;
			}
		}			
	}
	
	private GameObject Create(string type, string name, Vector2 pos, bool flippedX, bool flippedY)
	{
		pos.x = pos.x / 100.0f;
		pos.y = pos.y / 100.0f;
		
		pos.x = pos.x + 0.14f;
		pos.y = 9.0f - pos.y;
		
		//var path = "Assets/Resources/" + type + "/" + name + ".prefab";
		//var prefab = Resources.LoadAssetAtPath(path, typeof(GameObject));
		//var newObj = (GameObject)Instantiate(prefab, pos, Quaternion.identity);		
		
		if(!_supportedObjects.ContainsKey(name))
		{
			Debug.LogError(name + ": is not supported in the object library.");
		}
		
		var prefab = _supportedObjects[name];
		var newObj = (GameObject)PrefabUtility.InstantiatePrefab(prefab.gameObject);		
		
		newObj.transform.position = pos;
		if(flippedX)
		{
			newObj.transform.localScale = new Vector3(-newObj.transform.localScale.x, newObj.transform.localScale.y, newObj.transform.localScale.z);
		}
		if(flippedY)
		{
			newObj.transform.localScale = new Vector3(newObj.transform.localScale.x, -newObj.transform.localScale.y, newObj.transform.localScale.z);
		}
		return newObj;
	}
	
	private void FixLevel()
	{
		var levelLayers = GameObject.Find("LevelLayers");
		var levelLayers_Background0 = GameObject.Find("Layer Background 0");
		var levelLayers_Background1 = GameObject.Find("Layer Background 1");
		var levelLayers_Background2 = GameObject.Find("Layer Background 2");
		var levelLayers_Midground = GameObject.Find("Layer Midground");
		var levelLayers_Ground = GameObject.Find("Layer Ground");
		var levelLayers_Foreground = GameObject.Find("Layer Foreground");
		
		int groundLayerIndex = LayerMask.NameToLayer("Ground");
		
		if(levelLayers == null)
		{
			levelLayers = new GameObject("LevelLayers");
			
			levelLayers_Background0 = new GameObject("Layer Background 0");
			levelLayers_Background1 = new GameObject("Layer Background 1");
			levelLayers_Background2 = new GameObject("Layer Background 2");
			levelLayers_Midground = new GameObject("Layer Midground");
			levelLayers_Ground = new GameObject("Layer Ground");
			levelLayers_Foreground = new GameObject("Layer Foreground");
			
			levelLayers_Background0.transform.SetParent(levelLayers.transform);			
			levelLayers_Background1.transform.SetParent(levelLayers.transform);
			levelLayers_Background2.transform.SetParent(levelLayers.transform);
			levelLayers_Midground.transform.SetParent(levelLayers.transform);
			levelLayers_Ground.transform.SetParent(levelLayers.transform);
			levelLayers_Foreground.transform.SetParent(levelLayers.transform);
			
			levelLayers_Ground.layer = groundLayerIndex;
			
			var p1 = levelLayers_Background1.AddComponent<SimpleParallax>();										
			if(p1.Cam == null)
			{
				p1.Cam = GameObject.Find("Main Camera").transform;
			}
			p1.Offset = -10;									
			p1.LockY = true;
			
			var p2 = levelLayers_Background2.AddComponent<SimpleParallax>();										
			if(p2.Cam == null)
			{
				p2.Cam = GameObject.Find("Main Camera").transform;
			}
			p2.Offset = -20;									
			p2.LockY = true;
		}
		
		var level = FindObjectOfType<Tiled2Unity.TiledMap>();
		var n = level.transform.childCount;
		
		var colliders = GameObject.Find("FirstStage_01");
		var bg0 = GameObject.Find("Background0_09");
		
		while(level.transform.childCount > 0)
		{
			var c = level.transform.GetChild(0);
			if(c.name.Contains("FirstStage"))
			{
				c.transform.GetComponentInChildren<MeshRenderer>().enabled = false;
				c.SetParent(levelLayers_Ground.transform);
			} 
			else if(c.name.Contains("Background2"))
			{
				c.transform.GetComponentInChildren<MeshRenderer>().sortingLayerName = "Background_2";
				c.SetParent(levelLayers_Background2.transform);				
			} 
			else if(c.name.Contains("Background1"))
			{
				c.transform.GetComponentInChildren<MeshRenderer>().sortingLayerName = "Background_1";
				c.SetParent(levelLayers_Background1.transform);			
			} 
			else if(c.name.Contains("Background0"))
			{
				c.transform.GetComponentInChildren<MeshRenderer>().sortingLayerName = "Background_0";
				c.SetParent(levelLayers_Background0.transform);
			} 
			else if(c.name.Contains("Dirt"))
			{
				c.transform.GetComponentInChildren<MeshRenderer>().sortingLayerName = "Ground";
				c.SetParent(levelLayers_Ground.transform);
				c.transform.GetComponentInChildren<MeshRenderer>().sortingOrder = 0;
				c.gameObject.layer = groundLayerIndex;
			} 
			else if(c.name.Contains("Grass"))
			{
				c.transform.GetComponentInChildren<MeshRenderer>().sortingLayerName = "Ground";
				c.SetParent(levelLayers_Ground.transform);
				c.transform.GetComponentInChildren<MeshRenderer>().sortingOrder = 2;
				c.gameObject.layer = groundLayerIndex;
			} 
			else if(c.name.Contains("Foreground"))
			{
				c.transform.GetComponentInChildren<MeshRenderer>().sortingLayerName = "Foreground";
				c.SetParent(levelLayers_Foreground.transform);
			} 
			else if(c.name.Contains("Fill"))
			{			
				c.transform.GetComponentInChildren<MeshRenderer>().sortingLayerName = "Ground";
				c.SetParent(levelLayers_Ground.transform);
				c.transform.GetComponentInChildren<MeshRenderer>().sortingOrder = -1;
				c.gameObject.layer = groundLayerIndex;
			}
			else if(c.name.Contains("Vines2"))
			{
				c.transform.GetComponentInChildren<MeshRenderer>().sortingLayerName = "Background_2";
				c.SetParent(levelLayers_Background2.transform);
			} 
			else if(c.name.Contains("Vines1"))
			{
				c.transform.GetComponentInChildren<MeshRenderer>().sortingLayerName = "Background_1";
				c.SetParent(levelLayers_Background1.transform);
			} 
			else if(c.name.Contains("Vines_"))
			{
				c.transform.GetComponentInChildren<MeshRenderer>().sortingLayerName = "Ground";
				c.SetParent(levelLayers_Ground.transform);
				c.transform.GetComponentInChildren<MeshRenderer>().sortingOrder = 1;
				c.gameObject.layer = groundLayerIndex;
			} 			
			else
			{
				Debug.Log("Layer not recognized: " + c.name);
				break;
			}
		}			
	}
}