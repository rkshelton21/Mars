﻿using UnityEngine;
using System.Collections;

public class AIController2D : CharController2D {
	public bool DestroyOnDeath = false;
	protected Transform _Player = null;
	protected BoxCollider2D _boxCollider;
	protected BoxCollider2D _deathCollider;
	protected BoxCollider2D _overlapCollider;
	protected CircleCollider2D[] _circleColliders;
	protected int _id;
	protected float Health = 10;
	protected bool Dying = false;
	protected HealthBar _healthBar;
	protected float _maxHealth;
	protected Transform _target;
	protected System.Random _rand;
	protected float MaxTargetDistance = 0.5f;
	protected float _maxAudioDistance = 0.5f;

	public override void Init()
	{
		_healthBar = GetComponentInChildren<HealthBar>();
		_boxCollider = GetComponent<BoxCollider2D>();
		_circleColliders = GetComponents<CircleCollider2D>();	
		_overlapCollider = transform.FindChild("Overlap Collider").GetComponent<BoxCollider2D>();
		_deathCollider = transform.FindChild("DeathCollider").GetComponent<BoxCollider2D>();

		var playerObject = GameObject.Find("Player");
		if(playerObject != null)
		{
			_Player = GameObject.Find("Player").transform;
		}
		_id = transform.GetInstanceID();
		_rand = new System.Random(System.Guid.NewGuid().GetHashCode());

		_maxHealth = Health;
		if(_target == null)
		{
			_target = _Player;
		}

		var audioSources = GetComponents<AudioSource> ();
		if (audioSources.GetValue(0) != null) 
		{
			// Just to make sure we are
			// going to disable the play
			// on awake for the AudioSource
			audioSources[0].playOnAwake = false;

			// Make sure there is an
			// AudioClip to play before
			// continuing 
			if (audioSources[0].clip != null) 
			{
				// Get a random number between
				// our min and max delays
				float delayTime = Random.Range (0, 1);
				// Play the audio with a delay
				audioSources[0].PlayDelayed (delayTime);
				audioSources[0].pitch = Random.Range (0.8f, 1.8f);
			}
		}
	}

	public void SetTarget(Transform newTarget)
	{
		_target = newTarget;
	}

	public override float HorizontalInput
	{
		get
		{
			if(_target == null)
			{
				//no target
				return 0;
			}
			else
			{
				if(!Dying)
				{
					float distance = (_target.position - transform.position).magnitude;
					float absDistance = Mathf.Abs(distance);
					var x = _target.position.x - transform.position.x;
					float direction = x / Mathf.Abs(x);
					//DebugVelocity.x = absDistance;
					//DebugVelocity.y = -2;
					//too far, just wander
					if(absDistance > MaxTargetDistance)
					{
						//DebugVelocity.y = -1;
						if(_facingRight)
							return 1;
						else 
							return -1;
					}
					else
					{
						if(_facingRight && direction < 0)
							Flip();
						if(!_facingRight && direction > 0)
							Flip();

						//DebugVelocity.y = 1;
						return direction;
					}
				}
				else
				{
					return 0;
				}
			}
		}		
	}

	public override void UpdateAnimations()
	{
		_anim.SetFloat("Speed", Mathf.Abs(_move));
	}

	public override void UpdateSound()
	{
		if (_Player != null) 
		{
			var d = _Player.position - transform.position;
			if(GetComponent<AudioSource>() != null)
			{
				GetComponent<AudioSource>().volume = _maxAudioDistance / d.magnitude;
			}
		}
	}

	public override void Turn(bool[] forceDirection)
	{
		//Debug.Log ("Turn: " + System.Environment.StackTrace );
		if(_target != null)
		{
			float distance = (_target.position - transform.position).magnitude;
			float absDistance = Mathf.Abs(distance);

			//close enough to keep on target
			if(absDistance < MaxTargetDistance)
			{
				Debug.Log ("Turn: No");
				return;
			}
		}
		
		//ignore if already going the right direction
		//force left
		if(forceDirection[0] && !_facingRight)
			return;
		//force right
		if(forceDirection[1] && _facingRight)
			return;
		
		Flip ();
	}

	public override bool IsGrounded
	{
		get
		{
			return true;
		}
	}

	public override bool JumpIsPressed
	{
		get
		{
			return false;
		}		
	}

