using UnityEngine;
using System.Collections.Generic;
using CurveExtended;

public class PlayerContoller2D : CharController2D {

	private Inventory _Inv;
	private HUD _Hud;
	private Bullet _Bullet_Primary;
	private Bullet _Bullet_Secondary;
	private Transform _Ammo_Primary;
	private Transform _Ammo_Secondary;

	private BoxCollider2D _boxCollider;
	private CircleCollider2D _circleCollider;
	private int _id;
	private bool _crouched = false;
	private float _maxFireRateCooldown = 0.25f;
	private float _fireCoolDown = 0.0f;
	private MuzzleFlare _muzzleFlash;

	private float Health = 10;
	private bool Dying = false;
	private PlayerHealth _healthBar;
	private float _maxHealth;

	private Dictionary<int, string> _leftGroundStop = new Dictionary<int, string>();
	private Dictionary<int, string> _rightGroundStop = new Dictionary<int, string>();
	private Dictionary<int, string> _normalGroundStop = new Dictionary<int, string>();


	public override void Init()
	{
		_maxHealth = Health;
		_Inv = GameObject.Find("Inventory").GetComponent<Inventory>();
		_Hud = GameObject.Find("HUD").GetComponent<HUD>();
		_healthBar = GameObject.Find("HUD").GetComponentInChildren<PlayerHealth>();

		//_Bullet = Bullet.transform.GetComponent<Bullet>();
		_muzzleFlash = transform.GetComponentInChildren<MuzzleFlare>();

		_boxCollider = GetComponent<BoxCollider2D>();
		_circleCollider = GetComponent<CircleCollider2D>();	
		_id = transform.GetInstanceID();

		/*
		Keyframe a = new Keyframe(0, 0);
		a.tangentMode = 0;
		Keyframe b = new Keyframe(1, 1);
		b.tangentMode = 0;

		var curve = new AnimationCurve();// (new Keyframe(0, 0, 0, 0), new Keyframe(1, 1, Mathf.Tan(Mathf.PI / 2), 0));
		curve.AddKey(KeyframeUtil.GetNew(0.5f, 0.0f, TangentMode.Linear)); //false on 0.5 second
		curve.AddKey(KeyframeUtil.GetNew(1.0f, 1.0f, TangentMode.Linear)); // true on 1.0 second

		var clips = UnityEditor.AnimationUtility.GetAnimationClips(_anim.gameObject);
		foreach(var c in clips)
		{
			Debug.Log(c.name);
			if(c.name == "Legs_Walk")
			{
				Debug.Log("Test");
				c.SetCurve("UpperBody/ForwardArm", transform.GetType(), "localPosition.x", curve);
			}
		}
		*/
	}

	public override float HorizontalInput
	{
		get
		{
			if(_leftGroundStop.Count > 0 || _rightGroundStop.Count > 0)
			{
				var result = Input.GetAxis("Horizontal");
				//if blocked on left input
				if(_leftGroundStop.Count > 0 && result < 0)
					return 0;
				//if blocked on right input
				if(_rightGroundStop.Count > 0 && result > 0)
					return 0;
			}
			
			if(!_crouched)
			{
				//if walking backward
				if(Input.GetAxis("Horizontal") < 0 && _facingRight || Input.GetAxis("Horizontal") > 0 && !_facingRight)
					return Input.GetAxis("Horizontal")/2f;

				return Input.GetAxis("Horizontal");
			}
			else
			{
				//if walking backward
				if(Input.GetAxis("Horizontal") < 0 && _facingRight || Input.GetAxis("Horizontal") > 0 && !_facingRight)
					return Input.GetAxis("Horizontal")/4f;

				return Input.GetAxis("Horizontal")/2f;
			}
		}
		
		private set{}
	}

	public override bool IsGrounded
	{
		get
		{
			return _normalGroundStop.Count > 0;
		}
		
		set{}
	}

