using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Escape
{
	namespace Core
	{
		// This is the base class for all room scripts
		// it provides some helpful functionality for choosing the next room
		// and an interface for properly positioning a room
		public abstract class BaseRoomController : MonoBehaviour
		{
	
			protected List<BaseDoor> doors;

			// the room we came from in this particular case
			protected BaseRoomController parentRoom = null;

			// Use this for initialization
			void Start ()
			{
	
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

		}
	}
}