	public override void Collide(Collision2D collision)
	{
		/*
		if(collision.gameObject.tag != "Untagged" && collision.gameObject.tag != "Player")
		{
			Debug.Log("Collision:");
			Debug.Log("    " + collision.gameObject.tag);
			Debug.Log("    " + collision.gameObject.layer);
			Debug.Log("    " + Dying);
			Debug.Log("    " + _attackTimer);
		}
		*/

		//if(_teamColor != Color.red)
		//	return;

		bool validTarget = (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Player");
		if(validTarget && collision.gameObject.layer != gameObject.layer && !Dying)
		{
			if(_attackTimer < 0)
			{
				//float p = ((float)_rand.Next(1000))/10000.0f * 10.0f;
				float p = ((float)_rand.Next(100))/100.0f * 10.0f;

				_attackTimer = AttackCooldown + p * AttackCooldown;
				_anim.SetTrigger("Attack");
			}
			else
			{
				var damageFlagTriggered = _anim.GetFloat("ApplyMeleeDamage");
				if(damageFlagTriggered >= 1.0f)
				{
					//Debug.Log("Attack Trigger");
					DamageTrigger = true;
				}
				else
				{
					//Debug.Log("Attack Trigger Off");
					DamageTrigger = false;
				}

				if(DamageTrigger && _damageTimer < 0)
				{
					collision.gameObject.SendMessage("ApplyDamage", new DamageDescription(){
						AttackDamage = 1,
						AttackDirectionIsRight = _facingRight,
						AttackerId = _id,
						AttackForce = _facingRight ? new Vector2(20, 20) : new Vector2(-20, 20)
					});
					_damageTimer = _attackTimer;
				}				
			}
		}
	}

	public override void ApplyDamage(DamageDescription damage)
	{
		if(Dying)
			return;
			
		_attackTimer = AttackCooldown;

		if (ImpactClip != null) 
		{
			ImpactSource.PlayOneShot(ImpactClip);
			//AudioSource.PlayClipAtPoint (ImpactClip, transform.position);
		}

		//This shouldn't be used here I don't think
		//_damageTimer = _attackTimer;

		//Debug.Log("Taking damage from: " + damage.AttackerId + " for " + damage.AttackDamage + " dmg");
		Health -= damage.AttackDamage;
		int direction = damage.AttackDirectionIsRight ? 1 : -1;
		GetComponent<Rigidbody2D>().AddForce(new Vector2(2 * direction, 2));
		//rigidbody2D.velocity = new Vector2(damage.AttackForce.x*100, damage.AttackForce.y);
		//rigidbody2D.velocity = new Vector2(-50000, JumpSpeed);

		ObjectPooler.Current.Initialize("Block", 10, transform.position, new Vector4[]{ damage.AttackForce, ParticleColor });	
		
		if(Health <= 0)
		{
			if(!Dying)
			{
				for(int i=0; i<_circleColliders.Length; i++)
				{
					_circleColliders[i].enabled = false;
				}
				_boxCollider.enabled = false;
				_deathCollider.enabled = true;
				_healthBar.transform.gameObject.SetActive(false);

				Dying = true;
			}

			_anim.SetBool("Dying", true);
			_anim.SetTrigger("Die");

			if(_overlapCollider != null)
			{
				_overlapCollider.enabled = false;
			}
			if(_boxCollider != null)
			{
				_boxCollider.enabled = false;
			}
			if(_circleColliders != null)
			{
				for(int i=0; i<_circleColliders.Length; i++)
				{
					_circleColliders[i].enabled = false;
				}				
			}
			//rigidbody2D.gravityScale = 0f;

			this.enabled = false;

			if(DestroyOnDeath)
			{
				Destroy(gameObject, 1.75f);
				Debug.Log("Destroy");
			}
			else
			{
				Debug.Log("Dead Body");
				ObjectPooler.Current.Initialize("DeadBody", 1, transform.position, new DeadBodyParams(){ BodyRenderer = _renderer, Countdown = 1.5f});
			}
		}
		else
		{
			if(_anim != null)
			{
				_anim.SetTrigger("Hit");			
			}
		}

		damage.Reflect(_id, 2.5f, 1.0f);
		if(_healthBar != null)
		{
			_healthBar.SetHealth(Health / _maxHealth);			
		}
	}

	/*
	public override void Trigger(Collider2D collider)
	{
		Debug.Log("Collider " + collider.name + collider.tag);
		if(collider.tag == "Bullet")
		{
			Destroy(gameObject);
		}
	}
	*/
	
	public override void HandleOnCollisionStay2D (Collision2D collision)
	{
		if (collision.collider.tag != "Ground")
			return;
		var contact = collision.contacts[0];
		var n = collision.contacts[0].normal;
		//Debug.DrawRay(contact.point, contact.normal, Color.white, 0.5f, false);
		_contact_normal = n;
	}
}
