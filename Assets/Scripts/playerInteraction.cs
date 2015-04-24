using UnityEngine;
using System.Collections;

//this code handles everything to do with player interacting with other objects
public class playerPickUp : MonoBehaviour {



	public Camera camera;
	public float interactionDistance = 3f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	



		RaycastHit hit;
		
		Debug.DrawRay(transform.position, transform.forward * interactionDistance);
		
		if(Physics.Raycast (transform.position, camera.transform.forward, out hit, interactionDistance)) {
	
			//Debug.Log("Hit object " + hit.transform.gameObject.tag);
			if (Input.GetKeyDown (KeyCode.E)) {
				if(hit.transform.gameObject.tag == "battery"){
					//destroys battery object
					Destroy (hit.transform.gameObject);
					gameObject.GetComponent<flashlightScript>().loadBattery();
				}
			}
		} 

	}
}
