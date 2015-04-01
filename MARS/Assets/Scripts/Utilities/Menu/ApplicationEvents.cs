using UnityEngine;
using System.Collections;

public class ApplicationEvents: MonoBehaviour
{
	public void NextLevel()
	{
		Application.LoadLevel(Application.loadedLevel + 1);
	}
	
	public void Quit()
	{
		Application.Quit();
	}
}
