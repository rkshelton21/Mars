#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteEditor : MonoBehaviour {

	private Sprite[] _spriteSheet;
	private string _spriteSheetName = "";

	public Transform SampleSprite;
	public PhysicsMaterial2D ColliderMaterial;

	public void ClearLoadedSprites()
	{
		_spriteSheetName = "";
	}

	private bool LoadSprites(string sheetName)
	{
		if (_spriteSheetName == sheetName) 
		{
			//Debug.Log("Already loaded: " + sheetName);
			return true;
		}
		_spriteSheetName = sheetName;

		var searchResults = UnityEditor.AssetDatabase.FindAssets("n:" + _spriteSheetName);
		List<string> searchResultsList = new List<string>();
		foreach(var s in searchResults)
		{
			if(!searchResultsList.Contains(s))
				searchResultsList.Add(s);
		}

		if (searchResultsList.Count == 0) 
		{
			Debug.Log("No sprite sheets with this name found: " + _spriteSheetName);
			return false;
		}
		if (searchResultsList.Count > 1) 
		{
			Debug.Log("Too many sprite sheets with this name found: " + _spriteSheetName);
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

	public void ReMapAllSprites(string oldBase, string newBase)
	{
		if(!LoadSprites(newBase))
			return;

		var renderers = transform.GetComponentsInChildren<SpriteRenderer> ();
		foreach (var renderer in renderers) 
		{
			if(renderer.sprite != null)
			{
				var matchesBaseName = renderer.sprite.name.Contains(oldBase);
				if(matchesBaseName)
				{
					var currentIndex = int.Parse(renderer.sprite.name.Replace(oldBase + "_", ""));
					renderer.sprite = _spriteSheet[currentIndex];
				}
			}
		}
	}

	public void ReMapAllSpritesFromIndexToIndex(string oldBase, string newBase, string from, int to)
	{
		if(!LoadSprites(newBase))
			return;
		
		var renderers = transform.GetComponentsInChildren<SpriteRenderer> ();
		foreach (var renderer in renderers) 
		{
			if(renderer.sprite != null)
			{
				var matchesBaseName = renderer.sprite.name.Equals(oldBase + "_" + from);
				if(matchesBaseName)
				{
					renderer.sprite = _spriteSheet[to];
				}
			}
		}
	}

	public void ShiftAllSpritesForward(string spriteBase)
	{
		Debug.Log ("Shifting all sprites in sheet: " + spriteBase);
		var renderers = transform.GetComponentsInChildren<SpriteRenderer> ();
		foreach (var renderer in renderers) 
		{
			if(renderer.sprite != null)
			{
				var matchesBaseName = renderer.sprite.name.Contains(spriteBase);
				if(matchesBaseName)
				{
					if(!LoadSprites(spriteBase))
						return;

					var currentIndex = int.Parse(renderer.sprite.name.Replace(_spriteSheetName + "_", ""));
					currentIndex++;

					if(currentIndex >= _spriteSheet.Length)
						currentIndex = 0;

					renderer.sprite = _spriteSheet[currentIndex];
				}
			}
		}

		//string resourceName = "res1";
		//Sprite spr = Resources.Load(resourceName, typeof(Sprite)) as Sprite;
		//spriteRenderer.sprite = spr;
	}

	public void ShiftAllSpritesBackward(string spriteBase)
	{
		Debug.Log ("Shifting all sprites in sheet: " + spriteBase);
		var renderers = transform.GetComponentsInChildren<SpriteRenderer> ();
		foreach (var renderer in renderers) 
		{
			if(renderer.sprite != null)
			{
				var matchesBaseName = renderer.sprite.name.Contains(spriteBase);
				if(matchesBaseName)
				{
					if(!LoadSprites(spriteBase))
						return;
					
					var currentIndex = int.Parse(renderer.sprite.name.Replace(_spriteSheetName + "_", ""));
					currentIndex--;
					
					if(currentIndex < 0)
						currentIndex = _spriteSheet.Length - 1;
					
					renderer.sprite = _spriteSheet[currentIndex];
				}
			}
		}
		
		//string resourceName = "res1";
		//Sprite spr = Resources.Load(resourceName, typeof(Sprite)) as Sprite;
		//spriteRenderer.sprite = spr;
	}

	public void ShiftSingleSpritesForeward(SpriteRenderer target)
	{
		if(target.sprite != null)
		{
			var baseName = target.sprite.name.Substring(0, target.sprite.name.LastIndexOf('_'));
			if(!LoadSprites(baseName))
				return;
				
			var currentIndex = int.Parse(target.sprite.name.Replace(_spriteSheetName + "_", ""));
			currentIndex++;
			
			if(currentIndex >= _spriteSheet.Length)
				currentIndex = 0;
			
			target.sprite = _spriteSheet[currentIndex];
		}

		//string resourceName = "res1";
		//Sprite spr = Resources.Load(resourceName, typeof(Sprite)) as Sprite;
		//spriteRenderer.sprite = spr;
	}

	public void ShiftSingleSpritesBackward(SpriteRenderer target)
	{
		if(target.sprite != null)
		{
			var baseName = target.sprite.name.Substring(0, target.sprite.name.LastIndexOf('_'));
			if(!LoadSprites(baseName))
				return;
			
			var currentIndex = int.Parse(target.sprite.name.Replace(_spriteSheetName + "_", ""));
			currentIndex--;
			
			if(currentIndex < 0)
				currentIndex = _spriteSheet.Length - 1;
			
			target.sprite = _spriteSheet[currentIndex];
		}
		
		//string resourceName = "res1";
		//Sprite spr = Resources.Load(resourceName, typeof(Sprite)) as Sprite;
		//spriteRenderer.sprite = spr;
	}

	public void SnapAllChildSpritesWithBaseName(string baseName)
	{
		var sprites = transform.GetComponentsInChildren<SpriteRenderer> ();
		foreach (var renderer in sprites) 
		{
			if(renderer.sprite != null)
				if(!renderer.sprite.name.StartsWith(baseName))
					continue;

			var x = renderer.transform.position.x - 0.15;
			var xa = (int)(x / 0.3f);
			var xdiff = (xa * 0.3f + 0.15f - x);
			//Debug.Log (xdiff);
			if (xdiff > 0.225)
					xa--;
			if (xdiff < 0)
					xa++;
			//special case
			if (x > -0.15 && x < 0.15) {
					xa = 0;
			}

			var y = renderer.transform.position.y - 0.15;
			var ya = (int)(y / 0.3f);
			var ydiff = (ya * 0.3f + 0.15f - y);
			//Debug.Log(ydiff);
			if (ydiff > 0.225)
					ya--;
			if (ydiff < 0)
					ya++;
			//special case
			if (y > -0.15 && y < 0.15) {
					ya = 0;
			}

			var p = renderer.transform.position;
			p.x = xa * 0.3f + 0.15f;
			p.y = ya * 0.3f + 0.15f;
			renderer.transform.position = p;
			//Debug.Log(_myRenderer.transform.localPosition);		
		}
	}

	public void GenerateSprites(string baseName)
	{
		if (!LoadSprites (baseName))
			return;
		var startPosition = transform.position;

		var max = Mathf.Min (_spriteSheet.Length, 54);
		var renderer = SampleSprite.GetComponent<SpriteRenderer> ();
		for (int i=0; i < max; i++) 
		{
			var newPos = startPosition;
			newPos.x += (i % 15) * 0.3f;
			newPos.y -= (i / 15) * 0.3f;

			renderer.sprite = _spriteSheet[i];
			var obj = Instantiate (SampleSprite, newPos, Quaternion.identity);
			((Transform)obj).parent = transform;
		}
	}

	public void GenerateColliders(string baseName)
	{
		if (!LoadSprites (baseName))
			return;

		var baseObject = new GameObject ();

		var points = new Vector2[30];
		var internalPoints = new Vector2[9]
		{
			new Vector2(-0.15f, 0.15f),
			new Vector2(0.0f, 0.15f),
			new Vector2(0.15f, 0.15f),

			new Vector2(-0.15f, 0f),
			new Vector2(0f, 0f),
			new Vector2(0.15f, 0f),

			new Vector2(-0.15f, -0.15f),
			new Vector2(0f, -0.15f),
			new Vector2(0.15f, -0.15f)
		};

		var sprites = transform.GetComponentsInChildren<SpriteRenderer> ();
		foreach (var renderer in sprites) 
		{
			if (renderer.sprite != null)
				if (!renderer.sprite.name.StartsWith (baseName))
					continue;

			var currentIndex = int.Parse(renderer.sprite.name.Replace(_spriteSheetName + "_", ""));
			var n = 2;

			switch(currentIndex)
			{
			case 00:
				points[0] = internalPoints[0];
				points[1] = internalPoints[2];
				break;
			case 01:
				points[0] = internalPoints[0];
				points[1] = internalPoints[5];
				break;
			case 02:
				points[0] = internalPoints[0];
				points[1] = internalPoints[8];
				break;
			case 03:
				points[0] = internalPoints[0];
				points[1] = internalPoints[7];
				break;
			case 04:
				points[0] = internalPoints[0];
				points[1] = internalPoints[6];
				break;
			case 05:
				points[0] = internalPoints[1];
				points[1] = internalPoints[5];
				break;
			case 06:
				points[0] = internalPoints[1];
				points[1] = internalPoints[8];
				break;
			case 07:
				points[0] = internalPoints[1];
				points[1] = internalPoints[7];
				break;
			case 08:
				points[0] = internalPoints[7];
				points[1] = internalPoints[1];
				break;
			case 09:
				points[0] = internalPoints[6];
				points[1] = internalPoints[1];
				break;
			case 10:
				points[0] = internalPoints[3];
				points[1] = internalPoints[1];
				break;
			case 11:
				points[0] = internalPoints[8];
				points[1] = internalPoints[2];
				break;
			case 12:
				points[0] = internalPoints[7];
				points[1] = internalPoints[2];
				break;
			case 13:
				points[0] = internalPoints[6];
				points[1] = internalPoints[2];
				break;
			case 14:
				points[0] = internalPoints[3];
				points[1] = internalPoints[2];
				break;
			case 15:
				n = 0;
				break;
			case 16:
				points[0] = internalPoints[3];
				points[1] = internalPoints[5];
				break;
			case 17:
				points[0] = internalPoints[3];
				points[1] = internalPoints[8];
				break;
			case 18:
				points[0] = internalPoints[3];
				points[1] = internalPoints[7];
				break;
			case 19:
				n = 3;
				points[0] = internalPoints[3];
				points[1] = new Vector2();
				points[2] = internalPoints[8];
				break;
			case 20:
				n = 0;
				break;
			case 21:
				n = 3;
				points[0] = internalPoints[3];
				points[1] = new Vector2();
				points[2] = internalPoints[7];
				break;
			case 22:
				n = 3;
				points[0] = internalPoints[7];
				points[1] = new Vector2();
				points[2] = internalPoints[5];
				break;
			case 23:
				n = 0;
				break;
			case 24:
				n = 3;
				points[0] = internalPoints[6];
				points[1] = new Vector2();
				points[2] = internalPoints[5];
				break;
			case 25:
				n = 0;
				break;
			case 26:
				points[0] = internalPoints[7];
				points[1] = internalPoints[5];
				break;
			case 27:
				points[0] = internalPoints[6];
				points[1] = internalPoints[5];
				break;
			case 28:
				n = 0;
				break;
			case 29:
				n = 0;
				break;
			case 30:
				n = 0;
				break;
			case 31:
				n = 0;
				break;
			case 32:
				points[0] = internalPoints[3];
				points[1] = internalPoints[5];
				break;
			case 33:
				points[0] = internalPoints[3];
				points[1] = internalPoints[2];
				break;
			case 34:
				points[0] = internalPoints[3];
				points[1] = internalPoints[1];
				break;
			case 35:
				n = 3;
				points[0] = internalPoints[3];
				points[1] = new Vector2();
				points[2] = internalPoints[2];
				break;
			case 36:
				n = 0;
				break;
			case 37:
				n = 3;
				points[0] = internalPoints[3];
				points[1] = new Vector2();
				points[2] = internalPoints[1];
				break;
			case 38:
				n = 3;
				points[0] = internalPoints[1];
				points[1] = new Vector2();
				points[2] = internalPoints[5];
				break;
			case 39:
				n = 0;
				break;
			case 40:
				n = 3;
				points[0] = internalPoints[0];
				points[1] = new Vector2();
				points[2] = internalPoints[5];
				break;
			case 41:
				n = 0;
				break;
			case 42:
				points[0] = internalPoints[1];
				points[1] = internalPoints[5];
				break;
			case 43:
				points[0] = internalPoints[0];
				points[1] = internalPoints[5];
				break;
			case 44:
				n = 0;
				break;
			case 45:
				n = 0;
				break;
			case 46:
				n = 0;
				break;
			case 47:
				n = 0;
				break;
			case 48:
				points[0] = internalPoints[6];
				points[1] = internalPoints[8];
				break;
			case 49:
				points[0] = internalPoints[6];
				points[1] = internalPoints[5];
				break;
			case 50:
				points[0] = internalPoints[6];
				points[1] = internalPoints[2];
				break;
			case 51:
				points[0] = internalPoints[6];
				points[1] = internalPoints[1];
				break;
			case 52:
				points[0] = internalPoints[0];
				points[1] = internalPoints[6];
				break;
			case 53:
				points[0] = internalPoints[7];
				points[1] = internalPoints[5];
				break;
			case 54:
				points[0] = internalPoints[7];
				points[1] = internalPoints[2];
				break;
			case 55:
				points[0] = internalPoints[1];
				points[1] = internalPoints[7];
				break;
			case 56:
				points[0] = internalPoints[7];
				points[1] = internalPoints[1];
				break;
			case 57:
				points[0] = internalPoints[0];
				points[1] = internalPoints[7];
				break;
			case 58:
				points[0] = internalPoints[3];
				points[1] = internalPoints[7];
				break;
			case 59:
				points[0] = internalPoints[2];
				points[1] = internalPoints[8];
				break;
			case 60:
				points[0] = internalPoints[1];
				points[1] = internalPoints[8];
				break;
			case 61:
				points[0] = internalPoints[0];
				points[1] = internalPoints[8];
				break;
			case 62:
				points[0] = internalPoints[3];
				points[1] = internalPoints[8];
				break;
			case 63:
				n = 0;
				break;
			default:
				n = 0;
				break;
			}

			if(n >= 2)
			{
				var collider = baseObject.AddComponent<EdgeCollider2D>();
				var list = new System.Collections.Generic.List<Vector2>();
				for(int i=0; i<n; i++)
				{
					list.Add(points[i] + new Vector2(renderer.transform.position.x, renderer.transform.position.y));
				}
				collider.points = list.ToArray();
				collider.sharedMaterial = ColliderMaterial;
			}
		}
	}

	public void RenameAllSprites(string SpritePrefix)
	{
		var sprites = transform.GetComponentsInChildren<SpriteRenderer> ();
		foreach (var renderer in sprites) 
		{
			if(renderer.sprite != null)
			{
				if(renderer.transform != null)
				{
					renderer.transform.name = SpritePrefix + renderer.sprite.name;
				}
			}
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
#endif
