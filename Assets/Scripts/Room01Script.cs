﻿using UnityEngine;
using System.Collections;

namespace Escape {
	public class Room01Script : Core.BaseRoomController {

		// Use this for initialization
		void Start () {
	
		}
	
		// Update is called once per frame
		void Update () {
	
		}

		override public Transform GetEntrance () {
			return transform;
		}

		override public void Shuffle () {}

		
		public override void ItemPickedUp(string name)
		{
			
		}
	}
}
