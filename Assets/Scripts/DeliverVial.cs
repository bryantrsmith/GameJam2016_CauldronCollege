using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))]
public class DeliverVial : MonoBehaviour {

	public PotContents potContents;
	public WeekManager weekManager;
	public GameStateController gameStateController;
	public AngerBar angerBar;

	public SuspicionMeter suspicionMeter;

	public AudioClip successSound;
	public AudioClip failSound;
	public AudioClip failSound2;

	// Use this for initialization
	void Start () {
		gameStateController = FindObjectOfType<GameStateController> ();
		suspicionMeter = FindObjectOfType<SuspicionMeter> ();
		angerBar = FindObjectOfType<AngerBar> ();
	}
	
	// Update is called once per frame
	void Update () {
	
		//Compare when num ingredients match
		if (weekManager.isThereAnActiveRecipe && potContents.potIngredients.Count == weekManager.activeRecipe.finalRecipeList.Count) {
			CompareRecipeToPot ();
		}
	}

	void OnMouseDown () {
		//print ("Recipe Delivered!");
		//CompareRecipeToPot ();
	}

	void SetFailure (Recipe r)
	{
		AudioSource.PlayClipAtPoint(failSound2, transform.position);
		r.isFinished = true;
		r.isSuccessful = false;
		r.currentTimer = 0;
	}

	void CleanUpPot(){
		weekManager.isThereAnActiveRecipe = false;
		potContents.EmptyPot ();
	}

	bool CompareRecipeToPot(){

		if (weekManager.isThereAnActiveRecipe) {
			if (potContents.potIngredients.Count != weekManager.activeRecipe.finalRecipeList.Count) {
				Debug.Log ("WRONG SIZE!");
				SetFailure (weekManager.activeRecipe);	
				CleanUpPot ();
				return false;
			} else {

				string strContents = "Pot:\n";
				for (int i = 0; i < potContents.potIngredients.Count; i++) {
					strContents += potContents.potIngredients [i] + "\n";
				}
				strContents += "Recipe:\n";
				for (int i = 0; i < weekManager.activeRecipe.finalRecipeList.Count; i++) {
					strContents += weekManager.activeRecipe.finalRecipeList [i] + "\n";
				}
				Debug.Log (strContents);
					
				for (int i = 0; i < potContents.potIngredients.Count; i++) {

					//Debug.Log (potContents.potIngredients [i].ingredientName + "  " + weekManager.activeRecipe.finalRecipeList [i]);
					if (potContents.potIngredients [i].ingredientName != weekManager.activeRecipe.finalRecipeList [i]) {
						Debug.Log ("U SUX!");
						SetFailure (weekManager.activeRecipe);					

						CleanUpPot ();
						return false;
					}
				}
				Debug.Log ("Match its a MIRACLE!");
				SuceedRecipe (weekManager.activeRecipe);
				CleanUpPot ();
				return true;
			}

		} else {
			CleanUpPot ();
			return false;
		}
	}


	public void FailRecipe(Recipe r){
		r.isFinished = true;
		r.isSuccessful = false;
		r.currentTimer = 0;
		//weekManager.isThereAnActiveRecipe = false;
		AudioSource.PlayClipAtPoint(failSound, transform.position);

		gameStateController.gold -= r.goldReward;
		gameStateController.reputation -= r.reputationPenalty;
		gameStateController.suspicion += r.moodPenalty;
		gameStateController.failedRecipes += 1;

		angerBar.UpdateAnger (gameStateController.suspicion);
		//suspicionMeter.GetComponent<TextMesh> ().text = "Suspicion: " + gameStateController.suspicion + " Gold: " + gameStateController.gold + " Reputation: " + gameStateController.reputation;
	}

	void SuceedRecipe(Recipe r){
		r.isFinished = true;
		r.isSuccessful = true;
		//weekManager.isThereAnActiveRecipe = false;
		r.currentTimer = 0;

		AudioSource.PlayClipAtPoint(successSound, transform.position);

		gameStateController.gold += r.goldReward;
		gameStateController.reputation += r.reputationReward;

//		if (gameStateController.suspicion > 0) {
//			gameStateController.suspicion -= r.moodReward;
//		}
		gameStateController.sucessfulRecipes += 1;

		//angerBar.UpdateAnger (gameStateController.suspicion);
		//suspicionMeter.GetComponent<TextMesh> ().text = "Suspicion: " + gameStateController.suspicion + " Gold: " + gameStateController.gold + " Reputation: " + gameStateController.reputation;
	}
}
