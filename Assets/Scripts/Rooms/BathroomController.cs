
using System.Collections;
using UnityEngine;
using Escape.Core;

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
	}
}
