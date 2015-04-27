using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Escape.Util;

namespace Escape
{
	namespace Core
	{
		// This is the base class for all room scripts
		// it provides some helpful functionality for choosing the next room
		// and an interface for properly positioning a room
		public abstract class BaseRoomController : MonoBehaviour
		{
	
			
			public Core.BaseDoor[] doors;

			public Transform entrance;

			// has this room been put in its place?
			public bool positioned = false; 

			// how far back to turn off
			public int activeDepth = 3;

			// the room we came from in this particular case
			protected BaseRoomController parentRoom = null;

			// the physical door used to come into this room
			private GameObject entranceDoor;

			// Use this for initialization
			void Start ()
			{

			}

			// disables a room activeDepth from us, if such a room exists
			public void CheckParentRoomStatus () {
				BaseRoomController check = parentRoom;
				
				for (int i = 0; i < activeDepth; i++) {
					if (check != null && check.parentRoom != null) {
						check = check.parentRoom;
					} else {
						check = null;
						break;
					}
				}
				
				if (check != null) {
					check.gameObject.SetActive (false);
					Logging.Log("(BaseRoomController) disabling a room.");
				}
			}
	
			// Update is called once per frame
			void Update ()
			{
	
			}

			// should return the way the player came into the room
			// (or will come into the room)
			// ideally should be a transform in the center of the doorway
			// at the very edge, facing out
			public abstract Transform GetEntrance ();

			// sets the parent room -- this is potentially handy
			public void SetParentRoom (BaseRoomController parent) 
			{
				parentRoom = parent;
			}

			public void SetEntranceDoor (GameObject door) 
			{
				this.entranceDoor = door;
			}

		}
	}
}
