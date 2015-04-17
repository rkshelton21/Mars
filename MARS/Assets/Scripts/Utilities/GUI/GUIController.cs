using UnityEngine;
using System.Collections;

public class GUIController {
	
	private static HUD _hud;
	private static Inventory _inv;
	private static Transform _fps;
	private static Transform _con;
	private static ConsoleCameraHandler _conRender;
	private static Transform _gui;
	private static ParticleSystem _dust;
	private static CameraController _cam;
	
	private static Vector3 _oldCamPos;
	private static float _oldCamSize;
	
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
			_conRender = GameObject.Find("ConsoleRender").GetComponent<ConsoleCameraHandler>();
			_cam = GameObject.Find("Main Camera").GetComponent<CameraController>();
			_dust = GameObject.Find("Main Camera").GetComponent<ParticleSystem>();
			_oldCamPos = _cam.GetPosition();
			_oldCamSize = _cam.GetCamera().orthographicSize;
		}
	}
	
	public static void ShowHUD()
	{
		Init();
		_hud.Show();
		_inv.Hide ();
		_fps.gameObject.SetActive(false);
		_con.gameObject.SetActive(false);
		_conRender.Hide();
		
		if(_dust == null)
			_dust = GameObject.Find("Main Camera").GetComponent<ParticleSystem>();
		_dust.GetComponent<Renderer>().enabled = true;
		
		HUDActive = true;
		INVActive = false;
		FPSActive = false;
		CONActive = false;
		
		if(_cam.Locked)
		{
			_cam.Locked = false;
			_cam.SetPosition(_oldCamPos);
			_cam.GetCamera().orthographicSize = _oldCamSize;		
		}
	}
	
	public static void ShowNone()
	{
		Init();
		_hud.Hide();
		_inv.Hide ();
		_fps.gameObject.SetActive(false);
		_con.gameObject.SetActive(false);
		_conRender.Hide();
		_dust.GetComponent<Renderer>().enabled = false;
		HUDActive = false;
		INVActive = false;
		FPSActive = false;
		CONActive = false;
		
		if(_cam.Locked)
		{
			_cam.Locked = false;
			_cam.SetPosition(_oldCamPos);
			_cam.GetCamera().orthographicSize = _oldCamSize;		
		}	
	}
	
	public static void ShowInventory()
	{
		Init();
		_hud.Hide();
		_inv.Show();
		_fps.gameObject.SetActive(false);
		_con.gameObject.SetActive(false);
		_conRender.Hide();
		_dust.GetComponent<Renderer>().enabled = false;
		HUDActive = false;
		INVActive = true;
		FPSActive = false;
		CONActive = false;
		
		if(_cam.Locked)
		{
			_cam.Locked = false;
			_cam.SetPosition(_oldCamPos);
			_cam.GetCamera().orthographicSize = _oldCamSize;		
		}
	}
	
	public static void ShowConsole()
	{
		Init();
		_hud.Hide();
		_inv.Hide();
		_fps.gameObject.SetActive(false);
		_con.gameObject.SetActive(true);
		_conRender.Show();
		_dust.GetComponent<Renderer>().enabled = false;
		HUDActive = false;
		INVActive = false;
		FPSActive = false;
		CONActive = true;
		
		_cam.Locked = true;
		_oldCamPos = _cam.GetPosition();
		_oldCamSize = _cam.GetCamera().orthographicSize;
		_cam.GetCamera().orthographicSize = 1.0f;
		
		var newPos = _cam.GetPosition();
		newPos.x = 0.0f;
		newPos.y = 0.0f;
		_cam.SetPosition(newPos);
		
		//var newPos = _cam.GetPosition();
		//newPos.z = _con.position.z;
		//_con.position = newPos;				
	}
}
