using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SpriteEditor))]
public class SpriteMapper : Editor
{
	private string _oldBaseName = "30xGround_Tiles_00";
	private string _newBaseName = "30xGround_Tiles_01";

	private string _fromIndex = "00";
	private string _toIndex = "01";

	private string _prefix = "";

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector ();
		//base.OnInspectorGUI();

		_oldBaseName = EditorGUILayout.TextField ("BaseName", _oldBaseName);
		_newBaseName = EditorGUILayout.TextField ("RemapBaseName", _newBaseName);
		_fromIndex = EditorGUILayout.TextField ("From Index", _fromIndex);
		_toIndex = EditorGUILayout.TextField ("To Index", _toIndex);

		_prefix = EditorGUILayout.TextField ("Sprite Name Prefix", _prefix);

		SpriteEditor editor = (SpriteEditor)target;
		if (GUILayout.Button ("All Forward")) 
		{
			editor.ShiftAllSpritesForward(_oldBaseName);
		}

		if (GUILayout.Button ("All Back")) 
		{
			editor.ShiftAllSpritesBackward(_oldBaseName);
		}

		if (GUILayout.Button ("Re-Map")) 
		{
			editor.ReMapAllSprites(_oldBaseName, _newBaseName);
			var temp = _oldBaseName;
			_oldBaseName = _newBaseName;
			_newBaseName = temp;
		}

		if (GUILayout.Button ("Rename all sprites")) 
		{
			editor.RenameAllSprites(_prefix);
		}

		if (GUILayout.Button ("Clear loaded sprites")) 
		{
			editor.ClearLoadedSprites ();
		}

		/*
		if (GUILayout.Button ("Fix Em")) 
		{
			//for(int i=64; i>=39; i--){ editor.ReMapAllSpritesFromIndexToIndex("30xGround_Tiles_00", "30xGround_Tiles_00", i.ToString(), i+9); }
			//for(int i=38; i>=27; i--){ editor.ReMapAllSpritesFromIndexToIndex("30xGround_Tiles_00", "30xGround_Tiles_00", i.ToString(), i+5); }

			for(int i=64; i>=39; i--){ editor.ReMapAllSpritesFromIndexToIndex("30xGround_Tiles_02", "30xGround_Tiles_02", i.ToString(), i+9); }
			for(int i=38; i>=27; i--){ editor.ReMapAllSpritesFromIndexToIndex("30xGround_Tiles_02", "30xGround_Tiles_02", i.ToString(), i+5); }
			for(int i=26; i>=15; i--){ editor.ReMapAllSpritesFromIndexToIndex("30xGround_Tiles_02", "30xGround_Tiles_02", i.ToString(), i+1); }
			for(int i=14; i>=0; i--){ editor.ReMapAllSpritesFromIndexToIndex("30xGround_Tiles_02", "30xGround_Tiles_02", i.ToString(), i); }
		}
		*/

		if (GUILayout.Button ("Snap All Child Sprites With Base Name")) 
		{
			editor.SnapAllChildSpritesWithBaseName(_oldBaseName);
		}

		if (GUILayout.Button ("Generate Sprites")) 
		{
			editor.GenerateSprites(_oldBaseName);
		}

		if (GUILayout.Button ("Create Colliders")) 
		{
			editor.GenerateColliders(_oldBaseName);
		}
	}
}
