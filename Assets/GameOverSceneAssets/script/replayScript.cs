using UnityEngine;
using System.Collections;

public class replayScript : MonoBehaviour {

	public int levelLoad;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void loadLevel()
	{
		Application.LoadLevel (levelLoad);
	}
}
