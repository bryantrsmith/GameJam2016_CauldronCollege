using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))]
public class DragHandler : MonoBehaviour {

	private Vector3 screenPoint;
	private Vector3 offset;
	public PotContents potContents;
	public WeekManager weekManager;

	public AudioClip pickupSound;
	public AudioClip dropInPotSound;

	public GameObject splashParticle;

	//AudioSource audio;

	void Start(){
		weekManager = FindObjectOfType <WeekManager>();
		//audio = FindObjectOfType<AudioSource> ();
	}

	void OnMouseDown() {

		//glowTexture.GetComponent<Renderer> ().enabled = true;

		//Debug.Log ("ClickedDOWN");
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
		//offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));

		AudioSource.PlayClipAtPoint(pickupSound, transform.position);
	}

	void OnMouseDrag()
	{
		//glowTexture.GetComponent<Renderer> ().enabled = true;
		//Debug.Log ("ClickedDRAG");
		Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		//Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
		Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
		transform.position = curPosition;


	}

		
	public void OnMouseUp(){
		//Debug.Log ("ClickedUP");
				
		Ingredient ingredient = GetComponent<Ingredient> ();

		if (ingredient.isInPot == true && weekManager.isActiveAndEnabled && weekManager.isThereAnActiveRecipe) {


			potContents.AddToPot (ingredient);

			Instantiate (splashParticle, transform.position, Quaternion.identity);

			AudioSource.PlayClipAtPoint(dropInPotSound, transform.position);
		}

		transform.position = GetComponent<Ingredient> ().startPosition;
	}
}
