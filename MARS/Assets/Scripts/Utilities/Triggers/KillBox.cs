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
			if(current.parent.name != "Characters")
			{
				current = current.parent;
			}
			else
			{
				break;
			}						
		}
		
		Destroy (current.gameObject);	
	}
}
