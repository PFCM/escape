using UnityEngine;
using System.Collections;
using Escape.Core;

public class HallwayController : BaseRoomController {

	// Use this for initialization
	void Start () {
		doors [0].SetWeight ("Bathroom", 1);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	override public Transform GetEntrance ()
	{
		return base.entrance;
	}
}
