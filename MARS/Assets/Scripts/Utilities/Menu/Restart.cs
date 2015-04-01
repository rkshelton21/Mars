using UnityEngine;
using System.Collections;

public class Restart : MonoBehaviour {

	public AudioClip Button;
	private GUIText _Text;

	// Use this for initialization
	void Start () {	
		_Text = transform.GetComponent<GUIText>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseEnter()
	{
		_Text.color = Color.red;
		
		GetComponent<AudioSource>().PlayOneShot(Button);
	}
	
	void OnMouseExit()
	{
		_Text.color = Color.white;
	}
	
	void OnMouseUp()
	{
		Application.LoadLevel(Application.loadedLevelName);
	}
}
