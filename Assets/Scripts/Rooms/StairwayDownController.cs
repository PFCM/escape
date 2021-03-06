﻿using UnityEngine;
using System.Collections;
using Escape.Core;
using Escape.Util;

namespace Escape.Rooms
{
	public class StairwayDownController : BaseRoomController
	{
		
		// Use this for initialization
		void Start ()
		{
			base.CheckParentRoomStatus ();
			doors [0].SetWeight ("EntranceHall", 1);
		
			//doors [0].SetWeight ("DiningRoom", 1);
			//doors [0].SetWeight ("LivingRoom", 1);
		}
		
		// Update is called once per frame
		void Update ()
		{
			
		}
		
		public override Transform GetEntrance() 
		{
			return this.transform;
		}

		public override void Shuffle() 
		{
			base.Shuffle ();
			CheckParentRoomStatus ();
		}
		
		public override void ItemPickedUp(string name) 
		{
			// don't care
		}
	}
}