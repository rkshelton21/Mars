using UnityEngine;
using System.Collections;

public class Slug : AIController2D {

	BoxCollider2D _smallBoxCollider;
	BoxCollider2D _midBoxCollider;
	BoxCollider2D _largeBoxCollider;
	//private bool _deathTest = false;
	//private float _deathStage = 0;
	private float _roarStage = 0;

	public override void Init()
	{
		_healthBar = GetComponentInChildren<HealthBar>();
		_Player = GameObject.Find("Player").transform;
		_smallBoxCollider = transform.FindChild("SmallBox").GetComponent<BoxCollider2D>();
		_midBoxCollider = transform.FindChild("MidBox").GetComponent<BoxCollider2D>();
		_largeBoxCollider = transform.FindChild("LargeBox").GetComponent<BoxCollider2D>();
		_deathCollider = transform.FindChild("DeathCollider").GetComponent<BoxCollider2D>();

		_rand = new System.Random(System.Guid.NewGuid().GetHashCode());
		_boxCollider = GetComponent<BoxCollider2D>();
		_circleCollider = GetComponent<CircleCollider2D>();	
		_id = transform.GetInstanceID();

		_maxHealth = Health;
		if(_target == null)
		{
			_target = _Player;
		}
	}

	public override void Pre_Update()
	{
		if(_Player == null)
			return;
		var distance = (_Player.position - transform.position).magnitude;

		if(!Dying)
		{
			MaxSpeed = 0.05f;
			if( Mathf.Abs(distance) < 1)
			{
				MaxSpeed = 0.15f;				
				if(Mathf.Abs(distance) < 0.5)
				{
					MaxSpeed = 0.2f;
					_roarStage = 2;
					_anim.SetFloat("Roar_Stage", _roarStage);
					_smallBoxCollider.enabled = false;
					_midBoxCollider.enabled = false;
					_largeBoxCollider.enabled = true;
				}
				else
				{
					MaxSpeed = 0.15f;
					_roarStage = 1;
					_anim.SetFloat("Roar_Stage", _roarStage);
					_smallBoxCollider.enabled = false;
					_midBoxCollider.enabled = true;
					_largeBoxCollider.enabled = false;
				}
			}
			else
			{
				_roarStage = 0;
				_anim.SetFloat("Roar_Stage", _roarStage);
				_smallBoxCollider.enabled = true;
				_midBoxCollider.enabled = false;
				_largeBoxCollider.enabled = false;
			}
		}

		if(Dying)
		{
			//GetComponent<Rigidbody2D>().velocity = new Vector2();
			//_deathStage += Time.deltaTime*2;
			//_anim.SetFloat("Death_Stage", _deathStage);
		}
	}
	
	public override void Collide(Collision2D collision)
	{
		bool validTarget = (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Player");
		if(validTarget && collision.gameObject.layer != gameObject.layer && !Dying)
		{
			if(_attackTimer < 0)
			{
				//float p = ((float)_rand.Next(100))/100.0f * 10.0f;
				//_attackTimer = AttackCooldown + p * AttackCooldown;
				_attackTimer = AttackCooldown;
				
				_anim.SetTrigger("Attack");
			}
			else
			{
				var damageFlagTriggered = 1.0f;//_anim.GetFloat("ApplyMeleeDamage");
				if(collision.collider.GetType().ToString() == "UnityEngine.BoxCollider2D")
				{
					//Debug.Log("Box hit: " + damageFlagTriggered + ": " + _damageTimer);
				}
				else
				{
					return;
				}
				
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
						AttackDamage = 3,
						AttackDirectionIsRight = _facingRight,
						AttackerId = _id,
						AttackForce = _facingRight ? new Vector2(20, 20) : new Vector2(-20, 20)
					});
					_damageTimer = _attackTimer;
				}				
			}
		}			
	}

	/*
	public override void ApplyDamage(DamageDescription damage)
	{
		Health -= damage.AttackDamage;
		GetComponent<Rigidbody2D>().AddForce(damage.AttackForce);
		
		ObjectPooler.Current.Initialize(10, transform.position, damage.AttackForce);
		
		if(Health <= 0)
		{
			if(!Dying)
			{
				//_deathStage = _roarStage;
				
				//rigidbody2D.gravityScale = 0;
				
				_circleCollider.enabled = false;
				_boxCollider.enabled = false;
				_smallBoxCollider.enabled = false;
				_midBoxCollider.enabled = false;
				_largeBoxCollider.enabled = false;
				_deathCollider.enabled = true;

				Dying = true;
				_anim.SetTrigger("Die");
			}
			
			//_deathStage += Time.deltaTime*2;
			//_anim.SetFloat("Death_Stage", _deathStage);

			//Destroy(gameObject, 4f);
		}

		if(_healthBar != null)
		{
			_healthBar.SetHealth(Health / _maxHealth);
		}

		damage.Reflect(_id, 2.5f, 1.0f);
	}*/
}
