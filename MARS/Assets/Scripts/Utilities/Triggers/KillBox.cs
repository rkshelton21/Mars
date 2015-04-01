using UnityEngine;
using System.Collections;

public class KillBox : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	void OnTriggerEnter2D(Collider2D collider)
	{
		Transform current = collider.transform;
		while (current.parent != null) 
		{
			if(current.parent.tag == "Enemy")
			{
				current = current.parent;
			}
			else
			{
				break;
			}						
		}
		
		if(current.tag == "Player")
		{
			current.SendMessage("ApplyDamage", new DamageDescription(){
				AttackDamage = 100000,
				AttackerId = -1				
			});
		}
		else if(current.tag == "Particle")
		{
			current.gameObject.SetActive(false);
		}
		else
		{
			Destroy (current.gameObject);	
		}
	}
}
