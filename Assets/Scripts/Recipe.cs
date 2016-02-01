using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class Recipe : MonoBehaviour {

	public int difficulty; // easy - 4, medium - , hard - , extreme - 

	public float timeLimit; 
	public float currentTimer;

	public int reputationReward;
	public int reputationPenalty;

	public int goldReward;

	public int moodPenalty;
	public int moodReward;

	public int recipeNumber;

	public string recipeName;
	public string recipeDescription;

	public bool isFinished;
	public bool isSuccessful;

	public AudioClip personSound1;
	public AudioClip personSound2;
	public AudioClip personSound3;
	public AudioClip personSound4;
	public AudioClip personSound5;
	public AudioClip personSound6;
	public AudioClip personSound7;
	public AudioClip personSound8;
	public AudioClip personSound9;
	public AudioClip personSound10;

	public List<AudioClip> listPersonSounds = new List<AudioClip>();

	public List <string> listOfIngredients = new List<string>{	
		"Eye of Newt",
		"Hair of Virgin",
		"Wing of Bat",
		"Petal of Rose",
		"Leg of Frog",
		"Wyvern's Scale",
		"Tooth of Wolf",
		"Plume of Swan",
		"Dragon's Heart",
		"Unicorn's Blood", 
		"Tears of Saint"};

	public List <string> listOfRecipeNames = new List<string>{	
		"Introduction of Love Spell",
		"Ritual for Demise of Enemies",
		"Spell of Kindled Lust",
		"Ritual for Eased Childbirth",
		"Ritual of Lasting Wealth",
		"Healing Potion",
		"Call Spirit of Departed",
		"Potion to Induce Sleep",
		"Poison, Severe",
		"Poison, Mild",
		"Balancing of Humors Spell", 
		"Charm for Good Fortune",
		"Blessing on Child",
		"Ritual of Bountiful Harvest",
		"Deadly Curse", 
		"Cure of Mild Inconvenience",
		"Potion of Bees",
		"Charm Knowing Things Hidden",

	};

	public List <string> listOfDescriptions = new List<string>{	
		"Introduction only, \nduration not\nguaranteed.",
		"Rise of self or allies\n not guaranteed.",
		"Radius effect­\n ­Use with caucion.",
		"Amount of relief\n undetermined.",
		"One in 2.33 \nmillion odds.",
		"Restores 20% of \nmax HP.",
		"Spirits likely \nto be grumpy\nat disturbance.",
		"Duration of sleep\nunpredictable, up\nto centuries.",
		"Best used in \ndeposing child kings.",
		"Side effects include\n nausea, bloating,\ncramping, gas \nand severe internal\n bleeding.",
		"Do not attempt jokes.",
		"For others, not self",
		"Mild blessing only,\nbestowing deity \nrandom. No refunds.",
		"Works only on single\n plant.",
		"A horrific death likely.\nBeware of consequences.",
		"Pimples, lost tools\nand cats \nunderfoot.",
		"FRAGILE, HANDLE WITH\n CARE.",
		"Be advised: things are\n often hidden for good \nreason."
	};

	void PopulatePersonSoundList(){
		listPersonSounds.Add (personSound1);
		listPersonSounds.Add (personSound2);
		listPersonSounds.Add (personSound3);
		listPersonSounds.Add (personSound4);
		listPersonSounds.Add (personSound5);
		listPersonSounds.Add (personSound6);
		listPersonSounds.Add (personSound7);
		listPersonSounds.Add (personSound8);
		listPersonSounds.Add (personSound9);
		listPersonSounds.Add (personSound10);
	}

	public int villagerNum;

	public List <string> finalRecipeList = new List<string>();

	public Recipe(int difficulty ) {

		PopulatePersonSoundList ();
		this.difficulty = difficulty;

		RandomlyGenerateIngredients (difficulty, 0);

		int num = Random.Range ( 0, listOfRecipeNames.Count);

		recipeName = listOfRecipeNames[num];

		num = Random.Range ( 0, listOfDescriptions.Count);
		recipeDescription = listOfDescriptions[num];

		//Pick portrait face
		num = Random.Range (0, 19);
		villagerNum = num;

		//Set time limit and start the timer
		int time = difficulty * 6;
		timeLimit = difficulty + Random.Range ( time, time * 1.5f);

		reputationReward = difficulty * 2;
		reputationPenalty = difficulty * 2;
		goldReward = difficulty * 3;
		moodPenalty = difficulty * 2;
		moodReward = difficulty;

		currentTimer = timeLimit;

		isFinished = false;
		isSuccessful = false;

		num = Random.Range (1, listPersonSounds.Count);
		//AudioClip clip = GameObject.Find ("personSound1").GetComponent<AudioClip> ();
		//AudioSource.PlayClipAtPoint(clip, transform.position);
	}

	void Start () {
		PopulatePersonSoundList();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void RandomlyGenerateIngredients(int numIngredientsNeeded, int numOfSpecialActions){
		
		for (int i = 0; i < numIngredientsNeeded; i++) {
			int num = Random.Range (0, 11);
			finalRecipeList.Add (listOfIngredients[num]);
		}
	}
}
