﻿using UnityEngine;
using System.Collections;
using Escape.Core;
using Escape.Util;

public class MazeRoomController : BaseRoomController {
	private MazeGenerator generator;

	// chance of a wandering monster


	// Use this for initialization
	void Start () {
		generator = GetComponent<MazeGenerator> ();
		generator.GenerateMaze ();

		PlayerStatus.SetAmbientIntensity (999);

		doors [0].SetWeight ("Cell", 1);
	}

	// Update is called once per frame
	void Update () {
		
	}

	override public Transform GetEntrance ()
	{
		return base.entrance;
	}
	
	override public void Shuffle() 
	{
		generator.GenerateMaze ();
		base.Shuffle ();
	}
	
	
	public override void ItemPickedUp(string name)
	{
		
	}
}
