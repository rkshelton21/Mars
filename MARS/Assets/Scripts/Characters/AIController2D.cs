using UnityEngine;
using System.Collections;

public class AIController2D : CharController2D {
	protected Transform _Player = null;
	protected BoxCollider2D _boxCollider;
	protected BoxCollider2D _deathCollider;
	protected BoxCollider2D _overlapCollider;
	protected CircleCollider2D _circleCollider;
	protected int _id;
	protected float Health = 10;
	protected bool Dying = false;
	protected HealthBar _healthBar;
	protected float _maxHealth;
	protected Transform _target;
	protected System.Random _rand;

	public override void Init()
	{
		_healthBar = GetComponentInChildren<HealthBar>();
		_boxCollider = GetComponent<BoxCollider2D>();
		_circleCollider = GetComponent<CircleCollider2D>();	
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
					float distance = _target.position.x - transform.position.x;
					float absDistance = Mathf.Abs(distance);
					float direction = distance / absDistance;

					//too far, just wander
					if(absDistance > 5f)
					{
						if(_facingRight)
							return 1;
						else 
							return -1;
					}
					else
					{
						return direction;
					}
				}
				else
				{
					return 0;
				}
			}
		}
		
		private set{}
	}

	public override void Turn()
	{
		if(_target != null)
		{
			float distance = _target.position.x - transform.position.x;
			float absDistance = Mathf.Abs(distance);

			//close enough to keep on target
			if(absDistance < 5f)
			{
				return;
			}
		}

		_facingRight = !_facingRight;
	}


	public override bool JumpIsPressed
	{
		get
		{
			return false;
		}
		
		private set{}
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
						AttackDamage = 5,
						AttackDirectionIsRight = _facingRight,
						AttackerId = _id,
						AttackForce = new Vector2(2000, 2000)
					});
					_damageTimer = _attackTimer;
				}				
			}
		}


	}

	public override void ApplyDamage(DamageDescription damage)
	{
		//Debug.Log("Damage Applied");
		_attackTimer = AttackCooldown;

		AudioSource.PlayClipAtPoint(ImpactClip, transform.position);

		//This shouldn't be used here I don't think
		//_damageTimer = _attackTimer;

		//Debug.Log("Taking damage from: " + damage.AttackerId + " for " + damage.AttackDamage + " dmg");
		Health -= damage.AttackDamage;
		int direction = damage.AttackDirectionIsRight ? 1 : -1;
		rigidbody2D.AddForce(new Vector2(2000 * direction, 200));
		//rigidbody2D.velocity = new Vector2(damage.AttackForce.x*100, damage.AttackForce.y);
		//rigidbody2D.velocity = new Vector2(-50000, JumpSpeed);

		if(Health <= 0)
		{
			if(!Dying)
			{
				_circleCollider.enabled = false;
				_boxCollider.enabled = false;
				_deathCollider.enabled = true;
				
				Dying = true;
			}

			_anim.SetBool("Dying", true);
			_anim.SetTrigger("Die");

			_overlapCollider.enabled = false;
			_boxCollider.enabled = false;
			_circleCollider.enabled = false;
			//rigidbody2D.gravityScale = 0f;

			Destroy(gameObject, 0.75f);
		}
		else
		{
			_anim.SetTrigger("Hit");
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
}
