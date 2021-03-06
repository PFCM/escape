﻿using UnityEngine;
using System.Collections;
using Escape.Core;
using Escape.Util;

namespace Escape.Rooms
{
	public class NurseryController : BaseRoomController
	{
		private SoundControlledForce wind;

		// Use this for initialization
		void Start ()
		{
			base.CheckParentRoomStatus ();
			doors [0].SetWeight ("Attic", 1.5f);
			doors [0].SetWeight ("HallStairwayDown", 1);
			wind = GetComponentInChildren<SoundControlledForce> ();
			wind.UpdateDirection ();
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		public override Transform GetEntrance() 
		{
			return transform;
		}

		public override void ItemPickedUp(string name)
		{
			// do something
			Logging.Log ("(Nursery) Player picked up " + name);
		}

		public override void Shuffle() // TODO: move this into super class, even just as a helper
		{
			base.Shuffle ();
			
			base.CheckParentRoomStatus ();
			foreach (PositionableObject obj in objects) {
				obj.ChooseRandomPositionUniform ();
			}
			wind.UpdateDirection ();
		}
	}
}