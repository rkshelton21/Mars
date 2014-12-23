using UnityEngine;
using System.Collections;

public class SlimeBoss : MonoBehaviour {

	public Transform ColumnSpawner;
	public float ColumnCooldown = 5;
	public float ArmHoldTime = 3;

	private Spawner _columnSoawner;
	private float _columnCountdown = 0;
	public float _armCountdown = 0;
	private Animator _anim;
	private bool _armsUp = false;

	// Use this for initialization
	void Start () {
		_columnSoawner = ColumnSpawner.GetComponent<Spawner> ();
		_columnSoawner.StartSpawningOnRequest ();
		_anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		_columnCountdown -= Time.deltaTime;
		_armCountdown -= Time.deltaTime;

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
	}

	public void SpawnColumn()
	{
		_anim.ResetTrigger("SummonComplete");
		_columnSoawner.Spawn ();
		_armsUp = true;
		_armCountdown = ArmHoldTime;
	}
}
