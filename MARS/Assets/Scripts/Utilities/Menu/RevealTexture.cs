using UnityEngine;
using System.Collections;

public class RevealTexture : MonoBehaviour 
{
	public float f = 0f;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		var revealOffset = f / 100f; 
		f += Time.deltaTime;
		gameObject.GetComponent<Renderer>().material.SetTextureOffset ("_Mask", new Vector2(0f, revealOffset));

		if(revealOffset > 1)
			f = 0;
	}
}
