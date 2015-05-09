using UnityEngine;
using System.Collections;
using Escape.Core;

public class flashlightScript : MonoBehaviour {

	// Use this for initialization
	public Light flashlight;
	public double charge = 100;
	public double lossRate = 0.1;
	//brightness of the torch
	public float brightness = 2.0f;
	bool on = false;

	void Start()
	{

	}
	void Update()
	{
		//drain battery
		if (on == true) {
			charge = charge -lossRate;
			flicker (); //make flashlight flicker randomly, happens more with less battery

			//reduce brightness if battery low
			if(charge < 40){
				brightness = brightness - 0.003f;
			}
		}

		flashlight.transform.position.Set (transform.position.x, transform.position.y, transform.position.z);
		if(on)
			flashlight.GetComponent<Light>().enabled = true;
		else if(!on)
			flashlight.GetComponent<Light>().enabled = false;
		if (Input.GetKeyDown (KeyCode.F) || Input.GetKeyDown (KeyCode.JoystickButton11)) {
			//check if player has charge
			if(charge <=0 || !PlayerStatus.HasFlashlight()){
				on =false;
			}
			else{
				on = !on;
			}
		}
		if(charge <=0){
			on =false;
		}


			   
		  


	}

	public void flicker(){
	//makes the flashlight flicker by randomly enabling and disabling.
	//higher chance with lower battery
		double flickerChance = (charge - 100)*-0.04;
		float chance =  Random.Range(0.0f, 100.0f);
		if (chance < flickerChance) {
			flashlight.GetComponent<Light>().intensity = 0;
		} else {
			flashlight.GetComponent<Light>().intensity = brightness;
		//	flashlight.intensity = 100;
		}
	}

	public void loadBattery(){
		charge = charge + 50;

		//charge limit
		if (charge > 100) {
			charge = 100;
		}

		//reset brightness
		brightness = 2.0f;
	}
}