	public override void ProcessInput()
	{
		bool shoot = false;
		bool toggleMenu = Input.GetKeyDown(KeyCode.E);
		bool swap = Input.GetKeyDown(KeyCode.Q);
		bool secondary = Input.GetKey(KeyCode.LeftShift);
		bool die = Input.GetKeyDown(KeyCode.X);
		_crouched = Input.GetKey(KeyCode.S);
		_fireCoolDown -= Time.deltaTime;

		if(toggleMenu)
		{
			_Inv.Toggle();
		}

		if(swap)		
		{
			SwapWeapons();
		}

		float fireInputX = (int)Input.GetAxis("Fire1");
		float fireInputY = (int)Input.GetAxis("Fire2");

		if(fireInputX != 0 || fireInputY != 0)
		{
			shoot = true;
			_attacking = true;
		}

		//Face direction of shooting
		if(fireInputX > 0 && !_facingRight)
		{
			Flip ();
		}
		else if(fireInputX < 0 && _facingRight)
		{
			Flip ();
		}

		if(_facingRight)
		{
			//_anim.SetFloat("AimDir", fireInputY - fireInputX / 2.0f);
			_anim.SetFloat("AimDirX", fireInputX);
			_anim.SetFloat("AimDirY", fireInputY);
		}
		else
		{
			//_anim.SetFloat("AimDir", fireInputY - (-fireInputX) / 2.0f);
			_anim.SetFloat("AimDirX", fireInputX);
			_anim.SetFloat("AimDirY", fireInputY);
		}

		if(shoot && _fireCoolDown < 0)
		{
			if(_Inv != null)
			{
				_fireCoolDown = _maxFireRateCooldown;
				if(!secondary && _Inv._primaryWeapon.ItemName != "" && _Ammo_Primary != null)
				{
					_anim.SetTrigger("Attack");
					float force = _Bullet_Primary.ShotPower.magnitude;

					//Vector2 shotDir = new Vector2(fireInputX, Mathf.Max(fireInputY, 0));
					//Vector2 shotDir = new Vector2(fireInputX, fireInputY);
					Vector2 shotDir = GetShotDirection(fireInputX, fireInputY, _facingRight, _crouched);

					shotDir.Normalize();
					_Bullet_Primary.ShotForce = shotDir * force;

					var offset_xy = GetShotOffset(fireInputX, fireInputY, _facingRight, _crouched);
					var offset = new Vector3(offset_xy.x, offset_xy.y, 0.0f);

					Instantiate(_Ammo_Primary, transform.position + offset, Quaternion.identity);

					_muzzleFlash.Fire(shotDir);
					_body.AddForce( shotDir * -100f);
				}

				if(secondary && _Inv._secondaryWeapon.ItemName != "" && _Ammo_Secondary != null)
				{
					_anim.SetTrigger("Attack");
					float force = _Bullet_Secondary.ShotPower.magnitude;
					
					//Vector2 shotDir = new Vector2(fireInputX, Mathf.Max(fireInputY, 0));
					//Vector2 shotDir = new Vector2(fireInputX, fireInputY);
					Vector2 shotDir = GetShotDirection(fireInputX, fireInputY, _facingRight, _crouched);
					
					shotDir.Normalize();
					_Bullet_Secondary.ShotForce = shotDir * force;

					var offset_xy = GetShotOffset(fireInputX, fireInputY, _facingRight, _crouched);
					var offset = new Vector3(offset_xy.x, offset_xy.y, 0.0f);
					
					
					Instantiate(_Ammo_Secondary, transform.position + offset, Quaternion.identity);
					_muzzleFlash.Fire(shotDir);
					_body.AddForce( shotDir * -100f);
				}
			}
		}

		if(_crouched)
		{
			//_anim.SetBool("Crouched", true);
		}
		else
		{
			//_anim.SetBool("Crouched", false);
		}

		if(die)
		{
			_anim.SetBool("Dying", true);
			_anim.SetTrigger("Die");
			_boxCollider.enabled = false;
			_circleCollider.enabled = false;
			rigidbody2D.gravityScale = 0f;
			Destroy(gameObject, 0.75f);
		}
	}

