using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {
	public SpriteRenderer _renderer;
	public Rigidbody2D _rigidBody;
	public float LifeSpan = 3f;
	private float lifeTime;
	// Use this for initialization
	void Start () 
	{
		if(_rigidBody == null)
		{
			_rigidBody = transform.GetComponent<Rigidbody2D>();			
		}		
		if(_renderer == null)
		{
			_renderer = transform.GetComponent<SpriteRenderer>();
		}
	}
	
	void FixedUpdate()
	{
		lifeTime += Time.deltaTime;
		if(lifeTime > LifeSpan)
		{
			gameObject.SetActive(false);
		}
	}
	
	public void Init(Vector4[] parameters)
	{
		Vector2 dir = new Vector2(parameters[0].x, parameters[0].y);
		Color c = parameters[1];
		
		if(_rigidBody == null)
		{
			_rigidBody = transform.GetComponent<Rigidbody2D>();					
		}
		if(_renderer == null)
		{
			_renderer = transform.GetComponent<SpriteRenderer>();
		}
		Vector3 v = new Vector3();
		if(dir.x < 0)
		{
			v = Quaternion.Euler(0, 0, Random.Range(-70f, -20f)) * dir.normalized;
		}
		else
		{
			v = Quaternion.Euler(0, 0, Random.Range(70f, 20f)) * dir.normalized;
		}
		_rigidBody.velocity = v.normalized * 1.2f;
		_renderer.color = c;
		//Debug.Log(v);
		
		lifeTime = 0f;
	}
}
