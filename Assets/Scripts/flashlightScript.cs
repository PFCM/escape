using UnityEngine;
using System.Collections;
using Escape.Core;

[RequireComponent(typeof(AudioSource))]
public class flashlightScript : MonoBehaviour {

	// Use this for initialization
	public Light flashlight;
	public double charge = 100;
	public double lossRate = 0.1;
	//brightness of the torch
	public float brightness = 2.0f;
	public bool on = false;
	public bool forceFlicker = false;

	private Color normalColor; 
	public Color highlightColor;

	private AudioSource audioSrc;
	public AudioClip onSound;
	public AudioClip offSound;

	void Start()
	{
		audioSrc = GetComponent<AudioSource> ();
		normalColor =	flashlight.color;
	}
	void Update()
	{
		//drain battery
		if (on == true) {
			charge = charge -lossRate;
			flicker (forceFlicker); //make flashlight flicker randomly, happens more with less battery

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
				PlaySound(on);
			}
		}
		if(charge <=0){
			on =false;
		}
	}

	private void PlaySound(bool on) {
		if (on)
			audioSrc.PlayOneShot (onSound);
		else
			audioSrc.PlayOneShot (offSound);
	}

	public void flicker(bool forceFlicker){
	//makes the flashlight flicker by randomly enabling and disabling.
	//higher chance with lower battery
		double flickerChance = (charge - 100)*-0.04;
		float chance =  Random.Range(0.0f, 100.0f);
		if (flickerChance < 7 && forceFlicker) {
			flickerChance = 7;
		}
		if (chance < flickerChance) {
			flashlight.GetComponent<Light>().intensity = 0;
		} else {
			//smooths transition back to normal light
			if(flashlight.GetComponent<Light>().intensity <brightness){
				flashlight.GetComponent<Light>().intensity =0.1f + flashlight.GetComponent<Light>().intensity*1.5f;
			}
		}
	}

	public void setColorRed(bool setRed){
		if(setRed){
			flashlight.color = highlightColor;
		}
		else{
			flashlight.color = normalColor;
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