using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public void loadLevel(string name)
	{
		Application.LoadLevel (name);
	}
	public void QuitRequest(){
		Application.Quit ();
	}

}
