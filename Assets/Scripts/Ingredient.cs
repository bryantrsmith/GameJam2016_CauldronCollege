using UnityEngine;
using System.Collections;

public class Ingredient : MonoBehaviour {

	public string ingredientName;

	public Vector3 startPosition;
	public DragHandler dragHandler;

	public bool isInPot = false;

	// Use this for initialization
	void Start () {
		startPosition =	gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter2D(Collider2D coll){
		if (coll.tag == "Pot") {
			isInPot = true;	
		}
	}


	void OnTriggerExit2D(Collider2D coll){
		if (coll.tag == "Pot") {
			isInPot = false;	
		}
	}
}
