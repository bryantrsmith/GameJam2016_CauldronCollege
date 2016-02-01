using UnityEngine;
using System.Collections;

public class rotate : MonoBehaviour {

	private int speed = 1;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (Vector3.forward * speed * Time.deltaTime);
	}
}
