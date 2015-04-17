using UnityEngine;
using System.Collections.Generic;
using CurveExtended;

public class PlayerContoller2D : CharController2D {

	private Inventory _Inv;
	private HUD _Hud;
	private Bullet _Bullet_Primary;
	private Bullet _Bullet_Secondary;
	
	private BoxCollider2D _boxCollider;
	private CircleCollider2D _circleCollider;
	private int _id;
	private bool _crouched = false;
	private float _maxFireRateCooldown = 0.25f;
	private float _fireCoolDown = 0.0f;
	private float _flashCoolDown = 0.0f;
	private float _maxFlashCoolDown = 0.1f;
	private SpriteRenderer _muzzleFlash;

	private float Health = 10;
	private bool Dying = false;
	private PlayerHealth _healthBar;
	private float _maxHealth;

	private Dictionary<int, string> _leftGroundStop = new Dictionary<int, string>();
	private Dictionary<int, string> _rightGroundStop = new Dictionary<int, string>();
	private Dictionary<int, string> _normalGroundStop = new Dictionary<int, string>();
	
	private CameraController _cam;
	
	public override void Init()
	{
		_anim.SetTrigger("Spawn");
		_maxHealth = Health;
		_Inv = GameObject.Find("Inventory").GetComponent<Inventory>();
		_Hud = GameObject.Find("HUD").GetComponent<HUD>();
		_healthBar = GameObject.Find("HUD").GetComponentInChildren<PlayerHealth>();
		_cam = GameObject.Find("Main Camera").GetComponent<CameraController>();
		
		//_Bullet = Bullet.transform.GetComponent<Bullet>();
		_muzzleFlash = _anim.transform.FindChild("Flash").GetComponent<SpriteRenderer>();

		_boxCollider = GetComponent<BoxCollider2D>();
		_circleCollider = GetComponent<CircleCollider2D>();	
		_id = transform.GetInstanceID();					
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
		if(Input.GetKeyDown(KeyCode.L))
		{
			PlayerData.LoadGame();
			Debug.Log("Loaded");
		}
		if(Input.GetKeyDown(KeyCode.K))
		{
			PlayerData.SaveGame();
			Debug.Log("Saved");
		}
		if(Input.GetKeyDown(KeyCode.J))
		{
			PlayerData.NewGame();
			Debug.Log("Saved New");
		}
		if(Input.GetKeyDown(KeyCode.H))
		{
			GUIController.ShowHUD();
		}
		
		bool shoot = false;
		bool toggleMenu = Input.GetKeyDown(KeyCode.E);
		bool toggleConsole = Input.GetKeyDown(KeyCode.C);
		bool swap = Input.GetKeyDown(KeyCode.Q);
		bool secondary = Input.GetKey(KeyCode.LeftShift);
		bool die = Input.GetKeyDown(KeyCode.X);
		_crouched = Input.GetKey(KeyCode.S);
		_fireCoolDown -= Time.deltaTime;

		if(toggleMenu)
		{
			_Inv.Toggle();
		}
		
		if(toggleConsole)
		{
			if(GUIController.CONActive)
			{
				GUIController.ShowHUD();
			}
			else
			{
				GUIController.ShowConsole();
			}
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

		if(shoot)
		{
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
		}
		
		
		if(shoot && _fireCoolDown < 0)
		{
			if(_Inv != null)
			{
				_fireCoolDown = _maxFireRateCooldown;
				if(!secondary && _Inv._primaryWeapon.ItemName != "" && _Bullet_Primary != null)
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

					Instantiate(_Bullet_Primary.transform, transform.position + offset, Quaternion.identity);

					_muzzleFlash.enabled = true;
					_flashCoolDown = _maxFlashCoolDown;
					_body.AddForce( shotDir * -_Bullet_Primary.ShotForce.magnitude / 10f);									
				}

				if(secondary && _Inv._secondaryWeapon.ItemName != "" && _Bullet_Secondary != null)
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
					
					
					Instantiate(_Bullet_Secondary.transform, transform.position + offset, Quaternion.identity);
					
					_muzzleFlash.enabled = true;
					_flashCoolDown = _maxFlashCoolDown;
					_body.AddForce( shotDir * -_Bullet_Secondary.ShotForce.magnitude / 10f);
				}
			}
		}

