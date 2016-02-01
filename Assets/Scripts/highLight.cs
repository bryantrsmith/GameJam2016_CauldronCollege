using UnityEngine;
using System.Collections;

public class highLight : MonoBehaviour {

	public GameObject glowTexture;

	void OnMouseEnter(){
		glowTexture.GetComponent<Renderer> ().enabled = true;
		Debug.Log ("Enter");
	}

	void OnMouseExit(){
		Debug.Log ("Exit");
		glowTexture.GetComponent<Renderer> ().enabled = false;
	}

//	void OnTriggerEnter(){
//		glowTexture.GetComponent<Renderer> ().enabled = true;
//	}
//
//	void OnTriggerExit(){
//		glowTexture.GetComponent<Renderer> ().enabled = false;
//	}
}