	public override void PerformTurnIfRequired()
	{
		if(_move > 0 && !_facingRight && !_attacking)
		{
			Flip ();
		}
		else if(_move < 0 && _facingRight && !_attacking)
		{
			Flip ();
		}
	}

	public override void UpdateAnimations()
	{
		if(IsGrounded)
		{
			_TimeOffGround = 0f;
		}
		else
		{
			_TimeOffGround += Time.deltaTime;
		}
		
		if(_TimeOffGround > 0.25f && !IsGrounded)
		{
			//update animation variables
			_anim.SetBool("Ground", IsGrounded);
		}
		else
		{
			//update animation variables
			_anim.SetBool("Ground", true);
		}
		
		_anim.SetFloat("verticalSpeed", _body.velocity.y);
		//_anim.SetFloat("Speed", Mathf.Abs(_body.));
		_anim.SetFloat("TimeInAir", _TimeOffGround);
		
		var cur_speed = Mathf.Abs(_move);
		var notWalking = cur_speed < 0.01f;
		var sliding = notWalking && IsGrounded && Mathf.Abs (_movement_speed) > 0.001f;
		
		//if sliding
		if(sliding)
		{
			if(_facingRight)
			{
				if(_movement_speed > 0)
				{
					_anim.SetFloat("Sliding", 4.0f); //Slide Right Facing Right
				}
				else
				{
					_anim.SetFloat("Sliding", 2.0f); //Slide Left Facing Right
				}
			}
			else
			{
				if(_movement_speed > 0)
				{
					_anim.SetFloat("Sliding", 3.0f); //Slide Right Facing Left
				}
				else
				{
					_anim.SetFloat("Sliding", 1.0f); //Slide Left Facing Left
				}
			}
			
			//set current speed manually
			//cur_speed = _movement_speed;
			//DebugVelocity.x = cur_speed;
		}
		else
		{
			_anim.SetFloat("Sliding", 0.0f);
		}
		
		_anim.SetFloat("Speed", cur_speed);
	}

	private void SwapWeapons()
	{
		if(_Bullet_Primary != null && _Bullet_Secondary != null)
		{
			var success = _Inv.SwapWeapons();
			if(success)
			{
				var tempAmmo = _Ammo_Primary;
				var tempBullet = _Bullet_Primary;

				_Ammo_Primary = _Ammo_Secondary;
				_Bullet_Primary = _Bullet_Secondary;
				_Ammo_Secondary = tempAmmo;
				_Bullet_Secondary = tempBullet;
			}
		}
	}

	public void ConsumeItem(string Item)
	{
		if(Item.Contains("Gun"))
		{
			_anim.SetTrigger("WeaponArmed");
			_anim.SetBool("HasGun", true);

			if(_Ammo_Primary == null)
			{
				_Inv.GetAmmo(Item, out _Ammo_Primary, out _Bullet_Primary);
				_Inv.Equip(0, string.Empty, Item);
			}
			else if(_Ammo_Secondary == null)
			{
				_Inv.GetAmmo(Item, out _Ammo_Secondary, out _Bullet_Secondary);
				_Inv.Equip(1, string.Empty, Item);
			}
		}

		if(Item == "Heart")
		{
			Health = _maxHealth;
			_healthBar.Health = (Health / _maxHealth);
		}

		//Debug.Log("Nom nom: " + Item);
		_Inv.AddItem(Item);
	}

