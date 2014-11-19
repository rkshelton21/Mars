using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour 
{
	public float Health;
	public Color GainColor;
	public Color LossColor;
	public float FillSpeed = 2.0f;

	private float _minX;
	private GUITexture _topBar;
	private GUITexture _bottomBar;
	private float screenPercent;

	// Use this for initialization
	void Start () 
	{
		_topBar = transform.FindChild("TopHealthBar").GetComponent<GUITexture>();
		_bottomBar = transform.FindChild("BottomHealthBar").GetComponent<GUITexture>();

		_minX = -_topBar.GetScreenRect().width;
	}
	
	// Update is called once per frame
	void Update () 
	{
		screenPercent = Screen.width / 100f / FillSpeed;

		if(Health > 1)
			Health = 1;
		if(Health < 0)
			Health = 0;

		var topInset = _topBar.pixelInset;
		var bottomInset = _bottomBar.pixelInset;

		var targetWidth =  (1 - Health) * _minX;

		//if the top bar needs to shrink to get to the target width
		//losing health
		if(topInset.width > targetWidth)
		{
			//snap the health to the correct position
			//and start shrinking the red bar
			topInset.width = targetWidth;
			_bottomBar.color = LossColor;
		}

		//if the bottom bar needs to grow to get to the target width
		//gaining health
		if(bottomInset.width < targetWidth)
		{
			//snap the green to the correct position
			//and start growing the white bar
			bottomInset.width = targetWidth;
			_bottomBar.color = GainColor;
		}

		if(bottomInset.width < targetWidth)
		{
			bottomInset.width += screenPercent;
			if(bottomInset.width > targetWidth)
				bottomInset.width = targetWidth;
		}
		else if(bottomInset.width > targetWidth)
		{
			bottomInset.width -= screenPercent;
			if(bottomInset.width < targetWidth)
				bottomInset.width = targetWidth;
		}

		if(topInset.width < targetWidth)
		{
			topInset.width += screenPercent;
			if(topInset.width > targetWidth)
				topInset.width = targetWidth;
		}
		else if(topInset.width > targetWidth)
		{
			topInset.width -= screenPercent;
			if(topInset.width < targetWidth)
				topInset.width = targetWidth;
		}

		_topBar.pixelInset = topInset;
		_bottomBar.pixelInset = bottomInset;
	}
}
