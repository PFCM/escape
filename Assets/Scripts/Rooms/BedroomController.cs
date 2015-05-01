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