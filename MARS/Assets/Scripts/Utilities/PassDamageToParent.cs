using UnityEngine;
using System.Collections;

public class PassDamageToParent : MonoBehaviour {
	
	private ObjectThatAcceptsDamage _parent;
	
	// Use this for initialization
	void Start () {
		_parent = transform.GetComponentInParent<ObjectThatAcceptsDamage>();
	}
	
	public void ApplyDamage(DamageDescription damage)
	{
		_parent.ApplyDamage(damage);			
	}	
}
