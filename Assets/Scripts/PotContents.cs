using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PotContents : MonoBehaviour {

	public List <Ingredient> potIngredients;

	public Text potContents;

	// Use this for initialization
	void Start () {
		EmptyPot ();
	}

	public void EmptyPot(){
		potIngredients = new List<Ingredient>();
		potContents.text = ">>";
	}

	public void AddToPot(Ingredient ingredient){
		potIngredients.Add (ingredient);
		potContents.text += ingredient.ingredientName + System.Environment.NewLine;
	}
}
