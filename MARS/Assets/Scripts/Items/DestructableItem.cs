using UnityEngine;
using System.Collections;

public class DestructableItem : MonoBehaviour {
	private HealthBar _healthBar;
	public float Health = 10f; 
	private SimpleAnimation _anim;
	private DestructableAnimation _danim;
	private float _health = 1.0f;
	
	// Use this for initialization
	void Start () {
		_healthBar = GetComponentInChildren<HealthBar>();
		_anim = GetComponent<SimpleAnimation>();
		_danim = GetComponent<DestructableAnimation>();		
		_health = Health;
	}
	
	public void ApplyDamage(DamageDescription damage)
	{
		_health -= damage.AttackDamage;
		_healthBar.SetHealth(_health / Health);
		
		if(_health <= 0)
		{
			_anim.enabled = false;
			_danim.enabled = true;			
		}
	}
}
