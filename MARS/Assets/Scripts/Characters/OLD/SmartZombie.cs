﻿using UnityEngine;
using System.Collections;

public class SmartZombie : MonoBehaviour {

	public Transform Target;
	float MaxSpeed = 5.0f;
	public Transform GroundCheck;
	public LayerMask WhatIsGround;
	bool IgnoreSteepSlopeLimit = true;
	
	float offsetFrom45ForSlopes = 10;
	
	bool _facingRight = true;
	public bool _grounded = false;
	float _groundRadius = 0.3f;
	bool _ignoreLeftInput = false;
	bool _ignoreRightInput = false;
	Animator _anim;
	
	public bool IsOnGround = false;
	public bool IsOnSteepGround = false;
	public bool IsOnRegularGround = false;
	
	
	public bool IsSleeping = false;
	public bool IsAwake = false;
	// Use this for initialization
	void Start () 
	{
		_anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		IsSleeping = rigidbody2D.IsSleeping();
		IsAwake = rigidbody2D.IsAwake();
		
		if(IsOnGround)
		{
			//transform.rigidbody2D.gravityScale = 0.0f;
		}
		else
		{
			transform.rigidbody2D.gravityScale = 1.0f;
		}
		
		_grounded = Physics2D.OverlapCircle(GroundCheck.position, _groundRadius, WhatIsGround);
		_anim.SetBool("Ground", _grounded);
		_anim.SetFloat("verticalSpeed", rigidbody2D.velocity.y);
		
		float move = Target.position.x - transform.position.x;
		
		if(!IgnoreSteepSlopeLimit)
		{
			if(_ignoreLeftInput && move < 0.0f)
				move = 0.0f;
			if(_ignoreRightInput && move > 0.0f)
				move = 0.0f;
		}
		
		_anim.SetFloat("Speed", Mathf.Abs(move));
		
		rigidbody2D.velocity = new Vector2(move*MaxSpeed, rigidbody2D.velocity.y);
		//rigidbody2D.velocity = new Vector2(move*MaxSpeed, rigidbody2D.velocity.y);
		
		//don't slide down slopes
		if(IsOnRegularGround)
		{
			rigidbody2D.AddForce(new Vector2(0.0f, 30.0f));
		}
		
		if(move > 0 && !_facingRight)
		{
			Flip ();
		}
		else if(move < 0 && _facingRight)
		{
			Flip ();
		}
		
		_ignoreLeftInput = false;
		_ignoreRightInput = false;
		
		IsOnGround = false;
		IsOnSteepGround = false;
		IsOnRegularGround = false;
	}
	
	void Update()
	{
		/*
		if((_grounded || !_doubleJump) && Input.GetKeyDown(KeyCode.Space))
		{
			_anim.SetBool("Ground", false);
			//rigidbody2D.AddForce(new Vector2(0, JumpForce));
			var velocityY = rigidbody2D.velocity.y;
			if(velocityY < 0)
			{
				rigidbody2D.velocity = rigidbody2D.velocity + new Vector2(0, 7);
			}
			else if(velocityY < 7)
			{
				rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 7);
			}
			else
			{
			}
			
			if(!_doubleJump && !_grounded)
				_doubleJump = true;
		}
		*/
	}
	
	void Flip()
	{
		_facingRight = !_facingRight;
		
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	} 
	
	void OnCollisionEnter2D(Collision2D collision)
	{
		//foreach(ContactPoint2D contact in collision.contacts) {
		//Debug.DrawRay(contact.point, contact.normal, Color.green, 1, false);
		//}
		Slide (collision);
	}
	
	void OnCollisionExit2D(Collision2D collision)
	{
		//foreach(ContactPoint2D contact in collision.contacts) {
		//Debug.DrawRay(contact.point, contact.normal * 2, Color.white, 1, false);
		//}
	}
	
	
	void OnCollisionStay2D(Collision2D collision)
	{
		Slide (collision);
	}
	
	private void Slide(Collision2D collision)
	{
		foreach(ContactPoint2D contact in collision.contacts) {
			float angle = Vector2.Angle(contact.normal, Vector2.up);
			Vector3 cross = Vector3.Cross(contact.normal, Vector2.up);
			
			if(cross.z > 0)
				angle = 360 - angle;
			//Debug.Log(angle);
			
			if (angle < 40)
			{
				Debug.DrawRay(contact.point, -contact.normal, Color.red, 0.5f, false);
				IsOnRegularGround = true;
			}else if (angle >= 45 + offsetFrom45ForSlopes && angle < 90)
			{
				Debug.DrawRay(contact.point, -contact.normal, Color.cyan, 0.5f, false);
				_ignoreRightInput = true;
			}else if (angle >= 90 && angle < 135 + offsetFrom45ForSlopes)
			{
				Debug.DrawRay(contact.point, -contact.normal, Color.white, 0.5f, false);
				_ignoreRightInput = true;
				//rigidbody2D.AddForce(-transform.up * JumpForce * 0.1f);
			}
			else if (angle >= 135 + offsetFrom45ForSlopes && angle < 225 + offsetFrom45ForSlopes)
			{
				Debug.DrawRay(contact.point, -contact.normal, Color.blue, 0.5f, false);
				_ignoreRightInput = true;
			}
			else if (angle >= 225 + offsetFrom45ForSlopes && angle < 270)
			{
				Debug.DrawRay(contact.point, -contact.normal, Color.yellow, 0.5f, false);
				_ignoreLeftInput = true;
				//rigidbody2D.AddForce(-transform.up * JumpForce * 0.1f);
			}
			else if (angle >= 270 && angle < 315 - offsetFrom45ForSlopes)
			{
				Debug.DrawRay(contact.point, -contact.normal, Color.magenta, 0.5f, false);
				_ignoreLeftInput = true;
			}
			else
			{
				Debug.DrawRay(contact.point, -contact.normal, Color.green, 0.5f, false);
				IsOnRegularGround = true;
			}
			
			IsOnGround = true;
			if(_ignoreLeftInput || _ignoreRightInput)
			{
				IsOnSteepGround = true;
			}
			//rigidbody2D.AddForce(-transform.up * JumpForce * 0.1f);
		}
	}
}
