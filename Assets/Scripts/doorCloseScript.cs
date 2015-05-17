using UnityEngine;
using System.Collections;
using Escape.Util;
using Escape.Core;

public class doorCloseScript : MonoBehaviour {

	public bool open = false;
	public float doorOpenAngle = 90f; //angle the door will be when it is closed
	public float doorCloseAngle = 0f; //angle the door will be when it is open
	public float smooth = 2f;


	
	// the name of the key required -- if none, no key
	public string key; 

	private int closeDoorTimer = 0;

	private float startRotationY;

	public bool flippedDoor = false;

	public BaseDoor trigger;

	private bool _locked;
	public bool locked {
		get { return locked; }
	}

	// Use this for initialization
	void Start () {
		startRotationY = transform.rotation.eulerAngles.y;
		_locked = key != null && key != "";
	}

	public void Reset() {
		startRotationY = transform.rotation.eulerAngles.y;
		doorOpenAngle = startRotationY + doorOpenAngle;
		doorCloseAngle = startRotationY + doorCloseAngle;
		_locked = key != null && key != "";
	}
	
	// Update is called once per frame
	void Update () {

		if (open) {
			Quaternion targetRotation = Quaternion.Euler (0, doorOpenAngle, 0);
			transform.localRotation = Quaternion.Slerp (transform.localRotation, targetRotation, smooth * Time.deltaTime);
		} else {
			Quaternion targetRotationClose = Quaternion.Euler(0,doorCloseAngle,0);
			//transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotationClose,0.05f);//Time.deltaTime);
			transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotationClose, 200 * Time.deltaTime);
		}

		if (closeDoorTimer == 0 && open == true){ //&& closed == false) {
			open = false;
		}
		else{
			closeDoorTimer--;
		}

	}

	public void activateDoor(){

			if (!doorMoving() && trigger.loaded) {
				// check player has the key
				if (key == "" || key == null || !_locked 
					|| PlayerStatus.UseKey (this.key)) {
					open = !open;
					if(open == true){
						closeDoorTimer = 300;
					}
					_locked = false;
					Logging.Log ("(Door) Opened " + key);
				} else {
					Logging.Log ("(Door) Open fail " + key);
				}
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
