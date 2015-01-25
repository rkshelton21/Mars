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
			renderer.enabled = false;
		}
	}

	public void Show()
	{
		renderer.enabled = true;

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
		renderer.enabled = false;

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
		renderer.material.color = Color.red;

		if(!renderer.enabled)
			return;

		audio.PlayOneShot(Button);
	}
	
	void OnMouseExit()
	{
		renderer.material.color = Color.white;
	}
	
	void OnMouseUp()
	{
		Debug.Log ("Click");
		if(!renderer.enabled)
			return;

		if(IsNewGame)
		{
			Application.LoadLevel("New Game");
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
				menuEvent.Spawn(SpawnObject, SpawnOnTeams);
			}
		}
	}
}
