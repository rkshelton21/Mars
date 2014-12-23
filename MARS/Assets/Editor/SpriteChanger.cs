using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(SpriteRenderer))]
public class SpriteChanger : Editor
{
	private SpriteEditor _editor = null;
	private SpriteRenderer _myRenderer = null;
	private Sprite[] _spriteSheet;
	private string _spriteSheetName = "";
	private string[] _assetFolders = new string[]{
		"Assets/Images/Characters/BlobBoss",
		"Assets/Images/Platform"
	};
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector ();
		//base.OnInspectorGUI ();

		GetEditor ();

		if (_myRenderer == null) 
		{
			_myRenderer = ((SpriteRenderer)this.target);
		}

		if (_editor != null) 
		{
			if (GUILayout.Button ("Prev")) 
			{
				_editor.ShiftSingleSpritesBackward (_myRenderer);
			}

			if (GUILayout.Button ("Next")) 
			{
				_editor.ShiftSingleSpritesForeward (_myRenderer);
			}
		}
		else
		{
			if (GUILayout.Button ("Prev")) 
			{
				ExpensiveShiftSprite(false);
			}
			
			if (GUILayout.Button ("Next")) 
			{
				ExpensiveShiftSprite(true);
			}
		}

		if (GUILayout.Button ("Snap")) 
		{
			var x = _myRenderer.transform.position.x - 0.15;
			var xa = (int)(x / 0.3f);
			var xdiff = (xa*0.3f + 0.15f - x);
			//Debug.Log(xdiff);
			if(xdiff > 0.225)
				xa--;
			if(xdiff < 0)
				xa++;
			//special case
			if( x > -0.15 && x < 0.15)
			{
				xa = 0;
			}

			var y = _myRenderer.transform.position.y - 0.15;
			var ya = (int)(y / 0.3f);
			var ydiff = (ya*0.3f + 0.15f - y);
			//Debug.Log(ydiff);
			if(ydiff > 0.225)
				ya--;
			if(ydiff < 0)
				ya++;
			//special case
			if( y > -0.15 && y < 0.15)
			{
				ya = 0;
			}

			var p = _myRenderer.transform.position;
			p.x = xa*0.3f + 0.15f;
			p.y = ya*0.3f + 0.15f;
			_myRenderer.transform.position = p;
			Debug.Log(_myRenderer.transform.localPosition);
		}

		if (GUILayout.Button ("Print Info")) 
		{
			Debug.Log("World: " + _myRenderer.transform.position);
			Debug.Log("Local: " + _myRenderer.transform.localPosition);
		}
	}

	private void ExpensiveShiftSprite(bool forward)
	{
		var target = _myRenderer;
		if(target.sprite != null)
		{
			var baseName = target.sprite.name.Substring(0, target.sprite.name.LastIndexOf('_'));
			if(!ExpensiveLoadSprites(baseName))
				return;
			
			var currentIndex = int.Parse(target.sprite.name.Replace(_spriteSheetName + "_", ""));

			if(forward)
			{
				currentIndex--;
			
				if(currentIndex < 0)
					currentIndex = _spriteSheet.Length - 1;
			}
			else
			{
				currentIndex++;
				
				if(currentIndex >= _spriteSheet.Length)
					currentIndex = 0;
			}

			target.sprite = _spriteSheet[currentIndex];
		}
		
		//string resourceName = "res1";
		//Sprite spr = Resources.Load(resourceName, typeof(Sprite)) as Sprite;
		//spriteRenderer.sprite = spr;
	}

	private bool ExpensiveLoadSprites(string sheetName)
	{
		if (_spriteSheetName == sheetName) 
		{
			//Debug.Log("Already loaded: " + sheetName);
			return true;
		}
		_spriteSheetName = sheetName;
		
		var searchResults = UnityEditor.AssetDatabase.FindAssets(sheetName, _assetFolders);
		List<string> searchResultsList = new List<string>();
		foreach(var s in searchResults)
		{
			if(!searchResultsList.Contains(s))
			{
				Debug.Log(searchResults.Length);
				searchResultsList.Add(s);
			}
		}
		
		if (searchResultsList.Count == 0) 
		{
			Debug.Log("No sprite sheets with this name found: " + _spriteSheetName);
			Debug.Log("Or folder not found in search list" + _spriteSheetName);
			_spriteSheetName = "";
			return false;
		}
		if (searchResultsList.Count > 1) 
		{
			Debug.Log("Too many sprite sheets with this name found: " + _spriteSheetName);
			_spriteSheetName = "";
			return false;
		}
		
		var assetGuid = searchResultsList [0];
		var assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(assetGuid);
		
		var sprites = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(assetPath);
		_spriteSheet = new Sprite[sprites.Length - 1];
		int i = 0;
		foreach(var x in sprites)
		{
			if(x.GetType() == typeof(Sprite))
			{
				_spriteSheet[i] = (Sprite)x;
				i++;
			}
		}
		
		Debug.Log ("Sprites Loaded: " + sheetName);
		return true;
	}

	private bool GetEditor()
	{
		if (_editor == null) 
		{
			_editor = ((SpriteRenderer)this.target).GetComponentInParent<SpriteEditor>();
		}

		if (_editor == null) 
		{
			//Debug.Log("Sprite Editor not found.");
			return false;
		}
		return true;
	}
}