	public override void ApplyDamage(DamageDescription damage)
	{
		//Debug.Log("Damage Applied");
		_attackTimer = AttackCooldown;
		//This shouldn't be used here I don't think
		//_damageTimer = _attackTimer;
		
		//Debug.Log("Taking damage from: " + damage.AttackerId + " for " + damage.AttackDamage + " dmg");
		Health -= damage.AttackDamage;
		int direction = damage.AttackDirectionIsRight ? 1 : -1;
		rigidbody2D.AddForce(damage.AttackForce);
		//rigidbody2D.AddForce(new Vector2(2000 * direction, 200));
		//rigidbody2D.velocity = new Vector2(damage.AttackForce.x*100, damage.AttackForce.y);
		//rigidbody2D.velocity = new Vector2(-50000, JumpSpeed);
		
		if(Health <= 0)
		{
			if(!Dying)
			{
				_circleCollider.enabled = false;
				_boxCollider.enabled = false;
				//_deathCollider.enabled = true;
				
				Dying = true;
			}
			
			_anim.SetBool("Dying", true);
			_anim.SetTrigger("Die");
			
			//_overlapCollider.enabled = false;
			_boxCollider.enabled = false;
			_circleCollider.enabled = false;
			rigidbody2D.gravityScale = 0f;
			rigidbody2D.velocity = new Vector2();

			Destroy(gameObject, 0.75f);
		}
		else
		{
			_anim.SetTrigger("Hit");
		}
		
		damage.Reflect(_id, 2.5f, 1.0f);
		if(_healthBar != null)
		{
			_healthBar.Health = (Health / _maxHealth);
			//Debug.Log(Health + " of " + _maxHealth + " = " + _healthBar.Health);
		}
	}

	public Vector2 GetShotOffset(float inputX, float inputY, bool facingRight, bool crouched)
	{
		var offset = new Vector2();
		if(inputX > -0.5 && inputX < 0.5 && inputY > 0.5)//up
		{
			offset = new Vector2();//Vector2(-0.05f, 0.0f);
		}
		else if((inputX < -0.5 || inputX > 0.5) && inputY > 0.5)//up angle
		{
			offset = new Vector2();//Vector2(0f, -0.2f);
		}
		else if((inputX < -0.5 || inputX > 0.5) && (inputY > -0.5 && inputY < 0.5))//norm
		{
			offset = new Vector2();//Vector2(0.35f, -0.5f);
		}
		else if((inputX < -0.5 || inputX > 0.5) && (inputY < -0.5 || inputY > 0.5))//down angle
		{
			offset = new Vector2();//Vector2(0.1f, -0.6f);
		}
		else if(inputX > -0.5 && inputX < 0.5 && inputY < -0.5)//down
		{
			offset = new Vector2();//Vector2(0.0f, -1f);
		}

		if(!_facingRight)
			offset.x = -offset.x;
		
		if (_crouched) {
			offset.y -= 0.3f;
		} else {
			offset.y -= 0.1f;
		}

		return offset;
	}

	public Vector2 GetShotDirection(float inputX, float inputY, bool facingRight, bool crouched)
	{
		var dir = new Vector2();
		if(inputX > -0.5 && inputX < 0.5 && inputY > 0.5)//up
		{
			dir = new Vector2(0.0f, 1.0f);
		}
		else if((inputX < -0.5 || inputX > 0.5) && inputY > 0.5)//up angle
		{
			dir = new Vector2(1.0f, 1.0f);
		}
		else if((inputX < -0.5 || inputX > 0.5) && (inputY > -0.5 && inputY < 0.5))//norm
		{
			dir = new Vector2(1.0f, 0.0f);
		}
		else if((inputX < -0.5 || inputX > 0.5) && (inputY < -0.5 || inputY > 0.5))//down angle
		{
			dir = new Vector2(0.66f, -0.33f);
		}
		else if(inputX > -0.5 && inputX < 0.5 && inputY < -0.5)//down
		{
			dir = new Vector2(0.0f, -1f);
		}
		
		if(!_facingRight)
			dir.x = -dir.x;
		
		if(_crouched)
		{
			//
		}
		
		return dir;
	}

	public override void HandleOnTriggerEnter2D(Collider2D collider)
	{
		Trigger(collider);
	}
	
	public override void HandleOnCollisionEnter2D(Collision2D collision) 
	{
		AddCollisionSurfaces(collision);
		Collide (collision);
	}
	
