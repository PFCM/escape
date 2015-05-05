using UnityEngine;
using System.Collections;
using Escape.Core;
using Escape.Util;

public class HallwayController : BaseRoomController {

	// the door into this hallway, needs to be deactivated on reload
	public GameObject firstDoor;

	// Use this for initialization
	void Start () {
		doors [0].SetWeight ("Bathroom", 1);

		this.CheckParentRoomStatus ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	override public Transform GetEntrance ()
	{
		return base.entrance;
	}

	// called on reload
	override public void Shuffle () 
	{
		Logging.Log ("(Hallway) Shuffled");
		
		this.CheckParentRoomStatus ();
	}
}
