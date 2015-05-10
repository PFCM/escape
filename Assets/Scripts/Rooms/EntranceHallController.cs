using UnityEngine;
using System.Collections;
using Escape.Core;
using Escape.Util;

namespace Escape.Rooms
{
	public class EntranceHallController : BaseRoomController
	{

		// Use this for initialization
		void Start ()
		{
			// locked door is door0
			this.doors [0].SetWeight ("Bedroom", 1);
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

		}
	}
}
