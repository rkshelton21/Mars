using UnityEngine;
using System.Collections;

public class ApplyDamageToParent : MonoBehaviour 
{
	public void ApplyDamage(DamageDescription damage)
	{
		transform.parent.gameObject.SendMessage("ApplyDamage", damage);
	}
	
	public void Turn(bool[] turnFlags)
	{
		transform.parent.gameObject.SendMessage("Turn", turnFlags);
	}
}
