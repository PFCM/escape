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
	public Color col = new Color (255, 255, 255, 0);
	private string guiDisplayedText = "";
	private int guiTextChangeTimer = 800;
	private GUIStyle startStyle = new GUIStyle ();

	// object we are colliding (raycast won't hit things we are touching)
	private GameObject colliding;
	private int layerMask;// = ~(1 << 12); // everything except player
	private int noPickupLayerMask;// = layerMask & ~(1 << 11); // everything except player and pickups

	// Use this for initialization
	void Start ()
	{
		layerMask = ~(1 << 12);
		noPickupLayerMask = layerMask & ~(1 << 11);

		startStyle.fontSize = 20;
		startStyle.normal.textColor = Color.white;//(255,255,255);
	}
	
	// Update is called once per frame
	void Update ()
	{

		changeStartGuiText ();
		
		RaycastHit hit;


		if (Input.GetButtonDown ("Interact")) {
			GameObject other = null;
			//sends out ray to find object player is looking at 
			// TODO: layerMask for interactions etc
			// TODO: fix this biz for Oculus
			if (Physics.Raycast (camera.transform.position, 
			                     camera.transform.forward, 
			                     out hit, 
			                     interactionDistance, 
			                     PlayerStatus.IsHolding()? noPickupLayerMask : layerMask)) {
				other = hit.transform.gameObject;
			} else if (colliding != null) {
				if (Vector3.Distance (colliding.transform.position, camera.transform.position) <= interactionDistance) {
					other = colliding;
				}
			}

			if (PlayerStatus.IsHolding() && 
			    (other == null || other.tag != "Interactable")) {
				PickupableObject obj = PlayerStatus.TakeObjectHeld ();
				obj.Drop(this.transform);
			}

			if (other != null) {

				//if the object is a battery
				if (other.tag == "battery") {
					//Debug.Log("Hit object " + camera.transform.gameObject.tag);
					//when the user presses E destroy object and load battery
					//destroys battery object
					Destroy (other);
					gameObject.GetComponent<flashlightScript> ().loadBattery ();
				
				} else if (other.tag == "door") {
					//Debug.Log("Hit object " + camera.transform.gameObject.tag);
					//when the user presses E destroy object and load battery
					//if (Input.GetKeyDown (KeyCode.E) || Input.GetKeyDown (KeyCode.JoystickButton1)) {
					//destroys battery object
					//Destroy (hit.transform.gameObject);
					//gameObject.GetComponent<flashlightScript>().loadBatter
					Debug.Log ("hit door");
					other.GetComponent<doorCloseScript> ().activateDoor ();
				
				
				} else if (other.tag == "Key") {
					Key key = other.GetComponent<Key>();
					key.PickUp();
					string name = key.name;
					PlayerStatus.AddKey (name);
					Logging.Log ("(Player) Pickup " + name);
					Destroy (other);
				} else if (other.tag == "Flashlight") {
					PlayerStatus.GiveFlashlight ();
					Destroy (other);
				} else if (other.tag == "Pickupable") {
					PickupableObject p = other.GetComponent<PickupableObject>();
					if (!PlayerStatus.IsHolding ()) {
						p.PickUp();
						PlayerStatus.GiveObjectToHold(p);
					}
				} else if (other.tag == "Interactable") {
					PickupableObject held = null;
					if (PlayerStatus.IsHolding ())
						held = PlayerStatus.TakeObjectHeld();
					other.GetComponent<InteractableObject> ().Interact (held);	
				}
				else if(other.tag == "Chime"){
					print ("found");
					other.GetComponent<ChimeScript>().activate ();
				}
			}
		
		} 
		


		//Longer range ray cast for sight instead of touch. Triggers sprinting.
		if (Physics.Raycast (transform.position, camera.transform.forward, out hit, 10, layerMask)) {
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

	private void changeStartGuiText ()
	{
		guiTextChangeTimer --;
		//set opacity and text
		if (guiTextChangeTimer > 600) {
			col.a = col.a + 0.005f;
			guiDisplayedText = "press 'x' to interact";
		} else {
			if (guiTextChangeTimer < 350 && guiTextChangeTimer > 2) {
				col.a = col.a + 0.005f;
				guiDisplayedText = "press 'R3' to toggle flashlight";
			} else {
				col.a = col.a - 0.005f;
			}
			
		}
	}
	
	void OnGUI ()
	{
		GUI.color = col;
		GUI.Label (new Rect (500, 250, 500, 500), guiDisplayedText, startStyle);
	}

	void OnTriggerEnter (Collider other)
	{
		colliding = other.gameObject;
	}

	void OnTriggerExit (Collider other)
	{
		colliding = null;
	}

}
