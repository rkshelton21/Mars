using UnityEngine;
using System.Collections;

[System.Serializable]
public class Tile {
	public Sprite TileImage { get; private set; }
	public Transform Model;
	public int Entrance { get; private set; }
	public int Exit { get; private set; }
	public string Name { get; private set; }

	public void UpdatePropertiesFromModel()
	{
		var props = Model.GetComponent<TileProperties>();

		TileImage = Model.GetComponent<SpriteRenderer>().sprite;
		Entrance = props.Entrance;
		Exit = props.Exit;
		Name = props.Name;
	}
}
