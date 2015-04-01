using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour 
{
	private bool _paused = false;
	private float _originalTimeScale;
	private float _timeStep = 0.05f;
	private float _audioStep = 0.01f;

	// Use this for initialization
	void Start () 
	{
		_originalTimeScale = Time.timeScale;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.P))
		{
			_paused = !_paused;
		}

		if(_paused)
		{
			//slow time to a stop
			if(Time.timeScale > 0)				
			{
				if(Time.timeScale - _timeStep <= 0)					
				{
					Time.timeScale = 0;
				}
				else
				{
					Time.timeScale -= _timeStep;
				}
			}

			if(GetComponent<AudioSource>() != null)
			{
				//soften audio to a stop
				if(GetComponent<AudioSource>().pitch > 0)				
				{
					if(GetComponent<AudioSource>().pitch - _audioStep <= 0)
					{
						GetComponent<AudioSource>().pitch = 0;
					}
					else
					{
						GetComponent<AudioSource>().pitch -= _audioStep;
					}
				}
				else
				{
					GetComponent<AudioSource>().Pause();
				}
			}
		}
		else
		{
			//speed up time to original speed
			if(Time.timeScale < _originalTimeScale)
			{
				if(Time.timeScale + _timeStep >= 100)					
				{
					Time.timeScale = 100;
				}
				else
				{
					Time.timeScale += _timeStep;
				}
			}

			if(GetComponent<AudioSource>() != null)
			{
				//ramp up audio to full
				if(GetComponent<AudioSource>().pitch < 1)
				{
					if(!GetComponent<AudioSource>().isPlaying)
						GetComponent<AudioSource>().Play();
					
					if(GetComponent<AudioSource>().pitch + _audioStep >= 1)
					{
						GetComponent<AudioSource>().pitch = 1;
					}
					else
					{
						GetComponent<AudioSource>().pitch += _audioStep;
					}
				}
			}
		}
	}
}
