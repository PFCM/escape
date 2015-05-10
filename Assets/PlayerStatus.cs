using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Escape.Util;

//keys, event chance management
public class PlayerStatus : MonoBehaviour {
	
	private static int eventChance = 0;


	private static PlayerStatus singleton; // for efficiency/ease of access
	static PlayerStatus instance {
		get { return singleton; }
	}

	// what keys are we holding on to?
	private IDictionary<string, int> keys;

	// have we picked up the flashlight yet?
	public bool haveFlashlight = false; 

	// init the singleton
	void Start () {
		if (singleton == null) {
			singleton = this;
			singleton.keys = new Dictionary<string, int>();
		} else {
			Logging.Log("(PlayerStatus) ERROR: initialised more than once.");
		}
	}

	// adds a key to the inventory
	// returns how many of the keys the player has
	public static int AddKey(string name) {
		if (instance.keys.ContainsKey (name)) 
			instance.keys [name]++;
		else
			instance.keys [name] = 1;
		return instance.keys [name];
	}

	// checks if the player has a given key
	public static bool HasKey(string name) {
		if (name == null || name == "")
			return true;
		return instance.keys.ContainsKey(name) && instance.keys [name] > 0;
	}

	// if a player has a key, rturns true and decrements the count for that key.
	// otherwise returns false
	public static bool UseKey(string name) {
		if (instance.keys.ContainsKey(name) && instance.keys [name] > 0) {
			instance.keys[name]--;
			Logging.Log("(Player) UseKey " + name + " " + instance.keys[name]);
			return true;
		}

		return false;
	}

	public static bool HasFlashlight() {
		return instance.haveFlashlight;
	}

	// makes sure HaveFlashLight is true
	public static void GiveFlashlight() {
		Logging.Log ("(Player) Pickup Flashlight");
		instance.haveFlashlight = true;
	}

	// could be static?
	public int getEventChance(){
		return eventChance;
	}
	
	public void addEventChance(int add){
		eventChance = eventChance + add;
	}
}
