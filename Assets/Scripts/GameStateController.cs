using UnityEngine;
using System.Collections;

public class GameStateController : MonoBehaviour {

	public int gold;
	public int reputation;
	public int suspicion;
	public int suspicionFailNum;

	public int sucessfulRecipes;
	public int failedRecipes;
	public SuspicionMeter suspicionMeter;

	void Start () {
	
		suspicionMeter = FindObjectOfType<SuspicionMeter> ();

		//Init values
		gold = 0;
		reputation = 0;
		suspicion = 0;
		sucessfulRecipes = 0;
		failedRecipes = 0;

		SaveGameState ();

		//Grab saved values
		GetStoredGameState();

	}

	// Update is called once per frame
	void Update () {
		//suspicionMeter.GetComponent<TextMesh> ().text = "Suspicion: " + suspicion + " Gold: " + gold + " Reputation: " + reputation;

		if (suspicion > suspicionFailNum) {
			Debug.Log ("Game Over");
		}
	}

	public void GetStoredGameState(){
		gold = PlayerPrefs.GetInt("gold");
		reputation = PlayerPrefs.GetInt("reputation");
		suspicion = PlayerPrefs.GetInt("suspicion");
		sucessfulRecipes = PlayerPrefs.GetInt("sucessfulRecipes");
		failedRecipes = PlayerPrefs.GetInt("failedRecipes");
		 
	}

	public void SaveGameState(){

		int highscore = PlayerPrefs.GetInt("highscore");
		if (reputation > highscore) {
			PlayerPrefs.SetInt("highscore",reputation);
		}
		PlayerPrefs.SetInt("gold",gold);
		PlayerPrefs.SetInt("reputation",reputation);
		PlayerPrefs.SetInt("suspicion",suspicion);
		PlayerPrefs.SetInt("sucessfulRecipes",sucessfulRecipes);
		PlayerPrefs.SetInt("failedRecipes",failedRecipes);
	}
	

}