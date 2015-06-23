using UnityEngine;
using System.Collections;
using Escape.Core;
using Escape.Rooms;
using Escape.Util;

public class EntranceHallReentry : MonoBehaviour {

	EntranceHallController ehc;
	public BaseDoor mainDoor;

	// Use this for initialization
	void Start () {
		ehc = GetComponentInParent<EntranceHallController> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider other) {
		if (other.tag == "Player") {
			if (ehc.reloaded) {
				Logging.Log ("(EntranceHall) Reentry, re loading children.");
				ehc.ReloadDoors ();
				ehc.reloaded = false; // once per shuffle!
			}
			if (PlayerStatus.canOpenMainDoor()) {
				mainDoor.LoadNextRoom ();
			}
		}
	}
}