		if(_crouched)
		{
			_anim.SetBool("Crouched", true);
		}
		else
		{
			_anim.SetBool("Crouched", false);
		}

		if(die)
		{
			_anim.SetBool("Dying", true);
			_anim.SetTrigger("Die");
			_boxCollider.enabled = false;
			_circleCollider.enabled = false;
			//GetComponent<Rigidbody2D>().gravityScale = 0f;
			Destroy(gameObject, 0.75f);
		}
		
		_flashCoolDown -= Time.deltaTime;
		if(_flashCoolDown < 0)
		{
			_muzzleFlash.enabled = false;
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
		_anim.SetFloat("AttackCooldown", _fireCoolDown);

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
		_Inv.SwapWeapons();		
	}

	public void ConsumeItem(string Item)
	{
		if(Item.Contains("Gun"))
		{
			_anim.SetTrigger("WeaponArmed");
			_anim.SetBool("HasGun", true);
		}

		if(Item == "Heart")
		{
			Health = _maxHealth;
			_healthBar.Health = (Health / _maxHealth);
		}

		//Debug.Log("Nom nom: " + Item);
		_Inv.AddItem(Item);
	}
	
	public void SetInfo(PlayerInfo info)
	{
		_Inv.Clear();
		
		Debug.Log("Player Loading");
		if(info.HasWeapon)
		{
			Debug.Log("Player Has Weapon");
			_anim.SetTrigger("WeaponArmed");
			_anim.SetBool("HasGun", true);
			
			if(info.PrimaryWeapon != "")
			{
				Debug.Log("Player Armed Primary " + info.PrimaryWeapon);
				//_Inv.GetAmmo(info.PrimaryWeapon, out _Ammo_Primary, out _Bullet_Primary);
				//_Inv.Equip(0, string.Empty, info.PrimaryWeapon);				
			}
			if(info.SecondaryWeapon != "")
			{
				Debug.Log("Player Armed Secondary " + info.SecondaryWeapon);
				//_Inv.GetAmmo(info.SecondaryWeapon, out _Ammo_Secondary, out _Bullet_Secondary);
				//_Inv.Equip(1, string.Empty, info.SecondaryWeapon);
			}
		}
		
		Health = info.Health * _maxHealth;
		_healthBar.Health = info.Health;		
	}
	
	public PlayerInfo GetInfo()
	{
		PlayerInfo info = new PlayerInfo();
		info.HasWeapon = _anim.GetBool("HasGun");
		info.Health = _healthBar.Health;
		
		return info;
	}
	
	public void SetPrimaryAmmo(Bullet bullet)
	{
		_Bullet_Primary = bullet;
	}
	
	public void SetSecondaryAmmo(Bullet bullet)
	{
		_Bullet_Secondary = bullet;
	}

	public override void ApplyDamage(DamageDescription damage)
	{
		_cam.Shake = true;
		//Debug.Log("Damage Applied");
		_attackTimer = AttackCooldown;
		//This shouldn't be used here I don't think
		//_damageTimer = _attackTimer;
		
		//Debug.Log("Taking damage from: " + damage.AttackerId + " for " + damage.AttackDamage + " dmg");
		Health -= damage.AttackDamage;
		//int direction = damage.AttackDirectionIsRight ? 1 : -1;
		GetComponent<Rigidbody2D>().AddForce(damage.AttackForce);
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
			//GetComponent<Rigidbody2D>().gravityScale = 0f;
			GetComponent<Rigidbody2D>().velocity = new Vector2();

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
			offset.y -= 0.1f;
		} else {
			offset.y -= 0.05f;
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
		if (collision.collider.tag != "Ground")
			return;

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
