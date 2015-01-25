using UnityEngine;
using System.Collections;

public class TextTrigger : MonoBehaviour {

	private MeshRenderer Text;
	public bool Inner = false;
	public bool Outter = false;
	public bool Disabler = false;
	public bool TextRenderer = false;

	// Use this for initialization
	void Start () {
		var guiText = transform.GetComponent<GUIText>();
		if(guiText != null)
		{
			guiText.text = guiText.text.Replace("\\n", "\r\n");
		}
		
		if (!TextRenderer) {
			Text = transform.parent.GetComponentInChildren<MeshRenderer> ();
		} 
		else 
		{
			var textMesh = transform.parent.GetComponentInChildren<TextMesh> ();			
			textMesh.text = textMesh.text.Replace("\\n", "\r\n");
		}

	}
	
	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Player") 
		{
			if(Inner)
			{
				if(Text != null)
				{
					Text.enabled = true;
				}
			}

			if(Outter)
			{

			}

			if(Disabler)
			{
			}
		}
	}

	void OnTriggerExit2D(Collider2D collider)
	{
		if (collider.tag == "Player") 
		{
			if(Inner)
			{
			}

			if(Outter)
			{
				if(Text != null)
				{
					Text.enabled = false;
				}
			}

			if(Disabler)
			{
				GameObject.Destroy(transform.parent.gameObject);
			}
		}
	}
}
