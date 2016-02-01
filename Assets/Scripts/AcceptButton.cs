using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(AudioSource))]
public class AcceptButton : MonoBehaviour {

	public WeekManager weekManager;
	public int buttonNumber;

	public AudioClip acceptSound;

	// Use this for initialization
	void Start () {
		weekManager = FindObjectOfType<WeekManager> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void ResetButton() {
		gameObject.SetActive (true);
	}

	void OnMouseEnter(){
		//Debug.Log ("Recipe Accepted! - Enter");
	}

	void OnMouseUp(){
		//Debug.Log ("Recipe Accepted - UP!");
	}

	void OnMouseDown () {

		//if (!weekManager.isThereAnActiveRecipe) {
			weekManager.activeRecipe = weekManager.recipeList [buttonNumber];
			weekManager.isThereAnActiveRecipe = true;
			weekManager.whichRecipeIsActive = buttonNumber;

			AudioSource.PlayClipAtPoint(acceptSound, transform.position);
		//}
	}
}
