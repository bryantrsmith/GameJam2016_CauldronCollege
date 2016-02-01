using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FinalScore : MonoBehaviour {

	public int gold;
	public int reputation;
	public int suspicion;
	public int suspicionFailNum;

	public int sucessfulRecipes;
	public int failedRecipes;

	public int highScore;
	public SuspicionMeter suspicionMeter;

	void Start () {
		//Grab saved values
		GetStoredGameState();

		//Set score and highscore
		//GameObject.Find ("Score").GetComponent<Text>().
		Text yourButtonText = transform.FindChild("Score").GetComponent<Text>();
		yourButtonText.text = "" + reputation;

		yourButtonText = transform.FindChild("HighScore").GetComponent<Text>();
		yourButtonText.text = "" + highScore;
	}
		
	public void GetStoredGameState(){
		gold = PlayerPrefs.GetInt("gold");
		reputation = PlayerPrefs.GetInt("reputation");
		suspicion = PlayerPrefs.GetInt("suspicion");
		sucessfulRecipes = PlayerPrefs.GetInt("sucessfulRecipes");
		failedRecipes = PlayerPrefs.GetInt("failedRecipes");
		highScore = PlayerPrefs.GetInt("highScore");

	}
}
