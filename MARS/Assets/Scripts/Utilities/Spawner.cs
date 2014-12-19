﻿using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public Transform EnemyToSpawn = null;
	public int SpawnCount = -1;
	public float SpawnFrequency = 3.0f;
	public bool StartSpawnerAutomatically = false;

	private bool _spawningEnabled = false;
	private float _timeSinceSpawn = 0.0f;
	private int _numSpawned = 0;
	private bool _SpawnOnRequest = false;
	private bool _SpawnRequestTrigger = false;
	
	public bool RandomX = false;
	public float xVariant = 0f;

	public void StartSpawning()
	{
		_spawningEnabled = true;
	}

	public void SetRandomX(float x)
	{
		xVariant = x;
		RandomX = true;
	}

	public void StartSpawningOnRequest()
	{
		_spawningEnabled = true;
		_SpawnOnRequest = true;
	}

	public bool Spawn()
	{
		_SpawnRequestTrigger = true;
		if(_SpawnOnRequest == true && (SpawnCount == -1 || _numSpawned < SpawnCount))
		{
			return true;
		}
		return false;
	}

	// Use this for initialization
	void Start () {
		if (StartSpawnerAutomatically) 
		{
			StartSpawning();
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if(_spawningEnabled)
		{
			_timeSinceSpawn += Time.fixedDeltaTime;
		}

		//if spanwing is enabled
		//and there are more to spawn
		//and either spawn on request is off, or it's on and we haven't spawned yet
		if(	_spawningEnabled 
			&& (SpawnCount == -1 || _numSpawned < SpawnCount) && 
			(!_SpawnOnRequest || (_SpawnOnRequest && _SpawnRequestTrigger)))
		{
			_SpawnRequestTrigger = false;
			if(_timeSinceSpawn > SpawnFrequency)
			{
				var pos = transform.position;
				if(RandomX)
				{
					float offset = Random.Range(xVariant, -xVariant);
					pos.x += offset;
				}
				Debug.Log("Spawning at: " + pos);
				var boj = Instantiate(EnemyToSpawn, pos, Quaternion.identity);
				if(boj == null)
					Debug.LogError("Failed");

				_numSpawned++;
				_timeSinceSpawn = 0.0f;
			}
		}

	}
}
