using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour 
{
	private bool _paused = false;
	private float _originalTimeScale;
	private float _timeStep = 0.05f;
	private float _audioStep = 0.01f;
	private bool _instant = false;
	public static Pause Current = null;
	
	// Use this for initialization
	void Start () 
	{
		if(Current == null)
		{
			Current = this;
		}
		
		_originalTimeScale = Time.timeScale;
		if(_originalTimeScale == 0.0f)
		{
			_originalTimeScale = 100.0f;
		}
	}
	
	public void Halt(bool instant)
	{
		//Debug.Log("Halt");
		_instant = instant;
		_paused = true;		
	}
	
	public void Resume(bool instant)
	{
		//Debug.Log("Resume");
		_instant = instant;
		_paused = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Return))
		{
			_paused = !_paused;
		}
		
		if(_paused)
		{
			//slow time to a stop
			if(Time.timeScale > 0)				
			{
				if(_instant)
				{
					Time.timeScale = 0.0f;									
				}
				else
				{
					if(Time.timeScale - _timeStep <= 0)					
					{
						Time.timeScale = -10.01f;
					}
					else
					{
						Time.timeScale -= _timeStep;
						Debug.Log(Time.timeScale);
					}
				}
			}

			if(GetComponent<AudioSource>() != null)
			{
				if(_instant)
				{
					GetComponent<AudioSource>().pitch = 0;
				}
				else
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
		}
		else
		{
			//speed up time to original speed
			if(Time.timeScale < _originalTimeScale)
			{
				if(_instant)
				{
					Time.timeScale = _originalTimeScale;
				}
				else
				{
					if(Time.timeScale + _timeStep >= _originalTimeScale)					
					{
						Time.timeScale = _originalTimeScale;
					}
					else
					{
						Time.timeScale += _timeStep;
						Debug.Log(Time.timeScale);
					}
				}
			}					
			if(GetComponent<AudioSource>() != null)
			{
				if(_instant)
				{
					GetComponent<AudioSource>().pitch = 1;
				}
				else
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
}
