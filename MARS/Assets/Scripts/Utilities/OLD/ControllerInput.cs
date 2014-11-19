/*
using UnityEngine;
using System.Collections;

public class IControllerInput
{	public float HorizontalInput;
	public bool Jump;
}

public class AIControllerInput: IControllerInput
{
	private Transform _Player = null;
	public float HorizontalInput
	{
		get
		{
			if(_Player == null)
			{
				_Player = GameObject.Find("Player").transform;
				return _Player.position.x - _Player.position.x;
			}
			else
			{
				return _Player.position.x - _Player.position.x;
			}
		}

		private set{}
	}
	public bool Jump
	{
		get
		{
			return false;
		}

		private set{}
	}
}

public class PlayerControllerInput: IControllerInput
{
	public float HorizontalInput
	{
		get
		{
			Debug.Log("Hello");
			return Input.GetAxis("Horizontal");
		}
		
		private set{}
	}
	public bool Jump
	{
		get
		{
			return Input.GetKeyDown(KeyCode.Space);
		}
		
		private set{}
	}
}
*/