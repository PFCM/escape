
using System.Collections;
using UnityEngine;
using Escape.Core;
using Escape.Util;

namespace Escape.Rooms
{
	public class LivingRoomController : BaseRoomController
	{
		
		// Use this for initialization
		void Start ()
		{
			doors [0].SetWeight ("Attic", 1);
			doors [0].SetWeight ("HallStairwayDown", 0.5f);
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
			Logging.Log ("(livingroom) shuffled!");
		}
		
		
		public override void ItemPickedUp(string name)
		{
			
		}
	}
}
