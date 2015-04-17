using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;

[CustomEditor(typeof(MeshCollider))]
public class PlaneSizer  : Editor
{

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector ();
		//base.OnInspectorGUI();
		
		if (GUILayout.Button ("Test")) 
		{
			var height = 2.0f * Mathf.Tan(0.5f * Camera.main.fieldOfView * Mathf.Deg2Rad) * 100.0f;
			var width = height * Screen.width / Screen.height;
			//((Plane)this.target).transform.localScale = Vector3(width, height, 0.1);
			Debug.Log(width + " " + height);
		}
	}
}
