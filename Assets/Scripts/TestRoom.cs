using UnityEngine;
using System.Collections;

namespace Escape
{
	namespace Rooms
	{
		public class TestRoom : Core.BaseRoomController
		{

			// Use this for initialization
			void Start ()
			{
				base.CheckParentRoomStatus ();

				if (doors.Length == 1) {
					doors [0].SetWeight ("TestRoomCube", 1);
					doors [0].SetWeight ("TestRoomCylinder", 1);
				} else if (doors.Length == 2) {
					doors [1].SetWeight ("TestRoomCube", 1);
					doors [0].SetWeight ("TestRoomCylinder", 1);
				}
			}
	
			// Update is called once per frame
			void Update ()
			{
	
			}

			override public Transform GetEntrance () 
			{
				return entrance;
			}
		}
	}
}
