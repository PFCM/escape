using UnityEngine;
using System.Collections;

public class ChimeScript : MonoBehaviour {

	public int order;
	public GameObject chimePuzzleController;

	public AudioClip failSound;
	public AudioClip succeedSound;
	public AudioSource audioSource;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
		//order = -1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool activate(){
		print("order" +order);
		if (chimePuzzleController.GetComponent<ChimePuzzleController> ().getCurrentOrder () == order) {
			print("correct order" +order);
			chimePuzzleController.GetComponent<ChimePuzzleController> ().increaseCurrentOrder ();
			audioSource.clip = succeedSound;
			audioSource.Play ();
			return true;
		}
		audioSource.clip = failSound;
		audioSource.Play ();
		//player failed puzzle
		chimePuzzleController.GetComponent<ChimePuzzleController> ().resetPuzzle ();
		return false;
	}

	public bool setOrder(int ord){
		if (order == -1) {
			order = ord;
			return true;
		}
		return false;
	}
}
