﻿using UnityEngine;
using System.Collections;

public class BlobController : AIController2D {

	public override void Collide(Collision2D collision)
	{
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
	
	public void OnTriggerEnter2D(Collider2D collider)
	{
		bool validTarget = (collider.gameObject.tag == "Enemy" || collider.gameObject.tag == "Player");
		if(validTarget && !Dying)
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
				var damageFlagTriggered = 1.0f;//_anim.GetFloat("ApplyMeleeDamage");
				/*if(collider.collider.GetType().ToString() == "UnityEngine.Collider")
				{
					Debug.Log("Box hit: " + damageFlagTriggered + ": " + _damageTimer);
				}
				else
				{
					Debug.Log(collider.collider.GetType().ToString());
					Debug.Log("OK");
					return;
				}*/
								
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
					Debug.Log(collider.tag);
					collider.gameObject.SendMessage("ApplyDamage", new DamageDescription(){
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
}
