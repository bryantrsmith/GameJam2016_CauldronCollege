using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WeekManager : MonoBehaviour {

	public int weekNumber; //1st, 2nd, 3rd, etc

	public float weekTotalTime;
	public float weekTimeCount;

	public float maxDelayToAddNext;
	public float delayCounter;

	public int totalRecipesToAddThisWeek;
	public int recipesAddedThisWeekCount;

	public int totalNumberOfRecipesWeek1;
	public int totalNumberOfRecipesWeek2;
	public int totalNumberOfRecipesWeek3;
	public int totalNumberOfRecipesWeek4;
	public int totalNumberOfRecipesWeek5;

	//Week difficulty arrays

	public GameObject recipePanel_1;
	public GameObject recipePanel_2;
	public GameObject recipePanel_3;
	public GameObject recipePanel_4;
	public GameObject recipePanel_5;
	public GameObject recipePanel_6;

	public GameObject recipeButton_1;
	public GameObject recipeButton_2;
	public GameObject recipeButton_3;
	public GameObject recipeButton_4;
	public GameObject recipeButton_5;
	public GameObject recipeButton_6;

	private Vector3 recipePanelStartPos1;
	private Vector3 recipePanelStartPos2;
	private Vector3 recipePanelStartPos3;
	private Vector3 recipePanelStartPos4;
	private Vector3 recipePanelStartPos5;
	private Vector3 recipePanelStartPos6;
	private Vector3 recipeButtonStartPos1;
	private Vector3 recipeButtonStartPos2;
	private Vector3 recipeButtonStartPos3;
	private Vector3 recipeButtonStartPos4;
	private Vector3 recipeButtonStartPos5;
	private Vector3 recipeButtonStartPos6;

	public Vector3 hidden = new Vector3(-100f, -100f, 0f);

	public int currentRecipeNum;

	public Text recipeText;

	public bool isThereAnActiveRecipe;
	public int whichRecipeIsActive;
	public Recipe activeRecipe;

	public PotContents potContents;
	public GameObject currentActiveRecipeText;
	public DeliverVial deliverVial;
	public GameObject recipePanel;
	public Camera threeDCamera;
	private bool dayOne;

	//public List<RecipePanel> panelList = new List<RecipePanel> ();
	public List<Recipe> recipeList = new List<Recipe>();
	private List<int> weekDifficultyList = new List<int>();

	// Use this for initialization
	void Start () {

		//Camera weirdness
		threeDCamera.enabled = true;

		dayOne = true;

		StartWeek ();
		weekTimeCount = weekTotalTime;
		isThereAnActiveRecipe = false;

		potContents = FindObjectOfType<PotContents>();
		deliverVial = FindObjectOfType<DeliverVial> ();

		//Store panel starting locations
		recipePanelStartPos1 = recipePanel_1.transform.position;
		recipePanelStartPos2 = recipePanel_2.transform.position;
		recipePanelStartPos3 = recipePanel_3.transform.position;
		recipePanelStartPos4 = recipePanel_4.transform.position;
		recipePanelStartPos5 = recipePanel_5.transform.position;
		recipePanelStartPos6 = recipePanel_6.transform.position;

		recipeButtonStartPos1 = recipeButton_1.transform.position;
		recipeButtonStartPos2 = recipeButton_2.transform.position;
		recipeButtonStartPos3 = recipeButton_3.transform.position;
		recipeButtonStartPos4 = recipeButton_4.transform.position;
		recipeButtonStartPos5 = recipeButton_5.transform.position;
		recipeButtonStartPos6 = recipeButton_6.transform.position;
	}
		
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.Escape))
		{
			
			Application.LoadLevel ("StartScene");
		}

		weekTimeCount -= Time.deltaTime;


		if (weekTimeCount <= 0 || (recipesAddedThisWeekCount >= totalRecipesToAddThisWeek && recipeList.Count <= 0)) {
			Debug.Log ("Week Over!");

			//Show summary
			GameStateController state = FindObjectOfType<GameStateController>();
			state.SaveGameState();

			Application.LoadLevel ("winScene");
		}

		//The delay before adding another recipe
		delayCounter -= Time.deltaTime;

		if (dayOne != true) {
			delayCounter = 0;
			dayOne = false;
		}

		if (delayCounter <= 0 && recipesAddedThisWeekCount < totalRecipesToAddThisWeek && recipeList.Count < 4) {

			//Get new value off weekDifficultyList
			int num = Random.Range (0, weekDifficultyList.Count);
			AddRecipeToWeek (weekDifficultyList[num]);
		}

		//Hide all panels
		HidePanels();

		if (isThereAnActiveRecipe) {
			currentActiveRecipeText.GetComponent<TextMesh> ().text = "";
			currentActiveRecipeText.GetComponent<TextMesh> ().text += recipeList [whichRecipeIsActive].recipeName + "\n\n";

			foreach (var item in recipeList [whichRecipeIsActive].finalRecipeList) {
				currentActiveRecipeText.GetComponent<TextMesh> ().text += item + "\n";
			}				
			currentActiveRecipeText.GetComponent<TextMesh> ().text += "\n" + recipeList [whichRecipeIsActive].recipeDescription;
		} else {
			currentActiveRecipeText.GetComponent<TextMesh> ().text = "";
		}

		for (int i = 0; i < recipeList.Count; i++) {
			recipeList[i].currentTimer -= Time.deltaTime;

			//If successful
			if (recipeList [i].isFinished && recipeList [i].isSuccessful) {

				//If this is the active recipe, remove it from activeRecipe
				if (whichRecipeIsActive == i) {

					//Clear Text
					currentActiveRecipeText.GetComponent<TextMesh> ().text = "";
					isThereAnActiveRecipe = false;

					//Empty Pot
					potContents.EmptyPot ();
				}

				if (recipeList [i].currentTimer <= -3) {
					if (whichRecipeIsActive > i) {
						whichRecipeIsActive--;
					}

					recipeList.RemoveAt (i); //WARNING: POSSIBLE ISSUES HERE
				}
			}
			else if (recipeList [i].currentTimer <= 0) {
				//Timer ran out on a task	
				recipeList [i].isFinished = true;
				recipeList [i].isSuccessful = false;

				//If this is the active recipe, remove it from activeRecipe
				if (whichRecipeIsActive == i) {

					//Clear Text
					currentActiveRecipeText.GetComponent<TextMesh> ().text = "";

					//Add failure animation
					isThereAnActiveRecipe = false;

					//Empty Pot
					potContents.EmptyPot();
				}

				//Disable accept button
				//Feedback that a task failed
				//After a set time Remove from List
				if (recipeList [i].currentTimer <= -3) {

					if (whichRecipeIsActive > i) {
						whichRecipeIsActive --;
					}					
					deliverVial.FailRecipe (recipeList [i]);
					recipeList.RemoveAt (i); //WARNING: POSSIBLE ISSUES HERE
				}
			}

		}
			
		for (int i = 0; i < recipeList.Count; i++) {
			//Display / Reposition Panels
			PopulatePanel (i, recipeList [i]);
		}
			
	}

	void HidePanels(){

		recipeButton_1.transform.position= hidden;
		recipeButton_2.transform.position= hidden;
		recipeButton_3.transform.position= hidden;
		recipeButton_4.transform.position= hidden;
		recipeButton_5.transform.position= hidden;
		recipeButton_6.transform.position= hidden;

		recipePanel_1.transform.position = hidden;
		recipePanel_2.transform.position = hidden;
		recipePanel_3.transform.position = hidden;
		recipePanel_4.transform.position = hidden;
		recipePanel_5.transform.position = hidden;
		recipePanel_6.transform.position = hidden;

	}
		
	void PopulatePanel(int i, Recipe recipe){
		if (i == 0) {

			recipeButton_1.transform.position = recipeButtonStartPos1;
			recipePanel_1.transform.position = recipePanelStartPos1;

//			if (recipeList [i].currentTimer < 7) {
//				recipePanel_1.transform.Find ("PanelTimer").GetComponent<TextMesh> ().color = Color.red;
//			} else if (recipeList [i].currentTimer < 20) {
//				recipePanel_1.transform.Find ("PanelTimer").GetComponent<TextMesh> ().color = new Color (230, 90, 0);
//			} else {
//				recipePanel_1.transform.Find ("PanelTimer").GetComponent<TextMesh> ().color = Color.black;
//			}

			recipePanel_1.transform.Find ("PanelTimer").GetComponent<TextMesh> ().text = "" + Mathf.RoundToInt( recipeList [i].currentTimer);
			recipePanel_1.transform.Find ("PanelName").GetComponent<TextMesh> ().text = "" + recipeList [i].recipeName;
			recipePanel_1.transform.Find ("PanelName").GetComponent<TextMesh> ().text = "" + recipeList [i].recipeName;
			Sprite s = GameObject.Find ("Villagers" + recipe.villagerNum).GetComponent<SpriteRenderer> ().sprite;		
			if (s != null) {
				recipePanel_1.transform.Find ("PanelPortrait").GetComponent<SpriteRenderer> ().sprite = s;
			}

			if (isThereAnActiveRecipe && i == whichRecipeIsActive) {
				s = GameObject.Find ("HighlightedRecipe").GetComponent<SpriteRenderer> ().sprite;		
				recipePanel_1.GetComponent<SpriteRenderer> ().sprite = s;
			} else {
				s = GameObject.Find ("RecipeBackground").GetComponent<SpriteRenderer> ().sprite;		
				recipePanel_1.GetComponent<SpriteRenderer> ().sprite = s;
			}

			if (recipe.isFinished && !recipe.isSuccessful && recipe.currentTimer <= 0) {
				s = GameObject.Find ("sealBroken").GetComponent<SpriteRenderer> ().sprite;
				GameObject.Find ("Button1").GetComponent<SpriteRenderer> ().sprite = s;
			} else if (recipe.isFinished && recipe.isSuccessful && recipe.currentTimer <= 0) {
				s = GameObject.Find ("sealSuccess").GetComponent<SpriteRenderer> ().sprite;
				GameObject.Find ("Button1").GetComponent<SpriteRenderer> ().sprite = s;
			} else {
				s = GameObject.Find ("seal").GetComponent<SpriteRenderer> ().sprite;
				GameObject.Find ("Button1").GetComponent<SpriteRenderer> ().sprite = s;
			}

			if (recipe.isFinished && !recipe.isSuccessful) {
				//recipePanel_1.transform.Find ("AcceptButton1").G2tComponent<AcceptButton>().transform.;
				recipePanel_1.transform.Find ("PanelTimer").GetComponent<TextMesh>().text = "FAILED";
			}

			if (recipe.isFinished && recipe.isSuccessful) {
				//recipePanel_1.transform.Find ("AcceptButton1").G2tComponent<AcceptButton>().transform.;
				recipePanel_1.transform.Find ("PanelTimer").GetComponent<TextMesh>().text = "THANKS!";
			}

		} else if (i == 1) {
			
			recipeButton_2.transform.position = recipeButtonStartPos2;
			recipePanel_2.transform.position = recipePanelStartPos2;
			recipePanel_2.transform.Find ("PanelTimer").GetComponent<TextMesh> ().text = "" +  Mathf.RoundToInt(recipeList [i].currentTimer);
			recipePanel_2.transform.Find ("PanelName").GetComponent<TextMesh> ().text = "" + recipeList [i].recipeName;

			Sprite s = GameObject.Find ("Villagers" + recipe.villagerNum).GetComponent<SpriteRenderer> ().sprite;
			recipePanel_2.transform.Find ("PanelPortrait").GetComponent<SpriteRenderer> ().sprite = s;

			if (isThereAnActiveRecipe && i == whichRecipeIsActive) {
				s = GameObject.Find ("HighlightedRecipe").GetComponent<SpriteRenderer> ().sprite;		
				recipePanel_2.GetComponent<SpriteRenderer> ().sprite = s;
			} else {
				s = GameObject.Find ("RecipeBackground").GetComponent<SpriteRenderer> ().sprite;		
				recipePanel_2.GetComponent<SpriteRenderer> ().sprite = s;
			}


			if (recipe.isFinished && !recipe.isSuccessful && recipe.currentTimer <= 0) {
				s = GameObject.Find ("sealBroken").GetComponent<SpriteRenderer> ().sprite;
				GameObject.Find ("Button2").GetComponent<SpriteRenderer> ().sprite = s;
			} else if (recipe.isFinished && recipe.isSuccessful && recipe.currentTimer <= 0) {
				s = GameObject.Find ("sealSuccess").GetComponent<SpriteRenderer> ().sprite;
				GameObject.Find ("Button2").GetComponent<SpriteRenderer> ().sprite = s;
			} else {
				s = GameObject.Find ("seal").GetComponent<SpriteRenderer> ().sprite;
				GameObject.Find ("Button2").GetComponent<SpriteRenderer> ().sprite = s;
			}

			if (recipe.isFinished && !recipe.isSuccessful) {
				recipePanel_2.transform.Find ("PanelTimer").GetComponent<TextMesh>().text = "FAILED";
			}

			if (recipe.isFinished && recipe.isSuccessful) {
				//recipePanel_1.transform.Find ("AcceptButton1").G2tComponent<AcceptButton>().transform.;
				recipePanel_2.transform.Find ("PanelTimer").GetComponent<TextMesh>().text = "THANKS!";
			}

		}else if (i == 2) {

			recipeButton_3.transform.position = recipeButtonStartPos3;
			recipePanel_3.transform.position = recipePanelStartPos3;
			recipePanel_3.transform.Find ("PanelTimer").GetComponent<TextMesh> ().text = "" +  Mathf.RoundToInt(recipeList [i].currentTimer);
			recipePanel_3.transform.Find ("PanelName").GetComponent<TextMesh> ().text = "" + recipeList [i].recipeName +"\n";

			Sprite s = GameObject.Find ("Villagers" + recipe.villagerNum).GetComponent<SpriteRenderer> ().sprite;
			recipePanel_3.transform.Find ("PanelPortrait").GetComponent<SpriteRenderer> ().sprite = s;

			if (isThereAnActiveRecipe && i == whichRecipeIsActive) {
				s = GameObject.Find ("HighlightedRecipe").GetComponent<SpriteRenderer> ().sprite;		
				recipePanel_3.GetComponent<SpriteRenderer> ().sprite = s;
			} else {
				s = GameObject.Find ("RecipeBackground").GetComponent<SpriteRenderer> ().sprite;		
				recipePanel_3.GetComponent<SpriteRenderer> ().sprite = s;
			}

			if (recipe.isFinished && !recipe.isSuccessful && recipe.currentTimer <= 0) {
				s = GameObject.Find ("sealBroken").GetComponent<SpriteRenderer> ().sprite;
				GameObject.Find ("Button3").GetComponent<SpriteRenderer> ().sprite = s;
			} else if (recipe.isFinished && recipe.isSuccessful && recipe.currentTimer <= 0) {
				s = GameObject.Find ("sealSuccess").GetComponent<SpriteRenderer> ().sprite;
				GameObject.Find ("Button3").GetComponent<SpriteRenderer> ().sprite = s;
			} else {
				s = GameObject.Find ("seal").GetComponent<SpriteRenderer> ().sprite;
				GameObject.Find ("Button3").GetComponent<SpriteRenderer> ().sprite = s;
			}

			if (recipe.isFinished && !recipe.isSuccessful) {
				recipePanel_3.transform.Find ("PanelTimer").GetComponent<TextMesh>().text = "FAILED";
			}

			if (recipe.isFinished && recipe.isSuccessful) {
				//recipePanel_1.transform.Find ("AcceptButton1").G2tComponent<AcceptButton>().transform.;
				recipePanel_3.transform.Find ("PanelTimer").GetComponent<TextMesh>().text = "THANKS!";
			}

		}else if (i == 3) {
			recipeButton_4.transform.position = recipeButtonStartPos4;
			recipePanel_4.transform.position = recipePanelStartPos4;
			recipePanel_4.transform.Find ("PanelTimer").GetComponent<TextMesh> ().text = "" +  Mathf.RoundToInt(recipeList [i].currentTimer);
			recipePanel_4.transform.Find ("PanelName").GetComponent<TextMesh> ().text = "" + recipeList [i].recipeName +"\n";
			//recipePanel_2.transform.Find ("AcceptButton1").GetComponent<AcceptButton>().buttonNumber = 3;

			Sprite s = GameObject.Find ("Villagers" + recipe.villagerNum).GetComponent<SpriteRenderer> ().sprite;
			recipePanel_4.transform.Find ("PanelPortrait").GetComponent<SpriteRenderer> ().sprite = s;

			if (recipe.isFinished && !recipe.isSuccessful && recipe.currentTimer <= 0) {
				s = GameObject.Find ("sealBroken").GetComponent<SpriteRenderer> ().sprite;
				GameObject.Find ("Button4").GetComponent<SpriteRenderer> ().sprite = s;
			} else if (recipe.isFinished && recipe.isSuccessful && recipe.currentTimer <= 0) {
				s = GameObject.Find ("sealSuccess").GetComponent<SpriteRenderer> ().sprite;
				GameObject.Find ("Button4").GetComponent<SpriteRenderer> ().sprite = s;
			} else {
				s = GameObject.Find ("seal").GetComponent<SpriteRenderer> ().sprite;
				GameObject.Find ("Button4").GetComponent<SpriteRenderer> ().sprite = s;
			}
			if (isThereAnActiveRecipe && i == whichRecipeIsActive) {
				s = GameObject.Find ("HighlightedRecipe").GetComponent<SpriteRenderer> ().sprite;		
				recipePanel_4.GetComponent<SpriteRenderer> ().sprite = s;
			} else {
				s = GameObject.Find ("RecipeBackground").GetComponent<SpriteRenderer> ().sprite;		
				recipePanel_4.GetComponent<SpriteRenderer> ().sprite = s;
			}

			if (recipe.isFinished && !recipe.isSuccessful) {
				recipePanel_4.transform.Find ("PanelTimer").GetComponent<TextMesh>().text = "FAILED";
			}

			if (recipe.isFinished && recipe.isSuccessful) {
				//recipePanel_1.transform.Find ("AcceptButton1").G2tComponent<AcceptButton>().transform.;
				recipePanel_4.transform.Find ("PanelTimer").GetComponent<TextMesh>().text = "THANKS!";
			}
		}else if (i == 4) {
			recipeButton_5.transform.position = recipeButtonStartPos5;
			recipePanel_5.transform.position = recipePanelStartPos5;
			recipePanel_5.transform.Find ("PanelTimer").GetComponent<TextMesh> ().text = "" +  Mathf.RoundToInt(recipeList [i].currentTimer);
			recipePanel_5.transform.Find ("PanelName").GetComponent<TextMesh> ().text = "" + recipeList [i].recipeName +"\n";
			//recipePanel_2.transform.Find ("AcceptButton1").GetComponent<AcceptButton>().buttonNumber = 4;

			Sprite s = GameObject.Find ("Villagers" + recipe.villagerNum).GetComponent<SpriteRenderer> ().sprite;
			recipePanel_5.transform.Find ("PanelPortrait").GetComponent<SpriteRenderer> ().sprite = s;

			if (recipe.isFinished && !recipe.isSuccessful && recipe.currentTimer <= 0) {
				s = GameObject.Find ("sealBroken").GetComponent<SpriteRenderer> ().sprite;
				GameObject.Find ("Button5").GetComponent<SpriteRenderer> ().sprite = s;
			} else if (recipe.isFinished && recipe.isSuccessful && recipe.currentTimer <= 0) {
				s = GameObject.Find ("sealSuccess").GetComponent<SpriteRenderer> ().sprite;
				GameObject.Find ("Button5").GetComponent<SpriteRenderer> ().sprite = s;
			} else {
				s = GameObject.Find ("seal").GetComponent<SpriteRenderer> ().sprite;
				GameObject.Find ("Button5").GetComponent<SpriteRenderer> ().sprite = s;
			}

			if (isThereAnActiveRecipe && i == whichRecipeIsActive) {
				s = GameObject.Find ("HighlightedRecipe").GetComponent<SpriteRenderer> ().sprite;		
				recipePanel_5.GetComponent<SpriteRenderer> ().sprite = s;
			} else {
				s = GameObject.Find ("RecipeBackground").GetComponent<SpriteRenderer> ().sprite;		
				recipePanel_5.GetComponent<SpriteRenderer> ().sprite = s;
			}

			if (recipe.isFinished && !recipe.isSuccessful) {
				recipePanel_5.transform.Find ("PanelTimer").GetComponent<TextMesh>().text = "FAILED";
			}

			if (recipe.isFinished && recipe.isSuccessful) {
				//recipePanel_1.transform.Find ("AcceptButton1").G2tComponent<AcceptButton>().transform.;
				recipePanel_5.transform.Find ("PanelTimer").GetComponent<TextMesh>().text = "THANKS!";
			}

		}else if (i == 5) {
			recipeButton_6.transform.position = recipeButtonStartPos6;
			recipePanel_6.transform.position = recipePanelStartPos6;
			recipePanel_6.transform.Find ("PanelTimer").GetComponent<TextMesh> ().text = "" +  Mathf.RoundToInt(recipeList [i].currentTimer);
			recipePanel_6.transform.Find ("PanelName").GetComponent<TextMesh> ().text = "" + recipeList [i].recipeName +"\n";
			//recipePanel_2.transform.Find ("AcceptButton1").GetComponent<AcceptButton>().buttonNumber = 5;

			Sprite s = GameObject.Find ("Villagers" + recipe.villagerNum).GetComponent<SpriteRenderer> ().sprite;
			recipePanel_6.transform.Find ("PanelPortrait").GetComponent<SpriteRenderer> ().sprite = s;

			if (recipe.isFinished && !recipe.isSuccessful && recipe.currentTimer <= 0) {
				s = GameObject.Find ("sealBroken").GetComponent<SpriteRenderer> ().sprite;
				GameObject.Find ("Button6").GetComponent<SpriteRenderer> ().sprite = s;
			} else if (recipe.isFinished && recipe.isSuccessful && recipe.currentTimer <= 0) {
				s = GameObject.Find ("sealSuccess").GetComponent<SpriteRenderer> ().sprite;
				GameObject.Find ("Button6").GetComponent<SpriteRenderer> ().sprite = s;
			} else {
				s = GameObject.Find ("seal").GetComponent<SpriteRenderer> ().sprite;
				GameObject.Find ("Button6").GetComponent<SpriteRenderer> ().sprite = s;
			}

			if (recipe.isFinished && !recipe.isSuccessful) {
				recipePanel_6.transform.Find ("PanelTimer").GetComponent<TextMesh>().text = "FAILED";
			}

			if (recipe.isFinished && recipe.isSuccessful) {
				//recipePanel_1.transform.Find ("AcceptButton1").G2tComponent<AcceptButton>().transform.;
				recipePanel_6.transform.Find ("PanelTimer").GetComponent<TextMesh>().text = "THANKS!";
			}
		}
	}
		
	Recipe AddRecipeToWeek(int difficulty){
		Recipe r = new Recipe (difficulty);
		recipeList.Add(r);
		recipesAddedThisWeekCount += 1;
		delayCounter = maxDelayToAddNext;
		return r;
	}

	void StartWeek(){
		
		if (weekNumber == 1) {			

			recipesAddedThisWeekCount = 0;

			//Set Timers			 
			weekTotalTime = 190;
			maxDelayToAddNext = Random.Range (2, 7);

			delayCounter = maxDelayToAddNext;
			totalRecipesToAddThisWeek = totalNumberOfRecipesWeek1;

			weekDifficultyList = new List<int>{	2, 2, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 4, 4, 4, 5 ,5};
		}
	}
}

