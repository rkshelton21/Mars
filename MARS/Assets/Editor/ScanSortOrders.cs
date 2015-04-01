using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;

[CustomEditor(typeof(SortingDisplay))]
public class ScanSortOrders  : Editor
{
	public override void OnInspectorGUI()
	{
		var renderList = GetSortingLayerNames();
		
		DrawDefaultInspector ();
		//base.OnInspectorGUI();
		
		if (GUILayout.Button ("Scan")) 
		{
			Scan(renderList);		
		}
		
		if (GUILayout.Button ("Fix Level")) 
		{
			FixLevel();		
		}		
	}
	
	// Get the sorting layer names
	public Dictionary<string, List<KeyValuePair<string, string>>> GetSortingLayerNames() {
		System.Type internalEditorUtilityType = typeof(InternalEditorUtility);
		PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
		var layers = (string[])sortingLayersProperty.GetValue(null, new object[0]);
		
		var layerList = new Dictionary<string, List<KeyValuePair<string, string>>>();
		foreach(var layer in layers)
		{
			layerList.Add(layer, new List<KeyValuePair<string, string>>());
		}
		
		return layerList;
	}	
	
	private void Scan(Dictionary<string, List<KeyValuePair<string, string>>> renderList)
	{
		var sprites = FindObjectsOfType<Renderer>();
		foreach(var sprite in sprites)
		{
			var order = sprite.sortingOrder;
			var layer = sprite.sortingLayerName;
			if(string.IsNullOrEmpty(layer))
				layer = "Default";
			
			if(renderList.ContainsKey(layer))
			{
				var name = sprite.gameObject.name;
				if(sprite.gameObject.transform.parent != null)
				{
					name = sprite.gameObject.transform.parent.name + "." + name;
				}
				var kvp = new KeyValuePair<string, string>(name, order.ToString().PadLeft(5, '0'));
				if(!renderList[layer].Contains(kvp))
					renderList[layer].Add(kvp);
			}
			else
			{
				var name = sprite.gameObject.name;
				if(sprite.gameObject.transform.parent != null)
				{
					name = sprite.gameObject.transform.parent.name + "." + name;
				}
				renderList.Add(layer, new List<KeyValuePair<string, string>>());
				var kvp = new KeyValuePair<string, string>(name, order.ToString().PadLeft(5, '0'));					
				renderList[layer].Add(kvp);
			}
		}
		
		var s = "";
		int i = 0;
		foreach(var layer in renderList.Keys)
		{
			s += "Layer " + i + ": " + layer + "\n";
			renderList[layer].Sort((x, y) => x.Value.CompareTo(y.Value));
			foreach(var order in renderList[layer])
			{
				s += "   " + order.Key + ": " + order.Value + "\n";					
			}	
			i++;
		}
		
		Debug.Log(s);
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
			} 
			else if(c.name.Contains("Grass"))
			{
				c.transform.GetComponentInChildren<MeshRenderer>().sortingLayerName = "Ground";
				c.SetParent(levelLayers_Ground.transform);
				c.transform.GetComponentInChildren<MeshRenderer>().sortingOrder = 2;
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
			} 			
			else
			{
				Debug.Log("Layer not recognized: " + c.name);
				break;
			}
		}
	}
}
