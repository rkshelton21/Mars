using UnityEngine;
using System.Collections;

public class CharController2D : MonoBehaviour {
	public Vector3 DebugVelocity = new Vector3();
	/**************************************************************/
	public bool UseForceMovement = false;
	protected Animator _anim;
	public AudioClip ImpactClip;
	protected Rigidbody2D _body = null;
	private SpriteRenderer _renderer;
	protected Color _teamColor = Color.white;

	public float MaxSpeed = 5.0f;
	public float JumpSpeed = 20.0f;
	protected float _move = 0.0f;
	public bool _facingRight = true;
	public bool _doubleJump = false;
	protected bool _ignoreLeftInput = false;
	protected bool _ignoreRightInput = false;
	private float _velocityModifier = 0.0f;

	public float AttackCooldown = 1.0f;
	public float _attackTimer = 0.0f;
	protected bool _attacking = false;
	public bool DamageTrigger = false;
	protected float _damageTimer = 0.0f;

	public float _TimeOffGround = 0f;
	protected float offsetFrom45ForSlopes = 10;
	private bool _IsOnRegularGround = false;
	private Vector3 _position_current;
	private Vector3 _position_previous;
	protected float _movement_speed;
	/**************************************************************/
	public virtual void ProcessInput(){}
	public virtual void Init(){}
	public virtual void Collide(Collision2D collision){}
	public virtual void Trigger(Collider2D collider){}
	//public virtual void Pre_FixedUpdate() {}
	public virtual void Pre_Update() {}
	public virtual void ApplyDamage(DamageDescription damage) {}
	public virtual void Turn() {}

	/**************************************************************/

	// Use this for initialization
	void Start () 
	{
		_body = GetComponentInChildren<Rigidbody2D>();
		_anim = GetComponentInChildren<Animator>();
		_renderer = transform.GetComponent<SpriteRenderer>();

		if(_renderer != null)
		{
			_renderer.color = _teamColor;
		}
		
		Init ();
	}

	public virtual void HandleOnTriggerEnter2D(Collider2D collider)
	{
		Trigger(collider);
	}

	public virtual void HandleOnCollisionEnter2D(Collision2D collision) 
	{
		Collide (collision);
	}

	public virtual void HandleOnCollisionStay2D(Collision2D collision)
	{
		Collide (collision);
	}

	public virtual void HandleOnCollisionExit2D(Collision2D collision) {}


	public virtual float HorizontalInput
	{
		get
		{
			return Input.GetAxis("Horizontal");
		}
		
		private set{}
	}

	public virtual bool JumpIsPressed
	{
		get
		{
			return (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W));
		}
		
