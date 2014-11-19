using UnityEngine;
using System.Collections;

public class TileProperties : MonoBehaviour {

	public int Entrance;
	public int Exit;
	public string Name;
	public int id;

	private Sprite _sprite;
	// Use this for initialization
	void Start () {
		var edge = GetComponent<EdgeCollider2D>();
		_sprite = GetComponent<SpriteRenderer>().sprite;

		edge.points = new Vector2[2]
		{
			GetCoords(Entrance), //+ (Vector2)transform.position,
			GetCoords(Exit)// + (Vector2)transform.position,
		};
	}

	private Vector2 GetCoords(int entranceOrExitVal)
	{
		//Debug.Log(entranceOrExitVal);
		switch(entranceOrExitVal)
		{
		case 0:
			return new Vector2(- _sprite.bounds.size.x / 2, _sprite.bounds.size.y / 2);
		case 1:
			return new Vector2(0, _sprite.bounds.size.y / 2);
		case 2:
			return new Vector2(_sprite.bounds.size.x / 2, _sprite.bounds.size.y / 2);
		case 3:
			return new Vector2(_sprite.bounds.size.x / 2, 0);
		case 4:
			return new Vector2(_sprite.bounds.size.x / 2, - _sprite.bounds.size.y / 2);
		case 5:
			return new Vector2(0, - _sprite.bounds.size.y / 2);
		case 6:
			return new Vector2(- _sprite.bounds.size.x / 2, - _sprite.bounds.size.y / 2);
		case 7:
			return new Vector2(- _sprite.bounds.size.x / 2, 0);
		default:
			return new Vector2(0, 0);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
