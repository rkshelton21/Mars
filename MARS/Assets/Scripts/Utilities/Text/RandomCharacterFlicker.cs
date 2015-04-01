using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RandomCharacterFlicker : MonoBehaviour {
	System.Random _r = new System.Random();
	public float Speed = 0;
	Text _myText;
	float _waitTime = 0;
	bool fadingOut = true;
	public int MinFlickers = 0;
	public int MaxFlickers = 10;
	public char FlickerCharacter = '█';
	private string BaseContent = "";
	
	// Use this for initialization
	void Start () {
		_myText = transform.GetComponent<Text>();
		BaseContent = _myText.text;		
	}
	
	// Update is called once per frame
	void Update () {
		var text = BaseContent.ToCharArray();
		if(_waitTime <= 0)
		{
			_waitTime = Speed;
			//_r.Next(70) / 255f;	
			var numFlickers = _r.Next(MinFlickers, MaxFlickers);
			for(int i=0; i<numFlickers; i++)
			{
				var index = _r.Next(BaseContent.Length);		
				text[index] = FlickerCharacter;
			}
			_myText.text = new string(text);
		}
		else
		{
			_waitTime -= Time.deltaTime;
		}
	}
}
