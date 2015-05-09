using UnityEngine;
using System.Collections;
using Escape.Util;

public class doorCloseScript : MonoBehaviour {

	
	// the name of the key required -- if none, no key
	public string key; 

	private bool opening = false;
	private bool closing = false;

	private bool closed = true;

	private int closeDoorTimer = 0;

	private float startRotationY;

	public bool flippedDoor = false;

	// Use this for initialization
	void Start () {
		startRotationY = transform.rotation.eulerAngles.y;
	}
	
	// Update is called once per frame
	void Update () {

	if (flippedDoor == false) {
			if (opening == true) {
				//if its open
				if (transform.rotation.eulerAngles.y > startRotationY + 90) {
					//start timer
					closeDoorTimer = 300;
					opening = false;
					closed = false;
				} else {
					transform.Rotate (Time.deltaTime, 1, 0);
				}
			}
			if (closing == true) {
				//if its closed
				if (transform.rotation.eulerAngles.y < startRotationY + 1) {
					closing = false;
					closed = true;
				} else {
					transform.Rotate (Time.deltaTime, -1, 0);
				}
			}
		} else {
			if (opening == true) {
				//if its open
				if (transform.rotation.eulerAngles.y > startRotationY + 89) {
					//start timer
					closeDoorTimer = 150;
					opening = false;
					closed = false;
				} else {
					transform.Rotate (Time.deltaTime, 1, 0);
				}
			}
			if (closing == true) {
				//if its closed
				if (transform.rotation.eulerAngles.y < startRotationY + 1) {
					closing = false;
					closed = true;
				} else {
					transform.Rotate (Time.deltaTime, -1, 0);
				}
			}
		}
		//Sets x and z rotation to 0 because something is causing the x and z rotation to change when they shouldnt be 
		transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

		//close door if timer reaches 0
		if (closeDoorTimer == 0 && closed == false) {
			closing = true;
		}
		else{
			closeDoorTimer--;
		}
	}

	public void activateDoor(){
		//open or close door
		//sets rotation to open
		//starts timer to close or closes when you walk past/exit its range

		if (closed == true) {
			// check player has the key
			if (key.Equals("") || key == null || PlayerStatus.UseKey(this.key)) {
				opening = true;
				Logging.Log("(Door) Opened " + key);
			}
			else {
				Logging.Log("(Door) Open fail " + key);
			}
		} else {
	
			closing = true;
		}
	}
	
}
