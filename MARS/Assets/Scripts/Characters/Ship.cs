using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour {

	bool uiIsOff = false;
	bool launchMessageOn = false;
	
	ConsoleTextController launchMessage;
	Transform _messages;
	
	// Use this for initialization
	void Start () {
		_messages = transform.FindChild("Messages");				
		launchMessage = transform.GetComponentInChildren<ConsoleTextController>();
	}
	
	// Update is called once per frame
	void Update () {
		if(!uiIsOff)
		{
			GUIController.ShowNone();
			GameObject.Find("Main Camera").GetComponent<CameraController>().Shake = true;
			uiIsOff = true;
		}
		
		bool toggleConsole = Input.GetKeyDown(KeyCode.C);
		if(toggleConsole)
		{
			if(GUIController.CONActive)
			{
				GUIController.ShowNone();
				_messages.gameObject.SetActive(true);
			}
			else
			{
				GUIController.ShowConsole();
				_messages.gameObject.SetActive(false);
				launchMessage.SetMessage("Click to Launch");
				launchMessageOn = true;
			}
		}				
	}
	
	public void MessageClicked()
	{
		if(launchMessageOn)
		{
			var i = Application.loadedLevel;
			Application.LoadLevel(i + 1);
		}
	}	
}
