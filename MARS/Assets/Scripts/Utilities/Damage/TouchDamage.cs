using UnityEngine;
using System.Collections;

public class TouchDamage : MonoBehaviour {

	public float Damage = 1f;
	public Vector2 AttackForce = new Vector2(20, 20);
	public float _CoolDown = 0.5f;
	private float _damageTimer	= 0f;
	private int _id;
	public bool FlipDirection = false;
	
	public void Start()
	{
		_id = GetInstanceID();
	}
	public void FixedUpdate()
	{
		_damageTimer -= Time.deltaTime;
	}
	
	public void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.tag != "Player")
			return;
		
		float direction = 1f;
		if(transform.localScale.x < 0)
			direction = -1f;
		if(FlipDirection)
			direction *= -1f;
		
		if(_damageTimer < 0)
		{
			collision.gameObject.SendMessage("ApplyDamage", new DamageDescription(){
				AttackDamage = Damage,
				AttackDirectionIsRight = transform.localScale.x > 0,
				AttackerId = _id,
				AttackForce = this.AttackForce*direction			
			});
			_damageTimer = _CoolDown;
		}		
	}
	
	public void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.tag != "Player")
			return;
			
		float direction = 1f;
		if(transform.localScale.x < 0)
			direction = -1f;
		if(FlipDirection)
			direction *= -1f;
		
		if(_damageTimer < 0)
		{
			collider.transform.parent.SendMessage("ApplyDamage", new DamageDescription(){
				AttackDamage = Damage,
				AttackDirectionIsRight = transform.localScale.x > 0,
				AttackerId = _id,
				AttackForce = this.AttackForce*direction			
			});
			_damageTimer = _CoolDown;
		}		
	}
}
