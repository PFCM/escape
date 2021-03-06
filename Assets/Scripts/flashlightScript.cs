﻿using UnityEngine;
using System.Collections;
using Escape.Core;
using Escape.Util;

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
	
	//whether torch flicering or not
	public bool flickering = false;
	public int flickerTimer;
	
	private Color normalColor; 
	private float normalIntensity ;
	public Color highlightColor;
	
	private AudioSource audioSrc;
	public AudioClip onSound;
	public AudioClip offSound;
	public AudioClip[] batteryLoadSounds;
	private AudioClip lastBatteryLoadSound;
	
	void Start()
	{
		flickering = false;
		flickerTimer = 0;
		audioSrc = GetComponent<AudioSource> ();
		normalColor =	flashlight.color;
		normalIntensity =	flashlight.intensity;
	}
	void Update()
	{
		if(flickerTimer <=1){
			flickering = false;
		}
		if(flickerTimer >0){
			flickerTimer--;
		}
		
		//drain battery
		if (on == true) {
			charge = charge -lossRate;
			
			flicker (flickering);
			
			//reduce brightness if battery low
			if(charge < 40){
				brightness = brightness - 0.004f;
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
		double flickerChance = 0;//(charge - 100)*-0.04;
		float chance =  Random.Range(0.0f, 100.0f);
		if (flickerChance < 7 && forceFlicker) {
			flickerChance = 7;
		}
		if (chance < flickerChance) {
			flashlight.GetComponent<Light>().intensity = 0;
			flashlight.GetComponent<Light>().intensity =- flashlight.GetComponent<Light>().intensity*0.5f;
		} else {
			//smooths transition back to normal light
			if(flashlight.GetComponent<Light>().intensity <normalIntensity){
				flashlight.GetComponent<Light>().intensity =0.1f + flashlight.GetComponent<Light>().intensity*1.1f;
			}
		}
	}
	
	public void setColorRed(bool setRed){
		if(setRed){
			flashlight.color = new Color(flashlight.color.r,flashlight.color.g-0.05f,flashlight.color.b-0.05f); //Color.red;
			flashlight.color = highlightColor;
		}
		else{
			//flashlight.color = normalColor;
			if(flashlight.color.b < normalColor.b && flashlight.color.g < normalColor.g){
				flashlight.color = new Color(flashlight.color.r,flashlight.color.g+0.05f,flashlight.color.b+0.05f);
			}
		}
	}
	
	public void flickerOnce(){
		flickering = true;
		if(flickerTimer <1){
			flickerTimer = 60;
		}
	}
	
	
	public void loadBattery(){
		charge = charge + 50;
		
		//charge limit
		if (charge > 100) {
			charge = 100;
		}
		
		//reset brightness
		brightness = normalIntensity; //2.0f;
		Logging.Log ("(Flashlight) charge " + charge);
		lastBatteryLoadSound = AudioTools.PlayRandomSound (audioSrc, this.batteryLoadSounds, this.lastBatteryLoadSound);
	}
}