	public override void HandleOnCollisionStay2D(Collision2D collision)
	{
		AddCollisionSurfaces(collision);
		Collide (collision);
	}
	
	public override void HandleOnCollisionExit2D(Collision2D collision)
	{
		RemoveCollisionSurfaces(collision);
	}

	private void AddCollisionSurfaces(Collision2D collision)
	{
		var id = collision.collider.GetInstanceID();

		foreach(ContactPoint2D contact in collision.contacts) 
		{
			float angle = Vector2.Angle(contact.normal, Vector2.up);
			Vector3 cross = Vector3.Cross(contact.normal, Vector2.up);
			
			if(cross.z > 0)
				angle = 360 - angle;
			
			if (angle < 40)
			{
				Debug.DrawRay(contact.point, -contact.normal, Color.red, 0.5f, false);
				if(!_normalGroundStop.ContainsKey(id))
				{
					_normalGroundStop.Add(id, "Test");
				}
			}else if (angle >= 45 + offsetFrom45ForSlopes && angle < 90)
			{
				Debug.DrawRay(contact.point, -contact.normal, Color.cyan, 0.5f, false);
				_ignoreRightInput = true;
				if(!_rightGroundStop.ContainsKey(id))
				{
					_rightGroundStop.Add(id, "Test");
				}
			}else if (angle >= 90 && angle < 135 + offsetFrom45ForSlopes)
			{
				Debug.DrawRay(contact.point, -contact.normal, Color.white, 0.5f, false);
				_ignoreRightInput = true;
				if(!_rightGroundStop.ContainsKey(id))
				{
					_rightGroundStop.Add(id, "Test");
				}
			}
			else if (angle >= 135 + offsetFrom45ForSlopes && angle < 225 + offsetFrom45ForSlopes)
			{
				Debug.DrawRay(contact.point, -contact.normal, Color.blue, 0.5f, false);
				_ignoreRightInput = true;
				if(!_rightGroundStop.ContainsKey(id))
				{
					_rightGroundStop.Add(id, "Test");
				}
			}
			else if (angle >= 225 + offsetFrom45ForSlopes && angle < 270)
			{
				Debug.DrawRay(contact.point, -contact.normal, Color.yellow, 0.5f, false);
				_ignoreLeftInput = true;
				if(!_leftGroundStop.ContainsKey(id))
				{
					_leftGroundStop.Add(id, "Test");
				}
			}
			else if (angle >= 270 && angle < 315 - offsetFrom45ForSlopes)
			{
				Debug.DrawRay(contact.point, -contact.normal, Color.magenta, 0.5f, false);
				_ignoreLeftInput = true;
				if(!_leftGroundStop.ContainsKey(id))
				{
					_leftGroundStop.Add(id, "Test");
				}
			}
			else
			{
				Debug.DrawRay(contact.point, -contact.normal, Color.green, 0.5f, false);
				if(!_normalGroundStop.ContainsKey(id))
				{
					_normalGroundStop.Add(id, "Test");
				}
			}
			
			//_IsOnGround = true;
			if(_ignoreLeftInput || _ignoreRightInput)
			{
				//_IsOnSteepGround = true;
			}
		}

		//T1 = _normalGroundStop.Count;
		//T2 = _leftGroundStop.Count;
		//T3 = _rightGroundStop.Count;
	}

	private void RemoveCollisionSurfaces(Collision2D collision)
	{
		//T1 = _normalGroundStop.Count;
		//T2 = _leftGroundStop.Count;
		//T3 = _rightGroundStop.Count;

		var id = collision.collider.GetInstanceID();
		//Debug.Log("Called " + id);

		_normalGroundStop.Remove(id);
		_leftGroundStop.Remove(id);
		_rightGroundStop.Remove(id);

		//T1 = _normalGroundStop.Count;
		//T2 = _leftGroundStop.Count;
		//T3 = _rightGroundStop.Count;
	}
}
