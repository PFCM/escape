using UnityEngine;
using System.Collections;
using Escape.Core;
using Escape.Util;

namespace Escape.Rooms
{
	public class NurseryController : BaseRoomController
	{

		// Use this for initialization
		void Start ()
		{
			doors [0].SetWeight ("Attic", 1);
			doors [0].SetWeight ("HallStairwayDown", 1);
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
			foreach (PositionableObject obj in objects) {
				obj.ChooseRandomPositionUniform ();
			}
		}
	}
}