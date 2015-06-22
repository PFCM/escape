using UnityEngine;
using System.Collections;

using Escape.Core;
using Escape.Util;

namespace Escape.Rooms
{
	public class DinningRoomController : BaseRoomController
	{
		
		// Use this for initialization
		void Start ()
		{
			CheckParentRoomStatus ();
			doors [0].SetWeight ("InfiniteHallway1", 1f);
			//doors [1].SetWeight ("Bathroom", 1f);
			//doors [1].SetWeight ("Bedroom", 0.5f);
			//doors [0].SetWeight ("Bathroom", 0.1f);
			//doors [0].SetWeight ("EntranceHall", 0.7f);
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