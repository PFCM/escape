using UnityEngine;
using System.Collections;
using Escape.Core;
using Escape.Util;

public class MazeRoomController : BaseRoomController {
	private MazeGenerator generator;


	// Use this for initialization
	void Start () {
		generator = GetComponent<MazeGenerator> ();
		generator.GenerateMaze ();

		doors [0].SetWeight ("EntranceHall", 1);
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
		base.Shuffle ();
	}
	
	
	public override void ItemPickedUp(string name)
	{
		
	}
}
