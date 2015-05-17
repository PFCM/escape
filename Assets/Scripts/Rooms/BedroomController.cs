using UnityEngine;
using System.Collections;

using Escape.Core;
using Escape.Util;

namespace Escape.Rooms
{
	public class BedroomController : BaseRoomController
	{

		// Use this for initialization
		void Start ()
		{
			CheckParentRoomStatus ();
			doors [0].SetWeight ("Hallway2", 1);
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		override public Transform GetEntrance ()
		{
			return base.entrance;
		}

		override public void Shuffle()
		{
			base.Shuffle ();
			CheckParentRoomStatus ();
			objects [0].ChooseRandomPositionUniform ();
		}

		
		public override void ItemPickedUp(string name)
		{
			
		}
	}
}