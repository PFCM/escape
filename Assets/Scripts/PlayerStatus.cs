using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Escape.Util;
using Escape.Core;

//keys, event chance management
public class PlayerStatus : MonoBehaviour
{
	
	private static int eventChance = 0;
	public PickupableObject holding;
	private static PlayerStatus singleton; // for efficiency/ease of access
	static PlayerStatus instance {
		get { return singleton; }
	}

	public static PickupableObject heldObject {
		get { return instance.holding; }
	}

	// what keys are we holding on to?
	private IDictionary<string, int> keys;

	// have we picked up the flashlight yet?
	public bool haveFlashlight = false; 

	// init the singleton
	void Start ()
	{
		if (singleton == null) {
			singleton = this;
			singleton.keys = new Dictionary<string, int> ();
		} else {
			Logging.Log ("(PlayerStatus) ERROR: initialised more than once.");
		}
	}

	// adds a key to the inventory
	// returns how many of the keys the player has
	public static int AddKey (string name)
	{
		if (instance.keys.ContainsKey (name)) 
			instance.keys [name]++;
		else
			instance.keys [name] = 1;
		return instance.keys [name];
	}

	// checks if the player has a given key
	public static bool HasKey (string name)
	{
		if (name == null || name == "")
			return true;
		return instance.keys.ContainsKey (name) && instance.keys [name] > 0;
	}

	// if a player has a key, rturns true and decrements the count for that key.
	// otherwise returns false
	public static bool UseKey (string name)
	{
		if (instance.keys.ContainsKey (name) && instance.keys [name] > 0) {
			instance.keys [name]--;
			Logging.Log ("(Player) UseKey " + name + " " + instance.keys [name]);
			return true;
		}

		return false;
	}

	public static bool HasFlashlight ()
	{
		return instance.haveFlashlight;
	}

	// makes sure HaveFlashLight is true
	public static void GiveFlashlight ()
	{
		Logging.Log ("(Player) Pickup Flashlight");
		instance.haveFlashlight = true;
	}


	// Make the player hold an object
	public static void GiveObjectToHold (PickupableObject obj)
	{
		if (instance.holding != null) {
			Logging.Log ("(Player) trying to pick up while holding?");
		} else {
			instance.holding = obj;
			obj.transform.SetParent (instance.transform);
			obj.transform.position = instance.transform.position +
				instance.transform.forward * 0.7f +
					instance.transform.up*0.5f;
			Logging.Log ("(Player) now holding: " + obj.name);
		}
	}

	// Take the object the player is holding (if there is anything)
	// this does not reset the parent of the object, rather it sets it to null
	// so that whatever takes the object is responsible for ensuring it
	// gets put back int the riht place in the hierarchy
	// returns null if the player is not holding anything
	public static PickupableObject TakeObjectHeld ()
	{
		if (instance.holding == null)
			return null;
		PickupableObject h = instance.holding;
		h.transform.SetParent (null);
		instance.holding = null;
		return h;
	}

	// is the player holding anything
	public static bool IsHolding ()
	{
		return instance.holding != null;
	}

	// could be static?
	public int getEventChance ()
	{
		return eventChance;
	}
	
	public void addEventChance (int add)
	{
		eventChance = eventChance + add;
	}
}
