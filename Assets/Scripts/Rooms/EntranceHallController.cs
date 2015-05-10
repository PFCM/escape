using UnityEngine;
using System.Collections;
using Escape.Core;
using Escape.Util;

namespace Escape.Rooms
{
	public class EntranceHallController : BaseRoomController
	{
		public Light switchableLight;
		public string key = "EntranceHallKey";

		// Use this for initialization
		void Start ()
		{
			// locked door is door0
			this.doors [0].SetWeight ("Bedroom", 1);
			this.doors [1].SetWeight ("Bathroom", 1);
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
		}

		public override void ItemPickedUp(string name)
		{
			if (name.Equals (key)) {
				switchableLight.enabled = true;
			}
		}
	}
}
