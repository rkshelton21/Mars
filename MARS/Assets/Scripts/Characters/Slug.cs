using UnityEngine;
using System.Collections;

public class Slug : AIController2D {

	BoxCollider2D _midBoxCollider;
	BoxCollider2D _largeBoxCollider;
	private bool _deathTest = false;
	private float _deathStage = 0;
	private float _roarStage = 0;

	public override void Init()
	{
		_healthBar = GetComponentInChildren<HealthBar>();
		_Player = GameObject.Find("Player").transform;
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
			if( Mathf.Abs(distance) < 4)
			{
				if(Mathf.Abs(distance) < 2)
				{
					_roarStage = 2;
					_anim.SetFloat("Roar_Stage", _roarStage);
					_boxCollider.enabled = false;
					_midBoxCollider.enabled = false;
					_largeBoxCollider.enabled = true;
				}
				else
				{
					_roarStage = 1;
					_anim.SetFloat("Roar_Stage", _roarStage);
					_boxCollider.enabled = false;
					_midBoxCollider.enabled = true;
					_largeBoxCollider.enabled = false;
				}
			}
			else
			{
				_roarStage = 0;
				_anim.SetFloat("Roar_Stage", _roarStage);
				_boxCollider.enabled = true;
				_midBoxCollider.enabled = false;
				_largeBoxCollider.enabled = false;
			}
		}

		if(Dying)
		{
			rigidbody2D.velocity = new Vector2();
			_deathStage += Time.deltaTime*2;
			_anim.SetFloat("Death_Stage", _deathStage);
		}
	}

	public override void ApplyDamage(DamageDescription damage)
	{
		Health -= damage.AttackDamage;
		int direction = damage.AttackDirectionIsRight ? 1 : -1;
		rigidbody2D.AddForce(new Vector2(250 * damage.AttackDamage * direction, 0));
		
		if(Health <= 0)
		{
			if(!Dying)
			{
				_deathStage = _roarStage;
				
				//rigidbody2D.gravityScale = 0;
				
				_circleCollider.enabled = false;
				_boxCollider.enabled = false;
				_midBoxCollider.enabled = false;
				_largeBoxCollider.enabled = false;
				_deathCollider.enabled = true;

				Dying = true;
			}
			
			_deathStage += Time.deltaTime*2;
			_anim.SetFloat("Death_Stage", _deathStage);

			//Destroy(gameObject, 4f);
		}

		if(_healthBar != null)
		{
			_healthBar.SetHealth(Health / _maxHealth);
		}

		damage.Reflect(_id, 2.5f, 1.0f);
	}
}
