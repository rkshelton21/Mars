using UnityEngine;
using System.Collections;

public class GUIController {
	
	private static HUD _hud;
	private static Inventory _inv;
	private static Transform _fps;
	private static Transform _con;
	private static Transform _gui;
	private static ParticleSystem _dust;
	
	public static bool HUDActive;
	public static bool INVActive;
	public static bool FPSActive;
	public static bool CONActive;
	
	private static void Init()
	{
		if(_gui == null)
		{
			_gui = GameObject.Find("GUI").transform;
			_hud = _gui.FindChild("HUD").GetComponent<HUD>();
			_inv = _gui.FindChild("Inventory").GetComponent<Inventory>();
			_fps = _gui.FindChild("FPS");
			_con = _gui.FindChild("Console");
			_dust = GameObject.Find("Main Camera").GetComponent<ParticleSystem>();
		}
	}
	
	public static void ShowHUD()
	{
		Init();
		_hud.Show();
		_inv.Hide ();
		_fps.gameObject.SetActive(false);
		_con.gameObject.SetActive(false);
		_dust.GetComponent<Renderer>().enabled = true;
		HUDActive = true;
		INVActive = false;
		FPSActive = false;
		CONActive = false;
	}
	
	public static void ShowNone()
	{
		Init();
		_hud.Hide();
		_inv.Hide ();
		_fps.gameObject.SetActive(false);
		_con.gameObject.SetActive(false);
		_dust.GetComponent<Renderer>().enabled = false;
		HUDActive = false;
		INVActive = false;
		FPSActive = false;
		CONActive = false;
	}
	
	public static void ShowInventory()
	{
		Init();
		_hud.Hide();
		_inv.Show();
		_fps.gameObject.SetActive(false);
		_con.gameObject.SetActive(false);
		_dust.GetComponent<Renderer>().enabled = false;
		HUDActive = false;
		INVActive = true;
		FPSActive = false;
		CONActive = false;
	}
	
	public static void ShowConsole()
	{
		Init();
		_hud.Hide();
		_inv.Hide();
		_fps.gameObject.SetActive(false);
		_con.gameObject.SetActive(true);
		_dust.GetComponent<Renderer>().enabled = false;
		HUDActive = false;
		INVActive = false;
		FPSActive = false;
		CONActive = true;
	}
}
