using UnityEngine;
using System.Collections;

public class DamageDescription
{
	public Vector2 AttackForce;
	public float AttackDamage;
	public bool AttackDirectionIsRight;
	public int AttackerId;

	public IDamageResponse Attacker;

	public void Reflect(int victim, float damageReduction, float percentageDeflected)
	{
		if(Attacker	!= null)
		{
			Attacker.ReduceAndDeflect(victim, damageReduction, percentageDeflected);
		}
	}
}
