using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AngerBar : MonoBehaviour {

	public float angerNum;
	public Image angerBarFill;
	float updateNum = 0f;
	bool isNegative = false;
	bool changeAnger = false;

	// Use this for initialization
	void Start () {
		angerBarFill.fillAmount = .01f;
		//updateNum = 0f;
	}
	
	// Update is called once per frame
	void FixedUpdate () {


		if (changeAnger == true) {
			//change bar position
			angerBarFill.fillAmount += updateNum;
			changeAnger = false;
		}
		if (angerBarFill.fillAmount >= 1) { // if anger bar reaches its zenith then go to game over screen
			Application.LoadLevel ("GameOverScene");
		}


	}

	public void UpdateAnger(float num){
		changeAnger = true;
		updateNum += num / 1000;
	}
}
		