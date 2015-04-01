using UnityEngine;
using System.Collections;

public class OldCameraController : MonoBehaviour {

	public Transform MainChar;
	public bool StrictFollow = false;
	public float yOffset = -5.0f;
	public int DefaultZoom = 1;
	public int SizeModifier = 1;

	public bool DisplayDebugText = false;

	private int _initialCameraHeight = 0;
	private int _initialCameraWidth = 0;
	TextMesh _mesh;

	// Use this for initialization
	void Start ()
	{
		if(StrictFollow)
		{
			if(MainChar == null)
			{
				MainChar = GameObject.Find("Main Camera").transform;
				if(MainChar == null)
				{
					Debug.LogError("Main character not set.");
				}
			}
		}
		else
		{
			if(MainChar == null)
			{
				MainChar = GameObject.Find("Player").transform;
				if(MainChar == null)
				{
					Debug.LogError("Main character not set.");
				}
			}
		}

		SetCameraSize();
		//camera.orthographicSize += DefaultZoom;

		var t = transform.FindChild("Debug Text");
		_mesh = t.GetComponent<TextMesh> ();
	}

	void SetCameraSize()
	{
		//camera.orthographicSize = (Screen.height / 13f / 4.0f); // 100f is the PixelPerUnit that you have set on your sprite. Default is 100.
		//camera.orthographicSize = (Screen.height / SpriteSize / 2.0f / DefaultZoom); // 100f is the PixelPerUnit that you have set on your sprite. Default is 100.
		GetComponent<UnityEngine.Camera>().orthographicSize = GetComponent<UnityEngine.Camera>().orthographicSize = Screen.height*SizeModifier / (2 * DefaultZoom);

		_initialCameraHeight = Screen.height*SizeModifier;
		_initialCameraWidth = Screen.width*SizeModifier;

		if(DisplayDebugText)
		{
			var text = transform.FindChild("Debug Text");
			if(text != null)
			{
				var size1 = (int)(GetComponent<UnityEngine.Camera>().orthographicSize * 10f);
				var size2 = GetComponent<UnityEngine.Camera>().orthographicSize * 10f - size1;
				
				if(size2 > 0.01 || size2 < -0.01f)
				{
					var p = new Vector3();
					p.z = 10f;
					text.localPosition = p;
					
					text.gameObject.SetActive(true);
					var mesh = text.GetComponent<TextMesh>();
					mesh.text = "Camera Is Not Correct: " + GetComponent<UnityEngine.Camera>().orthographicSize;
				}
				else
				{
					text.gameObject.SetActive(false);
				}
			}
			else
			{
				Debug.Log("Text not enabled");
			}
		}
	}
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.LeftBracket))
		{
			DefaultZoom--;
			GetComponent<UnityEngine.Camera>().orthographicSize = GetComponent<UnityEngine.Camera>().orthographicSize = Screen.height*SizeModifier / (2 * DefaultZoom);
		}
		if(Input.GetKeyDown(KeyCode.RightBracket))
		{
			DefaultZoom++;
			GetComponent<UnityEngine.Camera>().orthographicSize = GetComponent<UnityEngine.Camera>().orthographicSize = Screen.height*SizeModifier / (2 * DefaultZoom);
		}

		/*
		if(_initialCameraHeight != Screen.height || _initialCameraWidth != Screen.width)
		{
			SetCameraSize();
		}
		*/

		if(MainChar == null)
			return;

		Vector2 charPos = MainChar.position;
		
		if(StrictFollow)
		{
			//int a = (int)(charPos.x * 100f);
			//float b = a / 100f;
			//transform.position = new Vector3((int)(charPos.x + 0.5f), (int)(charPos.y + 0.5f), transform.position.z) + new Vector3(-0.1f,-0.1f,0f);
			//transform.position = new Vector3(charPos.x, charPos.y, transform.position.z);// + new Vector3(-0.1f,-0.1f,0f);
			transform.position = new Vector3(charPos.x, charPos.y, transform.position.z) + new Vector3(-0.1f,-0.1f,0f);

			_mesh.text = charPos.ToString() + ' ' + transform.position.ToString();
		}
		else
		{
			//PerformUpdate();
		}
	}

	// Update is called once per frame
	//void FixedUpdate ()
	private void PerformUpdate()
	{
		float moveX = 0.0f;
		float moveY = 0.0f;
		float moveZ = 0.0f;

		//don't move the camera if the player is dead or gone
		if(MainChar == null)
		{
			return;
		}

		Vector2 charPos = MainChar.position;

		if(StrictFollow)
		{
			//int a = (int)(charPos.x * 100f);
			//float b = a / 100f;
			transform.position = new Vector3((int)charPos.x, (int)charPos.y, transform.position.z);
		}
		else
		{
			charPos += (Vector2)MainChar.GetComponent<Rigidbody2D>().velocity.normalized * 4;
			
			float distanceX = charPos.x - transform.position.x;
			float distanceY = charPos.y - (transform.position.y + yOffset);

			if(distanceX > 1 || distanceX < -2)
			{
				moveX = distanceX * 4f * Time.deltaTime;//Time.fixedDeltaTime;			
			}
			if(distanceY > 2 || distanceY < -0.1)
			{
				moveY = distanceY * 0.4f * Time.deltaTime;//Time.fixedDeltaTime;			
			}

			//int a = (int)(moveX * 100f);
			//float b = a / 100f;
			transform.Translate(new Vector3((int)moveX, (int)moveY, -moveZ));			
		}
	}
}
