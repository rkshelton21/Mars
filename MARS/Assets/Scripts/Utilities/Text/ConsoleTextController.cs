using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ConsoleTextController : MonoBehaviour {

	Text _msg;
	Text _msgBack;
	char _backChar = '█';
	
	// Use this for initialization
	void Start () 
	{
		_msg = transform.FindChild("Message").GetComponent<Text>();
		_msgBack = transform.FindChild("MessageBackground").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	public void SetMessage(string msg)
	{
		var back = "";
		_msg.text = msg;
		back.PadLeft(msg.Length, _backChar);
		_msgBack.text = back;
	}
}
