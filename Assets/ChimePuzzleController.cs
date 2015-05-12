using UnityEngine;
using System.Collections;

public class ChimePuzzleController : MonoBehaviour {

	public GameObject[] chimes; 
	private int currentOrder = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void randomizeChimeOrder(){
		int numOfChimes = chimes.Length;
		int chimeOrder = 1;

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
		if (currentOrder >= chimes.Length) {
			puzzleComplete ();
		}
	}

	private void puzzleComplete(){
	//give a key or unlock some room
	}
}
