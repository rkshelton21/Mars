using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlimeBoss : ObjectThatAcceptsDamage {

	public AudioClip ImpactClip;
	public List<Transform> ColumnSpawners;
	public float ColumnCooldown = 5;
	public float ArmHoldTime = 3;

	private List<Spawner> _columnSpawners;
	private float _columnCountdown = 0;
	public float _armCountdown = 0;
	private Animator _anim;
	private bool _armsUp = false;
	public float Health = 100f;
	private float _maxHealth;
	private HealthBar _healthBar;
	private Transform _player;
	public bool _facingRight = false;
	private float _attackTimer = 0.5f;
	private float _damageTimer = 0;
	private int _id;
	
	// Use this for initialization
	void Start () {
		_columnSpawners = new List<Spawner>();
		foreach(var spawner in ColumnSpawners)
		{
			var s = spawner.GetComponent<Spawner> ();
			_columnSpawners.Add(s);		
			s.StartSpawningOnRequest ();
		}
		_anim = GetComponent<Animator> ();
		_maxHealth = Health;
		_healthBar = GetComponentInChildren<HealthBar>();
		_player = GameObject.Find("Player").transform;
		_id = GetInstanceID();
	}
	
	// Update is called once per frame
	void Update () 
	{
		_columnCountdown -= Time.deltaTime;
		_armCountdown -= Time.deltaTime;
		_damageTimer -= Time.deltaTime;
		
		if (_columnCountdown < 0) 
		{
			_anim.SetTrigger("Raise");
			_columnCountdown = ColumnCooldown;
		}

		if (_armsUp && _armCountdown < 0) 
		{
			_anim.SetTrigger("SummonComplete");
			_anim.ResetTrigger("Raise");
			_armsUp = false;
		}
		
		var targetInRange = false;
		float distance = (_player.position - transform.position).magnitude;
		float absDistance = Mathf.Abs(distance);
		targetInRange = absDistance < 0.5f;
		var needFaceRight = _player.position.x > transform.position.x;
		
		if(needFaceRight && !_facingRight)
		{
			Flip();
		}
		if(!needFaceRight && _facingRight)
		{
			Flip();
		}
		
		if(targetInRange)
		{
			_anim.SetTrigger("Slap");			
		}
	}
	
	protected void Flip()
	{
		_facingRight = !_facingRight;
		
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	} 

	public void SpawnColumn()
	{
		_anim.ResetTrigger("SummonComplete");
		foreach(var spawner in _columnSpawners)
		{
			spawner.Spawn ();
		}
		_armsUp = true;
		_armCountdown = ArmHoldTime;
	}
	
	public override void ApplyDamage(DamageDescription damage)
	{
		Health -= damage.AttackDamage;
		//Debug.Log("Damage taken: " + damage);
		
		AudioSource.PlayClipAtPoint (ImpactClip, transform.position);
		
		if(_healthBar != null)
		{
			_healthBar.SetHealth(Health / _maxHealth);
		}
		//rigidbody2D.AddForce(new Vector2(300 * bullet.TotalDamage, 0));
		//_anim.SetBool("Dying", true);
		if(Health <= 0 && _anim != null)
		{
			_anim.SetTrigger("Die");
			_anim.SetBool("Dying", true);
			
			Destroy(gameObject, 5.0f);
		}
	}
	
	public void OnCollisionEnter2D(Collision2D collision)
	{
		if(_damageTimer < 0)
		{
			collision.gameObject.SendMessage("ApplyDamage", new DamageDescription(){
				AttackDamage = 3,
				AttackDirectionIsRight = _facingRight,
				AttackerId = _id,
				AttackForce = new Vector2(20000, 20)
			});
			_damageTimer = _attackTimer;
		}	
	}
}	