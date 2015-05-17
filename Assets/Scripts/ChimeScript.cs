﻿using UnityEngine;
using System.Collections;

public class ChimeScript : MonoBehaviour {

	public int order;
	public GameObject chimePuzzleController;

	// Use this for initialization
	void Start () {
		order = -1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool activate(){
		print("order" +order);
		if (chimePuzzleController.GetComponent<ChimePuzzleController> ().getCurrentOrder () == order) {
			print("correct order" +order);
			chimePuzzleController.GetComponent<ChimePuzzleController> ().increaseCurrentOrder ();
			return true;
		}
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