using UnityEngine;
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

	private AudioSource audioSrc;

	
	// the name of the key required -- if none, no key
	public string key; 

	private int closeDoorTimer = 0;

	private float startRotationY;

	public bool flippedDoor = false;

	public BaseDoor trigger;

	private bool _locked;
	public bool locked {
		get { return locked; }
		set { 
			Logging.Log ("(BaseDoor) locked: " + value);
			_locked = value;
		}
	}

	// Use this for initialization
	void Start () {
		if (doorOpenAngleChoice1 == 0)
			doorOpenAngleChoice1 = doorOpenAngle;
		if (doorOpenAngleChoice2 == 0)
			doorOpenAngleChoice2 = doorCloseAngle - 90;
		startRotationY = transform.rotation.eulerAngles.y;
		_locked = true;//key != null && key != "";

		audioSrc = GetComponent<AudioSource> ();
	}

	public void Reset() {
		startRotationY = transform.localRotation.eulerAngles.y;
	/*	doorOpenAngle = startRotationY + doorOpenAngle;
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
			playRandomSound(closeSounds);
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
		if (!doorMoving () && trigger == null) {
			// check player has the key
			if (key == "" || key == null)
				_locked = false;
			if (!_locked 
			    || PlayerStatus.UseKey (this.key)) {
				changeOpenAngle ();
				open = !open;
				playRandomSound (openSounds);
				if (open == true) {
					closeDoorTimer = 200;
				}
				_locked = false;
				Logging.Log ("(Door) Opened " + key);
			} else {
				playRandomSound (lockedSounds);
				Logging.Log ("(Door) Open fail " + key);
			}
		}
		else if (!doorMoving () && trigger.loaded) {
			// check player has the key
			if (key == "" || key == null)
				_locked = false;
			bool doOpen = !_locked;
			if (!doOpen) {
				doOpen = PlayerStatus.UseKey(this.key);
			}
			if (doOpen) {
				changeOpenAngle ();
				open = !open;
				playRandomSound (openSounds);
				if (open == true) {
					closeDoorTimer = 200;
				}
				locked = false;
				Logging.Log ("(Door) Opened " + key);
			} else {
				playRandomSound (lockedSounds);
				Logging.Log ("(Door) Open fail " + key);
			}
		} else if (key != "" && key != null && !PlayerStatus.HasKey (key)) {
			playRandomSound (lockedSounds);
			if (_locked)
				GameObject.FindGameObjectWithTag("Player").GetComponent<playerGUIScript>().displayGuiText("Maybe I need a key...");
			Logging.Log ("(Door) Open fail " + key);
		}

	}

	// plays random sound from clip array
	private void playRandomSound(AudioClip[] clips) {
		if (clips.Length > 0) {
			audioSrc.pitch = Random.Range (0.8f, 1.2f); // TODO: fine tune the magic numbers
			audioSrc.PlayOneShot (clips[Random.Range(0,clips.Length)]);
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

	//checks if the door is opening or closing
	/*
	 * changed to localRotation -- seems to fix issues with when rooms get loaded at different angles
	 * paul
	 */
	private bool doorMoving(){
		if ((transform.localRotation.eulerAngles.y < doorOpenAngle+10) && (transform.localRotation.eulerAngles.y > doorOpenAngle-10 )) {
			return false;
		}
		if((transform.localRotation.eulerAngles.y < doorCloseAngle+10) && (transform.localRotation.eulerAngles.y > doorCloseAngle-10 )){
			return false;
	}
		//special case for when open rotation is 0
		if(doorOpenAngle == 0 && (transform.localRotation.eulerAngles.y < 360+10) && (transform.localRotation.eulerAngles.y > 360-10 )){
			return false;
		}
		return true;
	}
	
}
