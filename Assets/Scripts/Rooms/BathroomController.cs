
using System.Collections;
using UnityEngine;
using Escape.Core;
using Escape.Util;

namespace Escape.Rooms
{
	public class BathroomController : BaseRoomController
	{

		// Use this for initialization
		void Start ()
		{
			doors [0].SetWeight ("Bedroom", 1);
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
			Logging.Log ("(Bathroom) shuffled!");
		}
	}
}
