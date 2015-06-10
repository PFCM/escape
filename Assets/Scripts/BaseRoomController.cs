using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

			public GameObject[] pickups;
			public PositionableObject[] objects;

			public Transform entrance;

			// has this room been put in its place?
			public bool positioned = false; 

			// how far back to turn off
			public int activeDepth = 3;

			// the room we came from in this particular case
			public BaseRoomController parentRoom = null;

			// the physical door used to come into this room
			private GameObject entranceDoor;

			// list of child rooms to deactivate when we ourselves are deactivated
			private IList<BaseRoomController> children = new List<BaseRoomController> ();


			// Use this for initialization
			void Start ()
			{
				CheckParentRoomStatus ();
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
				
				if (check != null && check != this) {
					check.DisableChildrenExcept(this, this.parentRoom);
					check.gameObject.SetActive (false);
					check.ClearChildren ();
					Logging.Log("(BaseRoomController) disabling a room.");
				}
			}

			// clears the children list, this is used when deactivating rooms as we don't want them to 
			// come back to life in a different place with all their baggage from before
			public void ClearChildren() 
			{
				children.Clear ();
			}

			public void AddChild (BaseRoomController child) 
			{
				children.Add (child);
			}

			public void DisableChildrenExcept(params BaseRoomController[] except) 
			{
				foreach (BaseRoomController child in children) {
					if (!except.Contains(child)) {
						child.ClearChildren ();
						child.gameObject.SetActive(false);
					}
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

			// overriden by subclasses to provide a chance at shuffling the furniture
			public virtual void Shuffle () {

				foreach (BaseDoor bd in doors) {
					bd.collisions = 0;
					bd.loaded = false;
					bd.exitDoorObject.gameObject.SetActive(true);
				}

				
				doorCloseScript[] d = GetComponentsInChildren<doorCloseScript> ();
				foreach (doorCloseScript door in d) {
					// TODO: make this less lame
					// idea is to make sure all doors are reset, but that the door we are 
					// about to walk through is inactive
					door.Reset();
					if (Vector3.Distance(door.transform.position, GetEntrance ().position) < 3f)
						door.gameObject.SetActive(false);
				}
			}

			// informs the controller an item has been picked up
			public abstract void ItemPickedUp(string name);

			// sets the parent room -- this is potentially handy
			public void SetParentRoom (BaseRoomController parent) 
			{
				parentRoom = parent;
				parent.AddChild (this);
			}

			public void SetEntranceDoor (GameObject door) 
			{
				this.entranceDoor = door;
			}

		}
	}
}
