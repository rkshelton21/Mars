using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullet : MonoBehaviour, IDamageResponse {

	public Vector2 ShotPower = new Vector2(0.0f, 0.0f);
	public Vector2 ShotForce;
	public float TotalDamage = 0.1f;
	public int Id;
	public bool FacingRight = true;

	private BoxCollider2D _boxCollider;
	private float _timeAlive = 0.0f;
	private float TIME_TO_DIE = 0.5f;
	private bool _alive = false;
	private bool _dying = false;
	private bool _dead = false;
	System.Random r = new System.Random(System.Guid.NewGuid().GetHashCode());
	private List<int> _victims = new List<int>();
	private Animator _anim;
	private Rigidbody2D _rigidBody;
	// Use this for initialization
	void Start () 
	{
		_alive = true;
		_boxCollider = GetComponent<BoxCollider2D>();
		FacingRight = ShotForce.x > 0;
		if(!FacingRight)
		{
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}

		Id = transform.GetInstanceID();

		_anim = GetComponent<Animator>();
		_anim.SetTrigger("Fire");

		int direction = 0;
		if(ShotForce.x > -0.5 && ShotForce.x < 0.5 && ShotForce.y > 0.5)//up
		{
			direction = 0;
			//_boxCollider.size = new Vector2(0.3f, 0.5f);
			//_boxCollider.center = new Vector2(0f, 0.25f);
			transform.FindChild("HitBox_0").gameObject.SetActive(true);
			_rigidBody = transform.FindChild("HitBox_0").GetComponent<Rigidbody2D>();
		}
		else if((ShotForce.x < -0.5 || ShotForce.x > 0.5) && ShotForce.y > 0.5)//up angle
		{
			direction = 1;	
			//_boxCollider.size = new Vector2(0.5f, 0.5f);
			//_boxCollider.center = new Vector2(0.25f, 0.25f);
			transform.FindChild("HitBox_1").gameObject.SetActive(true);
			_rigidBody = transform.FindChild("HitBox_1").GetComponent<Rigidbody2D>();
		}
		else if((ShotForce.x < -0.5 || ShotForce.x > 0.5) && (ShotForce.y > -0.5 && ShotForce.y < 0.5))//norm
		{
			direction = 2;
			//_boxCollider.size = new Vector2(0.5f, 0.3f);
			//_boxCollider.center = new Vector2(0.25f, 0.03f);
			transform.FindChild("HitBox_2").gameObject.SetActive(true);
			_rigidBody = transform.FindChild("HitBox_2").GetComponent<Rigidbody2D>();
		}
		else if((ShotForce.x < -0.5 || ShotForce.x > 0.5) && (ShotForce.y < -0.5 || ShotForce.y > 0.5))//down angle
		{
			direction = 3;
			//_boxCollider.size = new Vector2(0.5f, 0.5f);
			//_boxCollider.center = new Vector2(0.25f, -0.25f);
			transform.FindChild("HitBox_3").gameObject.SetActive(true);
			_rigidBody = transform.FindChild("HitBox_3").GetComponent<Rigidbody2D>();
		}
		else if(ShotForce.x > -0.5 && ShotForce.x < 0.5 && ShotForce.y < -0.5)//down
		{
			direction = 4;
			//_boxCollider.size = new Vector2(0.3f, 0.5f);
			//_boxCollider.center = new Vector2(0f, -0.25f);
			transform.FindChild("HitBox_4").gameObject.SetActive(true);
			_rigidBody = transform.FindChild("HitBox_4").GetComponent<Rigidbody2D>();
		}

		_rigidBody = transform.rigidbody2D;
		_rigidBody.AddForce(ShotForce);
		_anim.SetFloat("Direction", (float)direction);
	}

	// Update is called once per frame
	void FixedUpdate () 
	{
		if(_alive)
		{
			_timeAlive += Time.deltaTime;
			if(_timeAlive > TIME_TO_DIE)
			{
				_dead = true;
			}
		}

		if(_dead)
		{
			Destroy(gameObject);
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		Debug.LogError("Collision?");
		_anim.SetTrigger("Explode");
		if(collision.collider.tag == "Bullet")
		{
			_anim.SetTrigger("Explode");
			_rigidBody.velocity = new Vector2(_rigidBody.velocity.y, _rigidBody.velocity.x);
			_boxCollider.isTrigger = true;
			_dead = true;
		}
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.tag == "Ground" && !_dying)
		{
			ReduceAndDeflect(0, 10000, 0);
		}

		if(collider.tag == "Enemy" && !_dying)
		{
			var instance = collider.transform;
			if(instance.parent != null)
				instance = instance.parent;
			if(instance.parent != null)
				instance = instance.parent;

			var victimId = instance.GetInstanceID();
			if(_victims.Contains(victimId))
				return;
			//Debug.Log("Damaged");

			_victims.Add(victimId);
			collider.gameObject.SendMessage("ApplyDamage", new DamageDescription()
        	{
				AttackDamage = TotalDamage,
				AttackDirectionIsRight = FacingRight,
				Attacker = this,
				AttackerId = Id,
				AttackForce = ShotForce
			});

			_anim.SetTrigger("Fire");
		}
	}

	public void ReduceAndDeflect(int victimId, float reduction, float deflection)
	{
		//Debug.Log("Reduce.");
		//if(_victims.Contains(victimId))
		//	return;
		//Debug.Log("Added " + victimId + " to "  + Id);

		//_victims.Add(victimId);

		//Debug.Log(victimId + " " + TotalDamage);
		
		TotalDamage -= reduction;
		if(TotalDamage <= 0)
		{
			_rigidBody.velocity = new Vector2();
			_dying = true;
			_boxCollider.enabled = false;
			Destroy(gameObject, 0.5f);
			_anim.SetTrigger("Explode");
		}

		int minXForce = (int)(300f);
		int maxXForce = (int)(500f);
		int minYForce = 0;
		int maxYForce = (int)(100f * deflection);
		int direction = FacingRight ? 1 : -1;

		System.Random r = new System.Random(System.Guid.NewGuid().GetHashCode());
		int xForce = r.Next(minXForce, maxXForce);
		int yForce = r.Next(minYForce, maxYForce);
		yForce = r.Next(2) == 0 ? yForce : -yForce;
		
		_rigidBody.velocity = new Vector2(0, 0);
		if(!_dying)
		{
			_rigidBody.AddForce(new Vector2(xForce*direction, yForce));
		}
	}
}
