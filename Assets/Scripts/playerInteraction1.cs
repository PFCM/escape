using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

//this code handles everything to do with player interacting with other objects
public class playerInteraction1 : MonoBehaviour {
	
	
	
	public Camera camera;
	public float interactionDistance = 3f;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		
		
		
		RaycastHit hit;


		//sends out ray to find object player is looking at 
		if(Physics.Raycast (transform.position, camera.transform.forward, out hit, interactionDistance)) {

			//if the object is a battery
			if(hit.transform.gameObject.tag == "battery"){
			//Debug.Log("Hit object " + camera.transform.gameObject.tag);
			//when the user presses E destroy object and load battery
			if (Input.GetKeyDown (KeyCode.E)) {
					//destroys battery object
					Destroy (hit.transform.gameObject);
					gameObject.GetComponent<flashlightScript>().loadBattery();
				}
			}

			//if(openableObject)
			//object.open

			//if the object is a battery
			if(hit.transform.gameObject.tag == "door"){
				//Debug.Log("Hit object " + camera.transform.gameObject.tag);
				//when the user presses E destroy object and load battery
				if (Input.GetKeyDown (KeyCode.E) || Input.GetKeyDown (KeyCode.JoystickButton1)) {
					//destroys battery object
					//Destroy (hit.transform.gameObject);
					//gameObject.GetComponent<flashlightScript>().loadBatter
						hit.transform.gameObject.GetComponent<doorCloseScript>().activateDoor();

				}
			}

		
		} 


		//Longer range ray cast for sight instead of touch. Triggers sprinting.
		if (Physics.Raycast (transform.position, camera.transform.forward, out hit, 10)) {
			if(hit.transform.gameObject.tag == "MonsterChasing"){
				//enrage monster
				hit.transform.gameObject.GetComponent<MonsterChase>().speedUp();
				//start running
				gameObject.GetComponent<FirstPersonController>().startRunning(500);//().loadBattery();
			}

			if(hit.transform.gameObject.tag == "MonsterSpawnBehind"){
				//enrage monster
				hit.transform.gameObject.GetComponent<MonsterSpawnBehind>().startChasingPlayer();
				//start running
				gameObject.GetComponent<FirstPersonController>().startRunning(500);//().loadBattery();
			}
		}

	}
}
