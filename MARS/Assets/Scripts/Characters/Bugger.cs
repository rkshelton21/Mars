using UnityEngine;
using System.Collections;

public class Bugger : MonoBehaviour {
	System.Random _Random;

	public Transform Target;
	public Sprite[] sprites;
	public float framesPerSecond;
	public float moveSpeed;
	public float turnSpeed;
	public AudioClip ImpactClip;

	protected Animator _anim;

	//private float Health = 1;
	private bool Dying = false;

	private int _id;
	private CircleCollider2D _circleCollider;
	private SpriteRenderer spriteRenderer;
	private Vector3 moveDirection;
	public Vector3 _floatOffset;
	public double _PermanentOffsetMax = 3.0;
	public double _RandomBuzzOffsetMax = 10.0;
	private bool _facingRight = false;
	private Color _teamColor = Color.white;
	public float AttackCooldown = 1.0f;
	public float _attackTimer = 0.0f;

	// Use this for initialization
	void Start () {
		if(Target == null)
		{
			var player = GameObject.Find("Player");
			if(player != null)
			{
				Target = GameObject.Find("Player").transform;
			}
		}

		_anim = GetComponent<Animator>();
		_circleCollider = GetComponent<CircleCollider2D>();
		_id = transform.GetInstanceID();

		_Random = new System.Random(System.DateTime.UtcNow.Millisecond);
		float x = (float)(_Random.NextDouble()*_PermanentOffsetMax - _PermanentOffsetMax/2.0);
		float y = (float)(_Random.NextDouble()*_PermanentOffsetMax - _PermanentOffsetMax/2.0);
		_floatOffset = new Vector3(x, y, 0);

		spriteRenderer = GetComponent<Renderer>() as SpriteRenderer;
		moveDirection = Vector3.right;
		spriteRenderer.color = _teamColor;
	}
	
	// Update is called once per frame
	void Update () {
		_attackTimer -= Time.deltaTime;

		if(sprites.Length > 0)
		{
			int index = (int)(Time.timeSinceLevelLoad * framesPerSecond);
			index = index % sprites.Length;
			spriteRenderer.sprite = sprites[ index ];
		}
		
		// 1
		Vector3 currentPosition = transform.position;
		// 2
		if(Target == null)
		{
			//move toward mouse click
			//Vector3 moveToward = Camera.main.ScreenToWorldPoint( Input.mousePosition );
			Vector3 moveToward = transform.position;
			moveDirection = moveToward - currentPosition;
			moveDirection.z = 0; 
			//moveDirection.Normalize();
			
			//Hover
			float x = (float)(_Random.NextDouble()*_RandomBuzzOffsetMax - _RandomBuzzOffsetMax/2.0);
			float y = (float)(_Random.NextDouble()*_RandomBuzzOffsetMax - _RandomBuzzOffsetMax/2.0);
			Vector3 buzzOffset = new Vector3(x, y, 0.0f);
			// 4
			moveDirection = currentPosition;
			moveDirection.z = 0; 
			moveDirection.Normalize();
			
			/*
			if( Input.GetButton("Fire1") ) {
				// 3
				Vector3 moveToward = Camera.main.ScreenToWorldPoint( Input.mousePosition );
				// 4
				moveDirection = moveToward - currentPosition;
				moveDirection.z = 0; 
				moveDirection.Normalize();
			}
			*/
		}
		else
		{			
			float x = (float)(_Random.NextDouble()*_RandomBuzzOffsetMax - _RandomBuzzOffsetMax/2.0);
			float y = (float)(_Random.NextDouble()*_RandomBuzzOffsetMax - _RandomBuzzOffsetMax/2.0);
			Vector3 buzzOffset = new Vector3(x, y, 0.0f);
			Vector3 moveToward = Target.position + buzzOffset;
			// 4
			moveDirection = moveToward - currentPosition;
			moveDirection.z = 0; 
			moveDirection.Normalize();					
		}
		
		var targetInRange = false;
		if(Target != null)
		{			
			float distance = (Target.position - transform.position).magnitude;
			float absDistance = Mathf.Abs(distance);
			//targetInRange = absDistance < 2.5f;
			targetInRange = true;
		}
		
		if(targetInRange)
		{
			Vector3 target = moveDirection * moveSpeed + currentPosition;
			transform.position = Vector3.Lerp( currentPosition, target, Time.deltaTime );
			
			float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
			transform.rotation = 
				Quaternion.Slerp( transform.rotation, 
				                 Quaternion.Euler( 0, 0, targetAngle ), 
				                 turnSpeed * Time.deltaTime );
		}
		else
		{
			Vector3 target = moveDirection * moveSpeed + currentPosition;
			transform.position = Vector3.Lerp( currentPosition, target, Time.deltaTime );
			
			float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
			transform.rotation = 
				Quaternion.Slerp( transform.rotation, 
				                 Quaternion.Euler( 0, 0, targetAngle ), 
				                 turnSpeed * Time.deltaTime );
		}
	}

	public void Flip()
	{
		_facingRight = !_facingRight;
		
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	
	void OnCollisionEnter2D(Collision2D collision)
	{
		bool validTarget = (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Player");
		if(validTarget && !Dying && collision.gameObject.tag != gameObject.tag && _attackTimer < 0)
		{
			collision.gameObject.SendMessage("ApplyDamage", new DamageDescription(){
				AttackDamage = 1,
				AttackDirectionIsRight = _facingRight,
				AttackerId = _id,
				AttackForce = _facingRight ? new Vector2(20, 20) : new Vector2(-20, 20)
			});

			_attackTimer = AttackCooldown;
		}
	}

	public void ApplyDamage(DamageDescription damage)
	{
		//Debug.Log("Damage taken: " + damage);

		AudioSource.PlayClipAtPoint (ImpactClip, transform.position);

		//rigidbody2D.AddForce(new Vector2(300 * bullet.TotalDamage, 0));
		//_anim.SetBool("Dying", true);
		if(_anim != null)
		{
			_anim.SetTrigger("Die");
			_circleCollider.enabled = false;
	
			damage.Reflect(_id, 0.5f, 0.2f);
			Destroy(gameObject, 0.75f);
		}
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
		
		if(spriteRenderer != null)
		{
			spriteRenderer.color = _teamColor;
		}
	}

	public void SetTarget(Transform newTarget)
	{
		Target = newTarget;
	}
}
