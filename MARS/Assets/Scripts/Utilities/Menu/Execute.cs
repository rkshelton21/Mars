using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Execute : MonoBehaviour {
	
	public bool IsExpandable = false;
	public bool StartHidden = false;
	
	public List<MenuOption> SubMenu;
	public AudioClip Button;
	private bool Expanded = false;

	public string ExectionString;

	void Start()
	{
		if(StartHidden)
		{
			GetComponent<Renderer>().enabled = false;
		}
	}
	
	void Show()
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
	
	void Hide()
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
		GetComponent<AudioSource>().PlayOneShot(Button);
		GetComponent<Renderer>().material.color = Color.red;
	}
	
	void OnMouseExit()
	{
		GetComponent<Renderer>().material.color = Color.white;
	}
	
	void OnMouseUp()
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
			switch(ExectionString)
			{
			case "Add Item":
				var inv = GameObject.Find("Inventory");
				var menu = inv.GetComponent<Inventory>();
				menu.SetItemCount("Heart", System.DateTime.UtcNow.Second);
					break;
			default:
					Debug.LogError("No Execution Code Provided");
				break;
			}
		}
	}
}
