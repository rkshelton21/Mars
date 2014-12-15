using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SpriteRenderer))]
public class SpriteChanger : Editor
{
	private SpriteEditor _editor = null;
	private SpriteRenderer _myRenderer = null;

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

	private bool GetEditor()
	{
		if (_editor == null) 
		{
			_editor = ((SpriteRenderer)this.target).GetComponentInParent<SpriteEditor>();
		}

		if (_editor == null) 
		{
			Debug.Log("Sprite Editor not found.");
			return false;
		}
		return true;
	}
}
