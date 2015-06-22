using UnityEngine;
using System.Collections;
using Escape.Util;

public class ChimePuzzleController : MonoBehaviour {

	public GameObject[] chimes; 
	public AudioClip[] successSounds;
	public AudioSource successSource;
	private int currentOrder = 0;

	// Use this for initialization
	void Start () {
		randomizeChimeOrder ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void randomizeChimeOrder(){

		int numOfChimes = chimes.Length;
		int chimeOrder = 0;

		//for (int i =numOfChimes; i<numOfChimes; i++) {
			//if chime hasn't been selected
			//increaes order
			//choose a random chime
		//continue until all chimes are given a number
		while(chimeOrder<chimes.Length){	

			int chosenChime = Mathf.RoundToInt(Random.Range (0,chimes.Length));
			//if it hasnt been set, set it
			if(chimes[chosenChime].GetComponent<ChimeScript>().setOrder(chimeOrder)){
				chimeOrder++;
			}
		}

	}

	public int getCurrentOrder(){
		return currentOrder;
	}

	public void increaseCurrentOrder(){
		currentOrder++;
		if (currentOrder == chimes.Length) {
			puzzleComplete ();
		}
	}

	public void resetPuzzle(){
		currentOrder = 0;
	}

	private void puzzleComplete(){
		print ("player completed puzzle");
		GameObject key = Instantiate(Resources.Load("Keys/mainDoorKey")) as GameObject; 
		key.transform.position = gameObject.transform.position;
		//give a key or unlock some room
		// play a sound
		AudioTools.PlayRandomSound (successSource, successSounds);
	}
}
