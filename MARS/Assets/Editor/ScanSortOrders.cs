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
}
