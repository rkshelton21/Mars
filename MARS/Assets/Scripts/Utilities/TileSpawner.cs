using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileSpawner : MonoBehaviour {
	
	public List<Tile> Tiles = new List<Tile>();
	private System.Random seedGenerator;
	private enum TileLocation { TOPLEFT, TOP, TOPRIGHT, RIGHT, BOTTOMRIGHT, BOTTOM, BOTTOMLEFT, LEFT, INVALID };
	
	private List<Object> _spawns = new List<Object>();
	
	// Use this for initialization
	void Start () {
		seedGenerator = new System.Random((int)System.DateTime.UtcNow.Ticks);
		/*
		var alphabet = "ABCDEFGHIJKLMNOPQRSTUV";
		foreach(var c in alphabet.ToCharArray())
		{
			Tiles.Add(new Tile(){
				Name = c.ToString()
			});
		}
		*/
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.F))
		{
			DespawnAll();
			
			System.Random r = new System.Random(seedGenerator.Next(0, 222));
			int startTile = 4;//r.Next(0, 22);
			
			Vector3 pos = new Vector3(3, 0, 0);
			
			var initialTile = Instantiate(Tiles[startTile].Model, pos, Quaternion.identity);
			_spawns.Add(((Transform)initialTile).gameObject);
			
			var prevPos = pos;
			var prevTile = startTile;
			
			for(int i = 0; i < 25; i++)
			{
				Vector3 newPos;
				int newTile;
				
				SpawnNextTile(prevTile, prevPos, r, out newPos, out newTile);
				
				prevPos = newPos;
				prevTile = newTile;
			}
		}	
	}
	
	private void DespawnAll()
	{
		foreach(var spawn in _spawns)
		{
			Destroy(spawn);
		}
		_spawns.Clear();
	}
	
	private void SpawnNextTile(int previousTile, Vector3 previousTilePosition, System.Random r, out Vector3 newPos, out int newTile)
	{
		newPos = previousTilePosition;
		
		newTile = SelectNextTile(previousTile, r);

		var delta = GetNextLocation(Tiles[previousTile].Exit, Tiles[newTile].Entrance);
		switch(delta)
		{
		case TileLocation.TOPLEFT:
			newPos.x -= Tiles[previousTile].TileImage.bounds.size.x;
			newPos.y += Tiles[previousTile].TileImage.bounds.size.y;
			break;
		case TileLocation.TOP:
			newPos.y += Tiles[previousTile].TileImage.bounds.size.y;
			break;
		case TileLocation.TOPRIGHT:
			newPos.x += Tiles[previousTile].TileImage.bounds.size.x;
			newPos.y += Tiles[previousTile].TileImage.bounds.size.y;
			break;
		case TileLocation.RIGHT:
			newPos.x += Tiles[previousTile].TileImage.bounds.size.x;
			break;
		case TileLocation.BOTTOMRIGHT:
			newPos.x += Tiles[previousTile].TileImage.bounds.size.x;
			newPos.y -= Tiles[previousTile].TileImage.bounds.size.y;
			break;
		case TileLocation.BOTTOM:
			newPos.y -= Tiles[previousTile].TileImage.bounds.size.y;
			break;
		case TileLocation.BOTTOMLEFT:
			newPos.x -= Tiles[previousTile].TileImage.bounds.size.x;
			newPos.y -= Tiles[previousTile].TileImage.bounds.size.y;
			break;
		default:
			newPos.x = 0;
			newPos.y = 0;
			break;
		}
		
		var spawn = Instantiate(Tiles[newTile].Model, newPos, Quaternion.identity);
		_spawns.Add(((Transform)spawn).gameObject);
	}
	
	private int SelectNextTile(int previousTile, System.Random r)
	{
		var validEntrances = ReturnMatchingEntrances(Tiles[previousTile].Exit);
		var validTiles = Tiles.FindAll(x => validEntrances.Contains(x.Entrance));
		if(validTiles.Count > 0)
		{
			int i = r.Next(validTiles.Count);
			return Tiles.IndexOf(validTiles[i]);
		}
		else
		{
			Debug.Log("Going from: " + previousTile);
			Debug.Log("No entrances matching: " + validEntrances.ToArray().ToString());
			foreach(var t in Tiles)
			{
				Debug.Log(t.Entrance);
			}
		}
		return Tiles.Count - 1;
	}
	
	private TileLocation GetNextLocation(int previousTileExit, int nextTileEntrance)
	{
		switch(previousTileExit)
		{
		case 0:
			switch(nextTileEntrance)
			{
			case 2:
				return TileLocation.LEFT;
			case 4:
				return TileLocation.TOPLEFT;
			case 6:
				return TileLocation.TOP;
			}
			return TileLocation.INVALID;
		case 1:
			return TileLocation.TOP;
		case 2:
			switch(nextTileEntrance)
			{
			case 4:
				return TileLocation.TOP;
			case 6:
				return TileLocation.TOPRIGHT;
			case 0:
				return TileLocation.RIGHT;
			}
			return TileLocation.INVALID;
		case 3:
			return TileLocation.RIGHT;
		case 4:
			switch(nextTileEntrance)
			{
			case 6:
				return TileLocation.RIGHT;
			case 0:
				return TileLocation.BOTTOMRIGHT;
			case 2:
				return TileLocation.BOTTOM;
			}
			return TileLocation.INVALID;
		case 5:
			return TileLocation.BOTTOM;
		case 6:
			switch(nextTileEntrance)
			{
			case 0:
				return TileLocation.BOTTOM;
			case 2:
				return TileLocation.BOTTOMLEFT;
			case 4:
				return TileLocation.LEFT;
			}
			return TileLocation.INVALID;
		case 7:
			return TileLocation.LEFT;
		default:		
			Debug.Log("sigh");	
			return TileLocation.INVALID;
		}
	}
	
	private List<int> ReturnMatchingEntrances(int exitValue)
	{
		switch(exitValue)
		{
		case 0:
			return new List<int>(){2, 4, 6};
		case 1:
			return new List<int>(){5};
		case 2:
			return new List<int>(){4, 6, 0};
		case 3:
			return new List<int>(){7};
		case 4:
			return new List<int>(){6, 0, 2};
		case 5:
			return new List<int>(){1};
		case 6:
			return new List<int>(){0, 2, 4};
		case 7:
			return new List<int>(){3};
		default:
			return new List<int>(){};
		}
		/*
		if(exitValue > 8)
			return 8;

		int entranceValue = exitValue + 4;
		if(entranceValue > 7)
			entranceValue -= 8;

		return entranceValue;
		*/
	}
}
