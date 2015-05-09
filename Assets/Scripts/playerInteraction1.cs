using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using Escape.Core;
using Escape.Util;

//this code handles everything to do with player interacting with other objects
public class playerInteraction1 : MonoBehaviour
{
	
	
	
	public Camera camera;
	public float interactionDistance = 3f;

	//GUI stuff
	public Color col = new Color(255,255,255,0);
	private string guiDisplayedText = "";
	private int guiTextChangeTimer = 800;
	private GUIStyle startStyle = new GUIStyle();

	// Use this for initialization
	void Start ()
	{
		startStyle.fontSize = 20;
		startStyle.normal.textColor = Color.white;//(255,255,255);
	}
	
	// Update is called once per frame
	void Update ()
	{

		changeStartGuiText();
		
		RaycastHit hit;


		if (Input.GetButtonDown ("Interact")) {
			//sends out ray to find object player is looking at 
			if (Physics.Raycast (camera.transform.position, camera.transform.forward, out hit, interactionDistance)) {

				//if the object is a battery
				if (hit.transform.gameObject.tag == "battery") {
					//Debug.Log("Hit object " + camera.transform.gameObject.tag);
					//when the user presses E destroy object and load battery
					//destroys battery object
					Destroy (hit.transform.gameObject);
					gameObject.GetComponent<flashlightScript> ().loadBattery ();

				}

				//if(openableObject)
				//object.open

				//if the object is a battery
				if (hit.transform.gameObject.tag == "door") {
					//Debug.Log("Hit object " + camera.transform.gameObject.tag);
					//when the user presses E destroy object and load battery
					//if (Input.GetKeyDown (KeyCode.E) || Input.GetKeyDown (KeyCode.JoystickButton1)) {
					//destroys battery object
					//Destroy (hit.transform.gameObject);
					//gameObject.GetComponent<flashlightScript>().loadBatter
					Debug.Log("hit door");
					hit.transform.gameObject.GetComponent<doorCloseScript> ().activateDoor ();

				
				}

				if (hit.transform.tag == "Key") {
					string name = hit.transform.GetComponent<Key> ().name;
					PlayerStatus.AddKey (name);
					Logging.Log ("(Player) Pickup " + name);
					Destroy (hit.transform.gameObject);
				} else if (hit.transform.tag == "Flashlight") {
					PlayerStatus.GiveFlashlight();
					Destroy(hit.transform.gameObject);
				}
			}
		
		} 
		


		//Longer range ray cast for sight instead of touch. Triggers sprinting.
		if (Physics.Raycast (transform.position, camera.transform.forward, out hit, 10)) {
			if (hit.transform.gameObject.tag == "MonsterChasing") {
				//enrage monster
				if (hit.transform.gameObject.GetComponent<MonsterChase> ().speedUp ()) {
					//start running
					gameObject.GetComponent<FirstPersonController> ().startRunning (500);//().loadBattery();
				}

			}

			if (hit.transform.gameObject.tag == "MonsterSpawnBehind") {
				//CHECK if monster has already been spotted by player
				if (hit.transform.gameObject.GetComponent<MonsterSpawnBehind> ().startChasingPlayer ()) {
					//start running
					gameObject.GetComponent<FirstPersonController> ().startRunning (300);
				}
			}
		}

	}
	private void changeStartGuiText(){
		guiTextChangeTimer --;
		//set opacity and text
		if (guiTextChangeTimer > 600) {
			col.a = col.a + 0.005f;
			guiDisplayedText = "'e' to interact";
		} else {
			if (guiTextChangeTimer < 350 && guiTextChangeTimer >2) {
				col.a = col.a + 0.005f;
				guiDisplayedText = "'f' to turn on flashlight";
			}
			else{
				col.a = col.a - 0.005f;
			}
			
		}
	}
	
	void OnGUI () {
		GUI.color = col;
		GUI.Label (new Rect (500, 250, 500, 500), guiDisplayedText,startStyle);
	}

}
