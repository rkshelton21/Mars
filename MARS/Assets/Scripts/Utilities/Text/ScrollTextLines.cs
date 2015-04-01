using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScrollTextLines : MonoBehaviour {
	string _text;
	string[] _lines;
	string[] _mask;
	Text _myText;
	public float Speed = 0;
	float _waitTime = 0;
	public string Mask = "";
	public string Content = "";
	
	// Use this for initialization
	void Start () {
		_myText = transform.GetComponent<Text>();
		_text = _myText.text;
		_lines = Content.Split('\n');
		if(!string.IsNullOrEmpty(Mask))
		{
			_mask = Mask.Split('\n');
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(_waitTime <= 0)
		{
			_waitTime = Speed;
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			for(int i=0; i<_lines.Length; i++)
			{
				if(!string.IsNullOrEmpty(_lines[i]) && _mask != null)
				{
					var mask = _mask[i];
					
					var leftWhiteSpaceCount = mask.Length;
					mask = mask.TrimStart();
					leftWhiteSpaceCount -= mask.Length;
					
					var rightWhiteSpaceCount = mask.Length;
					mask = mask.TrimEnd();
					rightWhiteSpaceCount -= mask.Length;
					
					var line = _lines[i];
					var newLine = "";
					if(mask.Length > 0)
					{
						newLine = line.Substring(1, mask.Length - 1) + line.Substring(0, 1);
						newLine = newLine.PadLeft(mask.Length + leftWhiteSpaceCount);
						newLine = newLine.PadRight(mask.Length + rightWhiteSpaceCount);
					}
					sb.AppendLine(newLine);
					_lines[i] = line.Substring(1, line.Length - 1) + line.Substring(0, 1);									
				}
				
				if(!string.IsNullOrEmpty(_lines[i]) && _mask == null)
				{
					var line = _lines[i];
					var newLine = line.Substring(1, line.Length - 1) + line.Substring(0, 1);
					sb.AppendLine(newLine);
					_lines[i] = newLine;
				}
			}
			
			_myText.text = sb.ToString();
		}
		else
		{
			_waitTime -= Time.deltaTime;
		}
	}
}
