
using System.Collections;
using UnityEngine;
using Escape.Core;
using Escape.Util;

namespace Escape.Rooms
{
	public class CellController : BaseRoomController
	{
		
		// Use this for initialization
		void Start ()
		{

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
			Logging.Log ("(Cell) shuffled!");
		}
		
		
		public override void ItemPickedUp(string name)
		{
			
		}
	}
}
