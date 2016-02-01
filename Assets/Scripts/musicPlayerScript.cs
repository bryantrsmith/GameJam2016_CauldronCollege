using UnityEngine;
using System.Collections;

public class musicPlayerScript : MonoBehaviour {

	static musicPlayerScript instance = null;
	private int levelName;
	// Use this for initialization
	void Awake () {
		if (instance != null) {
			Destroy (gameObject); //if we come back to main menu then this music player will stay.
		} else
		{
			instance = this;
			GameObject.DontDestroyOnLoad (gameObject); //carry music player to next scene.
		}
	}
	
	// Update is called once per frame
	void Update () {
		levelName = Application.loadedLevel;

		if (levelName == 3 || levelName == 4 || levelName == 5 || levelName == 6) {
			Destroy (gameObject);
		}
	}
}
