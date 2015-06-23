using UnityEngine;
using System.Collections;
using Escape.Core;
using Escape.Util;

public class HallwayController : BaseRoomController {

	// the door into this hallway, needs to be deactivated on reload
	public GameObject firstDoor;

	private LightSwitchScript[] switches;
	private bool solved = false;

	// Use this for initialization
	void Start () {
		doors [0].SetWeight ("Bathroom", 1);
		doors [1].SetWeight ("EntranceHall", 1);

		switches = GetComponentsInChildren<LightSwitchScript> ();
		//StartCoroutine (LightSwitchCheck());

		this.CheckParentRoomStatus ();
	}

	IEnumerator LightSwitchCheck() {
		while (true) {
			// check switches
			bool off = true;
			foreach (LightSwitchScript ls in switches) {
				off = off && !ls.on;
			}
			if (off) {
				doors[1].LoadNextRoom ();
				doorCloseScript door = doors[1].exitDoorObject;
				PlayerStatus.AddKey(door.key);
				door.activateDoor ();
				break;
			}
			yield return new WaitForSeconds(0.2f);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!solved) {
			bool off = true;
			foreach (LightSwitchScript ls in switches) {
				off = off && !ls.on;
			}
			if (off) {
				doors [1].LoadNextRoom ();
				doorCloseScript door = doors [1].exitDoorObject;
				PlayerStatus.AddKey (door.key);
				door.activateDoor ();
				solved = true;
			}
		}
	}

	override public Transform GetEntrance ()
	{
		return base.entrance;
	}

	// called on reload
	override public void Shuffle () 
	{
		base.Shuffle ();
		Logging.Log ("(Hallway) Shuffled");
		if (solved && this.parentRoom.tag != "EntranceHall") {
			doors[1].LoadNextRoom (); 
			this.parentRoom.gameObject.SetActive(true); // hilarious last minute fix
		}
		
		this.CheckParentRoomStatus ();
	}

	
	public override void ItemPickedUp(string name)
	{
		
	}
}
