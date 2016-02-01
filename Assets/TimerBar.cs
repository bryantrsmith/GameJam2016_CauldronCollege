using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimerBar : MonoBehaviour {

	public Image cooldown;
	public bool coolingDown = false;
	public float waitTime; //ß = 10.0f;

	public float currentTimer;
	public float maxTime = 0;

	void Start () {		
		cooldown.fillAmount = 1.0f;
	}

	// Update is called once per frame
	void Update () 
	{
		if (coolingDown == true)
		{
			//currentTimer -= Time.deltaTime;
			//float per = currentTimer / maxTime * 100;

			//Reduce fill amount over 30 seconds
			cooldown.fillAmount -= .7f/currentTimer * Time.deltaTime;
		}

	}

}
