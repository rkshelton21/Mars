using UnityEngine;
using System.Collections;

public class PassDamageTo : MonoBehaviour {

	public Transform Recepient;
	private CharController2D _damageRecepient;

	// Use this for initialization
	void Start () {
		_damageRecepient = Recepient.GetComponent<CharController2D>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ApplyDamage(DamageDescription damage)
	{
		_damageRecepient.ApplyDamage(damage);
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		_damageRecepient.Collide(collision);
	}
	
	void OnCollisionStay2D(Collision2D collision)
	{
		_damageRecepient.Collide(collision);
	}
}
