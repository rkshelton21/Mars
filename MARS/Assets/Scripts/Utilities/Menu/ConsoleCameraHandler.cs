using UnityEngine;
using System.Collections;

public class ConsoleCameraHandler : MonoBehaviour {
	private Transform _scaledCanvas;
	private Transform _consoleCam;
	
	private void GetChildren()
	{
		_scaledCanvas = transform.FindChild("ScaledCanvas");			
		_consoleCam = transform.FindChild("Camera");		
	}
	
	public void Show()
	{
		if(_scaledCanvas == null || _consoleCam == null)
		{
			GetChildren();
		}
		
		if(_scaledCanvas != null && _consoleCam != null)
		{
			for(int i=0; i < _scaledCanvas.childCount; i++)
			{
				_scaledCanvas.GetChild(i).gameObject.SetActive(true);
			}
			_consoleCam.gameObject.SetActive(true);
		}
	}	
	
	public void Hide()
	{
		if(_scaledCanvas == null || _consoleCam == null)
		{
			GetChildren();
		}
			
		if(_scaledCanvas != null && _consoleCam != null)
		{
			for(int i=0; i < _scaledCanvas.childCount; i++)
			{
				_scaledCanvas.GetChild(i).gameObject.SetActive(false);
			}
			_consoleCam.gameObject.SetActive(false);
		}
	}
}
