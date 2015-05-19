using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Escape.Util;
using Escape.Core;

//keys, event chance management
public class PlayerStatus : MonoBehaviour
{

	public bool isRunning = false;//used to tell fpscontrollers when player is running
	private int runningTimer = 0;

	private static int eventChance = 0;
	public PickupableObject holding;
	private static PlayerStatus singleton; // for efficiency/ease of access
	static PlayerStatus instance {
		get { return singleton; }
	}

	// holds all the rooms that have been loaded
	private IDictionary<string, BaseRoomController> rooms;

	public static PickupableObject heldObject {
		get { return instance.holding; }
	}

	// what keys are we holding on to?
	private IDictionary<string, int> keys;

	// have we picked up the flashlight yet?
	public bool haveFlashlight = false; 

	public Transform mCamera; // main camera, or something else for OVR?

	// anchor for the object we are holding
	private SpringJoint springJoint;
	private float oldDrag,oldAngularDrag; // data about what we are holding

	// init the singleton
	void Start ()
	{
		if (singleton == null) {
			singleton = this;
			singleton.keys = new Dictionary<string, int> ();
			singleton.rooms = new Dictionary<string, BaseRoomController> ();

			GameObject obj = new GameObject("Dragger");
			obj.transform.SetParent(this.transform);
			obj.transform.Translate(transform.forward);
			Rigidbody body = obj.AddComponent <Rigidbody> () as Rigidbody;
			springJoint = obj.AddComponent <SpringJoint> ();
			body.isKinematic = true;

			if (mCamera == null) // if not set by hand
				mCamera = Camera.main.transform;
		} else {
			Logging.Log ("(PlayerStatus) ERROR: initialised more than once.");
		}
	}

	void Update(){
		
		if (runningTimer == 0) {
			stopRunning ();
		}
		runningTimer--;
	}

	public static void AddRoom(BaseRoomController newRoom) {
		singleton.rooms [newRoom.tag] = newRoom;
	}

	// will return null if room unloaded.
	public static BaseRoomController GetRoomByTag (string tag) {
		if (singleton.rooms.ContainsKey (tag))
			return singleton.rooms [tag];
		return null;
	}

	// This is a coroutine that drags an object around with us
	// mostly thanks to http://answers.unity3d.com/questions/31658/picking-upholding-objects.html
	IEnumerator DragObject(float distance)
	{
		while (this.holding != null) {
			springJoint.transform.position = mCamera.position + mCamera.forward*distance;
			yield return new WaitForFixedUpdate();
		}
		springJoint.connectedBody = null;
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
	// (pickupablobject requires rigidbody so the getComponent is safe)
	public static void GiveObjectToHold (PickupableObject obj)
	{
		if (instance.holding != null) {
			Logging.Log ("(Player) trying to pick up while holding?");
		} else {
			instance.holding = obj;
			//obj.transform.SetParent (instance.transform);
			//obj.transform.position = instance.transform.position +
				//instance.transform.forward * 0.7f +
					//instance.transform.up*0.5f;
			instance.springJoint.transform.position = obj.transform.position;
			Rigidbody connected = obj.GetComponent<Rigidbody> ();
			//Vector3 anchor = instance.transform.TransformDirection(connected.centerOfMass) + 
							// obj.transform.position;
			//anchor = instance.springJoint.transform.InverseTransformPoint(anchor);
			instance.springJoint.anchor = Vector3.zero;//anchor;
			instance.springJoint.autoConfigureConnectedAnchor = true;
			instance.springJoint.connectedBody = connected;
			//instance.springJoint.connectedAnchor = obj.transform.TransformPoint(instance.transform.position);
			connected.isKinematic = false;
			// magic numbers -- make these public variables(probably yes, maybe move the lot to new script)?
			instance.springJoint.spring = 50f;
			instance.springJoint.damper = 5f;
			instance.springJoint.maxDistance = 0.00f;
			instance.springJoint.minDistance = 0f;
			instance.oldDrag = connected.drag;
			instance.oldAngularDrag = connected.angularDrag;
			connected.drag = 7.5f;
			connected.angularDrag = 5f;

			instance.StartCoroutine(instance.DragObject(Vector3.Distance(instance.transform.position, obj.transform.position)));

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
		Rigidbody r = h.GetComponent<Rigidbody> ();
		r.angularDrag = instance.oldAngularDrag;
		r.drag = instance.oldDrag;
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

	public void startRunning(int time){
		isRunning = true;
		runningTimer = time;
	}
	
	public void stopRunning(){
		runningTimer = -1;
		isRunning = false;
	}

	public void killPlayer(){
		//TODO
		//play death animation
		//change scene to first scene
	}
}
