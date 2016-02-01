using UnityEngine;
using System.Collections;

public class SplashParticle : MonoBehaviour {

	public float lifetime = 0;

	// Use this for initialization
	void Start () {

		//particleSystem.renderer.sortingLayerName = "Foreground";

		Destroy (gameObject, lifetime);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
