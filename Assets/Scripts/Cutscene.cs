using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

// does the cut scene the first time the entrance hall is loaded
public class Cutscene : MonoBehaviour {

	public GameObject controller;
	public Camera camera;

	// Use this for initialization
	void Start () {

		controller.GetComponentInChildren<FirstPersonController> ().enabled = false;
		camera.GetComponent<Animator> ().enabled = true;
		camera.GetComponent<Animator> ().Play ("Standup");
		StartCoroutine (WaitUntilDone ());
	}

	IEnumerator WaitUntilDone () {
		yield return new WaitForSeconds (3.0f); // lame, but it's how long the animation is
		camera.GetComponent<Animator> ().Play ("Normal");
		camera.GetComponent<Animator> ().enabled = false;
		ReturnControl ();
	}

	void ReturnControl () {
		controller.GetComponentInChildren<FirstPersonController> ().enabled = true;
	}
}