		private set{}
	}

	public virtual bool IsGrounded
	{
		get
		{
			return _IsOnRegularGround;
		}
		
		set{}
	}

	public virtual void UpdateAnimations()
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
	}

	void PerformJumpIfRequired()
	{
		if((IsGrounded || !_doubleJump) && JumpIsPressed)
		{
			Debug.Log("Jump");
			var tempJumpSpeed = JumpSpeed;
			//set double jump flag if needed
			if(!_doubleJump && !IsGrounded)
				_doubleJump = true;
			
			//update animation
			_anim.SetBool("Ground", false);
			//if(_doubleJump)
			//	tempJumpSpeed *= 2;
			
			//_doubleJump = false;
			
			//don't exceed the jump speed
			var velocityY = _body.velocity.y;
			if(velocityY < 0)
			{
				_body.velocity = _body.velocity + new Vector2(0, tempJumpSpeed);
			}
			else if(velocityY < JumpSpeed)
			{
				_body.velocity = new Vector2(_body.velocity.x, tempJumpSpeed);
			}
			else
			{
			}
		}
	}

	public virtual void PerformTurnIfRequired()
	{

	}

	void Update()
	{
		ProcessInput ();
		//Set character input
		_move = HorizontalInput;	
		//Get movement input
		float appliedMovement = _move;

		//Adjust timers
		_attackTimer -= Time.deltaTime;
		_damageTimer -= Time.deltaTime;

		//Adjust ground flag
		if(IsGrounded)
			_doubleJump = false;
		
		PerformJumpIfRequired();
		PerformTurnIfRequired ();

		//Cannot move if input disabled by a steep slope
		if(_ignoreLeftInput && _move < 0.0f)
			appliedMovement = 0.0f;
		if(_ignoreRightInput && _move > 0.0f)
			appliedMovement = 0.0f;	
		
		//don't slide down slopes, if on regular ground, negate gravity
		if(IsGrounded)
		{
			//_body.AddForce(new Vector2(0.0f, 2.0f));
		}
		
		DebugVelocity = _body.velocity;

		if (UseForceMovement) 
		{
			/*float maxVelocityChange = 10.0f;
			// Calculate how fast we should be moving
			var targetVelocity = new Vector2(appliedMovement, 0);
			targetVelocity = transform.TransformDirection(targetVelocity);
			targetVelocity *= MaxSpeed;
			
			// Apply a force that attempts to reach our target velocity
			var velocity = _body.velocity;
			var velocityChange = (targetVelocity - velocity);
			velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
			velocityChange.y = _body.velocity.y;
			_body.velocity = velocityChange;*/

			/*var curV = _body.velocity;
			var targetV = new Vector2(appliedMovement*MaxSpeed, _body.velocity.y);
			var diffX = targetV.x - curV.x;
			var t = 0f;
			if(diffX < 0)
			{
				//t = diffX * MaxSpeed; 
				//t = (1 / (diffX / targetV.x)) * appliedMovement * MaxSpeed;
				//t = Mathf.Clamp(t, -MaxSpeed*10f, MaxSpeed*10f);
			}
			//DebugVelocity.x = t;
			//DebugVelocity.y = diffX;

			var newV = new Vector2(appliedMovement*MaxSpeed + t, _body.velocity.y);
			_body.velocity = newV;*/

			float velocityChange = 0.1f;
			// Calculate how fast we should be moving
			var targetSpeed = Mathf.Abs(appliedMovement*MaxSpeed);
			var currSpeed = _movement_speed;
			if(currSpeed < targetSpeed)
			{
				_velocityModifier += velocityChange * Time.deltaTime * appliedMovement;
			}
			if(currSpeed > targetSpeed)
			{
				_velocityModifier -= velocityChange * Time.deltaTime * appliedMovement;
			}

			_body.velocity = new Vector2(appliedMovement*MaxSpeed + _velocityModifier, _body.velocity.y);

			DebugVelocity.x = _velocityModifier;
			DebugVelocity.y = currSpeed;
			DebugVelocity.z = targetSpeed;
		}
		else
		{
			_body.velocity = new Vector2(appliedMovement*MaxSpeed, _body.velocity.y);
		}

		UpdateAnimations ();
		
		//reset flags
		_ignoreLeftInput = false;
		_ignoreRightInput = false;		
		//_IsOnGround = false;
		//_IsOnSteepGround = false;
		_IsOnRegularGround = false;
		_attacking = false;
		//GroundTest = IsGrounded;
	}

	void FixedUpdate()
	{
		_position_previous = _position_current;
		_position_current = _body.position;
		
		_movement_speed = Mathf.Abs(_position_current.x - _position_previous.x) * Time.fixedTime;
	}

	public void SetTeam(string teamName)
	{
		switch(teamName)		
		{
		case "Red":
			_teamColor = Color.red;
			break;
		case "Blue":
			_teamColor = Color.blue;
			break;
		default:
			_teamColor = Color.white;
			break;
		}

		if(_renderer != null)
		{
			_renderer.color = _teamColor;
		}
	}

	protected void Flip()
	{
		_facingRight = !_facingRight;
		
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	} 

	void OnTriggerEnter2D(Collider2D collider)
	{
		HandleOnTriggerEnter2D(collider);
	}
	
	void OnCollisionEnter2D(Collision2D collision)
	{
		HandleOnCollisionEnter2D(collision);
	}
	
	void OnCollisionStay2D(Collision2D collision)
	{
		HandleOnCollisionStay2D(collision);
	}

	void OnCollisionExit2D(Collision2D collision)
	{
		HandleOnCollisionExit2D(collision);
	}
}
