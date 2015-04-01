using UnityEngine;
using System.Collections;

public class DustParticles : MonoBehaviour {

	private Transform _Player;
	private ParticleSystem _Particles;
	public float Speed = 0;
	public float SpawnFactor = 2;
	public bool FacingForward;
	// Use this for initialization
	void Start () 
	{
		var p = GameObject.Find("Player");
		if (p == null)
			Debug.LogError ("Player not named 'Player'");

		_Player = p.transform;
		_Particles = GetComponent<ParticleSystem>();
		//_Particles.loop = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		Speed = _Player.GetComponent<Rigidbody2D>().velocity.x;
		if(Speed != 0 && !_Particles.isPlaying)
		{
			//_Particles.Play();
		}

		if(Speed == 0)
		{
			//_Particles.Stop();
		}

		Speed = Speed / 2f;
		if((FacingForward && Speed < 0) || (!FacingForward && Speed > 0))
			Speed = Speed / 2f;

		var spawnRate = Speed / SpawnFactor;

		_Particles.startSpeed = Mathf.Abs(Speed);
		_Particles.emissionRate = Mathf.Abs(spawnRate);
	}
}
