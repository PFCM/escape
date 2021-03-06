﻿using UnityEngine;
using System.Collections;
using Escape.Util;
using Escape.Core;

[RequireComponent(typeof(AudioSource))] // needs to make sound
public class doorCloseScript : MonoBehaviour {
	
	public bool open = false;
	public float doorOpenAngle = 90f; //angle the door will be when it is closed
	public float doorCloseAngle = 0f; //angle the door will be when it is open
	public float doorOpenAngleChoice1; //use the choices for selecting what angle it will open at depending on player position. is doorOpenAngle.
	public float doorOpenAngleChoice2; //is doorCloseAngle - 90.
	public float smooth = 2f;
	
	public AudioClip[] closeSounds;
	public AudioClip[] openSounds;
	public AudioClip[] lockedSounds;
	private AudioClip lastSound;
	
	private AudioSource audioSrc;
	
	
	// the name of the key required -- if none, no key
	public string key; 
	
	private int closeDoorTimer = 0;
	
	private float startRotationY;
	
	public bool flippedDoor = false;
	
	public bool isMainDoor = false;
	
	public BaseDoor trigger;
	
	private bool _locked;
	public bool locked {
		get { if(isMainDoor){return !PlayerStatus.canOpenMainDoor();}return _locked; }
		set { 
			Logging.Log ("(BaseDoor) locked: " + value);
			_locked = value;
		}
	}
	
	// Use this for initialization
	void Start () {
		if (doorOpenAngle < 0)
			doorOpenAngle += 360f; // fixes some issues
		if (doorCloseAngle < 0)
			doorCloseAngle += 360f;
		if (doorOpenAngleChoice1 == 0)
			doorOpenAngleChoice1 = doorOpenAngle;
		if (doorOpenAngleChoice2 == 0)
			doorOpenAngleChoice2 = doorCloseAngle - 90;
		startRotationY = transform.localRotation.eulerAngles.y;
		_locked = true;//key != null && key != "";
		
		audioSrc = GetComponent<AudioSource> ();
	}
	
	public void Reset() {
		startRotationY = transform.localRotation.eulerAngles.y;/*
			doorOpenAngle = startRotationY + doorOpenAngle;
		doorCloseAngle = startRotationY + doorCloseAngle;
		
		doorOpenAngleChoice1 += startRotationY;
		doorOpenAngleChoice2 += startRotationY;*/
		open = false;
		//_locked = true;//key != null && key != "";
	}
	
	// Update is called once per frame
	void Update () {
		
		if (open) {
			Quaternion targetRotation = Quaternion.Euler (0, doorOpenAngle, 0);
			transform.localRotation = Quaternion.Slerp (transform.localRotation, targetRotation, smooth * Time.deltaTime);
		} else {
			Quaternion targetRotationClose = Quaternion.Euler(0,doorCloseAngle,0);
			//transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotationClose,0.05f);//Time.deltaTime);
			transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotationClose, smooth * Time.deltaTime);
		}
		
		if (closeDoorTimer == 0 && open == true){ //&& closed == false) {
			open = false;
			lastSound = AudioTools.PlayRandomSound (audioSrc, closeSounds, lastSound);
		}
		else{
			closeDoorTimer--;
		}
		
	}
	
	public bool IsClosed () {
		//Debug.Log (":" + transform.localEulerAngles.y + "," + doorCloseAngle);
		return Mathf.Approximately (transform.localEulerAngles.y,
		                            doorCloseAngle);
	}

	
	public void activateDoor(){
		/*this condition is if the trigger isn't set. 
		Its Just for doors inside of rooms which wont spawn new rooms such as the secret infinite hallway room.*/
		if (!doorMoving () && (trigger == null || trigger.loaded)) {
			// check player has the key
			if (key == "" || key == null)
				_locked = false;
			bool doOpen = !_locked;
			if (!doOpen) {
				doOpen = PlayerStatus.UseKey(this.key);
			}if(isMainDoor){
				
				doOpen = PlayerStatus.canOpenMainDoor();
				print("do oepn " + doOpen);
			}
			if (doOpen) {
				changeOpenAngle ();
				open = !open;
				lastSound = AudioTools.PlayRandomSound (audioSrc, openSounds, lastSound);
				if (open == true) {
					closeDoorTimer = 200;
				}
				locked = false;
				Logging.Log ("(Door) Opened " + key);
			} else {
				lastSound = AudioTools.PlayRandomSound (audioSrc, lockedSounds, lastSound);
				Logging.Log ("(Door) Open fail " + key);
			}
		} 
		else if (isMainDoor && !PlayerStatus.canOpenMainDoor()) {
			lastSound = AudioTools.PlayRandomSound (audioSrc, lockedSounds, lastSound);
			if (_locked)
				GameObject.FindGameObjectWithTag("Player").GetComponent<playerGUIScript>().displayGuiText("The locks on the door require "  
				                                                                                          +(PlayerStatus.getTotalMainDoorKeys()-PlayerStatus.getMainDoorKeys()) + " more keys"); 
			Logging.Log ("(Door) Open fail main door not enough keys");
		}
		else if (key != "" && key != null && !PlayerStatus.HasKey (key)) {
			lastSound = AudioTools.PlayRandomSound (audioSrc, lockedSounds, lastSound);
			if (_locked)
				GameObject.FindGameObjectWithTag("Player").GetComponent<playerGUIScript>().displayGuiText("Maybe I need a key...");
			Logging.Log ("(Door) Open fail " + key);
		}
		
	}
	
	//makes it so it always opens outward from player to avoid clipping
	private void changeOpenAngle(){
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		
		Vector3 directionToTarget = transform.position - player.transform.position;
		float angleToPlayer = Vector3.Angle(transform.forward, directionToTarget);
		
		if(Mathf.Abs(angleToPlayer) < 90){
			doorOpenAngle = doorOpenAngleChoice2;
		}
		else{
			doorOpenAngle = doorOpenAngleChoice1;
		}
		
	}
	
    // checks if the door is in flight
	private bool doorMoving(){
		Debug.Log ("(" + transform.localEulerAngles.y + "," + transform.rotation.eulerAngles.y + ")");
		if ((transform.localEulerAngles.y < doorOpenAngle+10) && (transform.localEulerAngles.y > doorOpenAngle-10 )) {
			return false;
		}
		if((transform.localEulerAngles.y < doorCloseAngle+10) && (transform.localEulerAngles.y > doorCloseAngle-10 )){
			return false;
		}
		//special case for when open rotation is 0
		if(doorOpenAngle == 0 && (transform.localEulerAngles.y < 360+10) && (transform.localEulerAngles.y > 360-10 )){
			return false;
		}
		return true;
	}
	
}