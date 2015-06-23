
using System.Collections;
using UnityEngine;
using Escape.Core;
using Escape.Util;

namespace Escape.Rooms
{
	public class LivingRoomController : BaseRoomController
	{
		public GameObject unopenableDoor;
		private bool hasKey = false;
		private bool reLoaded = false;
		// Use this for initialization
		void Start ()
		{
			base.CheckParentRoomStatus ();
			doors [0].SetWeight ("Attic", 1);
			doors [0].SetWeight ("HallStairwayDown", 0.5f);
			doors [1].gameObject.SetActive (false);
		}
		
		// Update is called once per frame
		void Update ()
		{

		}

		// makes sure the player can get out the door, if they've got the key and this is the second time they've visited
		void AllowOut() {
			if (reLoaded && hasKey) {
				unopenableDoor.SetActive(false);
				doors[1].gameObject.SetActive(true);
				doors[1].SetWeight("HallStairwayDown", 1);
				doors[1].SetWeight("Nursery", 0.2f);
				doors [1].exitDoorObject.gameObject.SetActive(true);
				Logging.Log ("(LivingRoom) Player allowed out.");
			}
		}
		
		override public Transform GetEntrance ()
		{
			return base.entrance;
		}
		
		override public void Shuffle() 
		{
			base.Shuffle ();
			
			base.CheckParentRoomStatus ();
			Logging.Log ("(livingroom) shuffled!");
			reLoaded = true;
			AllowOut ();
		}
		
		
		public override void ItemPickedUp(string name)
		{
			if (name == "LivingRoomKey") {
				hasKey = true;
			}
			AllowOut ();
		}
	}
}
