using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuOption : MonoBehaviour {

	public bool IsNewGame = false;
	public bool IsQuitButton = false;
	public bool IsExpandable = false;
	public bool StartHidden = false;
	public bool SpawnOnTeams = false;

	public List<MenuOption> SubMenu;
	public AudioClip Button;
	private bool Expanded = false;
	public Transform SpawnObject;

	void Start()
	{
		if(StartHidden)
		{
			GetComponent<Renderer>().enabled = false;
		}
	}

	public void Show()
	{
		GetComponent<Renderer>().enabled = true;

		if(SubMenu != null)
		{
			foreach(var item in SubMenu)
			{
				if(item != null)
				{
					item.Show();
				}
			}
		}
	}

	public void Hide()
	{
		GetComponent<Renderer>().enabled = false;

		if(SubMenu != null)
		{
			foreach(var item in SubMenu)
			{
				if(item != null)
				{
					item.Hide();
				}
			}
		}
	}

	void OnMouseEnter()
	{
		GetComponent<Renderer>().material.color = Color.red;

		if(!GetComponent<Renderer>().enabled)
			return;

		GetComponent<AudioSource>().PlayOneShot(Button);
	}
	
	void OnMouseExit()
	{
		GetComponent<Renderer>().material.color = Color.white;
	}
	
	void OnMouseUp()
	{
		Debug.Log ("Click");
		if(!GetComponent<Renderer>().enabled)
			return;

		if(IsNewGame)
		{
			Application.LoadLevel(Application.loadedLevel + 1);
		}
		else if(IsQuitButton)
		{
			Application.Quit();
		}
		else
		{
			if(SubMenu != null && SubMenu.Count > 0)
			{
				if(!Expanded)
				{
					foreach(var item in SubMenu)
					{
						if(item != null)
						{
							item.Show();
						}
					}
					Expanded = true;
				}
				else
				{
					foreach(var item in SubMenu)
					{
						if(item != null)
						{
							item.Hide();
						}
					}
					Expanded = false;
				}
			}
			else
			{
				var menuEvent = GetComponent<MenuEvent>();
				//menuEvent.Spawn(SpawnObject, SpawnOnTeams);
			}
		}
	}
}
