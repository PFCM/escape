
using System.Collections;
using UnityEngine;
using Escape.Core;
using Escape.Util;

namespace Escape.Rooms
{
	public class InfiniteHallway2Controller : BaseRoomController
	{
		
		// Use this for initialization
		void Start ()
		{
			//always loads next infinite hallway
			doors [0].SetWeight ("InfiniteHallway3", 1);
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
			//base.Shuffle ();
			Logging.Log ("(InfiniteHallway) shuffled!");
		}
		
		
		public override void ItemPickedUp(string name)
		{
			
		}
	}
}
