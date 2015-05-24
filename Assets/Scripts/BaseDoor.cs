using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Escape.Util;

namespace Escape
{
	namespace Core
	{
		// This is the base class for a door
		// it (or a subclass) should be attached to a trigger right before
		// the actual doorway (or somewhere else, depends how you want to to trigger)
		// it handles actually choosing and loading a room when something happens
		// it is up to a subclass to choose if this is in OnTriggerEnter
		// or in response to some other event
		public class BaseDoor : MonoBehaviour
		{
			//Fix for infinite halls not spawning right.
			public bool isInfiniteHallwayDoor = false;

			public Dictionary<string, float> weights;
			// this is the actual door position
			// the center of the doorway, on the very edge, facing out of the room
			public Transform doorPosition;

			private GameObject nextRoom;

			public bool loaded = false;// has the room been loaded?

			public doorCloseScript exitDoorObject;

			// how many collisions are needed before the room is loaded
			public int collisionsRequired = 1;
			public int collisions = 0; // how many have happened?

			void Start () 
			{
				//doorPosition = transform;
				if (weights == null) weights = new Dictionary<string, float> ();
			}

			// chooses a room probabilistically according to the weights
			public BaseRoomController LoadNextRoom () 
			{
				
				loaded = true;
				float sum = 0;
				foreach (float f in weights.Values) 
				{
					sum += f;
				}
				float rand = Random.Range (0f, sum);
				sum = 0; // now we find which one this is in between
				string result = null;
				foreach (KeyValuePair<string, float> entry in weights) 
				{
					sum += entry.Value;
					if (rand < sum) 
					{
						result = entry.Key;
						break; // get outta here, we've found it
					}
				}
				// in theory now result should never be null because Random.Range does not include the max value
				/*GameObject newRoom = Instantiate(Resources.Load (result) as GameObject);
				BaseRoomController newRoomController = newRoom.GetComponentInChildren<BaseRoomController> ();
				newRoomController.SetParentRoom (this.GetComponentInParent<BaseRoomController> ());

				LineUpFacing (newRoom.transform);
				*/
				//this.GetComponentInParent<BaseRoomController> ().gameObject.SetActive (false);
				BaseRoomController room = PlayerStatus.GetRoomByTag (result);

				if (room == null) {
					StartCoroutine (LoadRoomAsLevel (result));
				} else {
					ReloadRoom(room.gameObject);
				}



				return this.GetComponentInParent<BaseRoomController> ();//newRoom.GetComponent<BaseRoomController> ();
			}


			// ensures a room that already exists is re-loaded approprioately into the correct position
			void ReloadRoom (GameObject root) {
				LineUpFacing (root.transform.root);
				root.SetActive (true);
				exitDoorObject.gameObject.SetActive (true);
				BaseRoomController newRoom = root.GetComponentInChildren<BaseRoomController> ();
				newRoom.SetParentRoom (this.GetComponentInParent<BaseRoomController> ());
				newRoom.Shuffle ();
				Logging.Log ("(BaseDoor) Repositioned already loaded room: " + root.tag);
			}

			// this has to happen
			// Unity appears to not actually load the level until the end of the frame
			// meaning that if you call FindX you won't find it, so good idea to 
			// yield until it has actually loaded
			IEnumerator LoadRoomAsLevel (string name) {
				AsyncOperation async = Application.LoadLevelAdditiveAsync (name);
				
				yield return async;

				GameObject[] newStuff = GameObject.FindGameObjectsWithTag (name);//.transform.root.gameObject;
				GameObject newRoom = null;
				foreach (GameObject o in newStuff) {
					if (o.transform.root.GetComponent<BaseRoomController> ().positioned == false) { // must be the new one!
						newRoom = o.transform.root.gameObject;
						break; // there may be lots of them, so jump ship asap
					}
				}

				if (newRoom == null) {
					Debug.LogError("Could not find the room supposedly loaded?");
					return false; 
				}

				BaseRoomController newRoomController = newRoom.GetComponentInChildren<BaseRoomController> ();
				newRoomController.SetParentRoom (this.GetComponentInParent<BaseRoomController> ());
				
				LineUpFacing (newRoom.transform);
				newRoomController.positioned = true;
				this.nextRoom = newRoom;

				exitDoorObject.gameObject.SetActive (true);

				// store in PlayerStatus
				PlayerStatus.AddRoom (newRoomController);
				BaseRoomController thisRoom = GetComponentInParent<BaseRoomController> ();
				if (thisRoom != null) {
					thisRoom.AddChild(newRoomController);
				}

				Logging.Log ("(BaseDoor) loaded " + name);
			}

			// Lines up a given transform so it is colocated with doorPosition, facing the opposite 
			// direction
			public void LineUpFacing (Transform other) 
			{
				other.position = doorPosition.position;
				other.forward = doorPosition.forward;
				//other.RotateAround (other.position, Vector3.up, 180);
				//other.LookAt (doorPosition); // should do the trick???
			}

			// default -- choose a new room when the collider is triggered
			// would have to have an attached collider with 'isTrigger' checked
			// this is here for convenience, some doors may be triggered by other means
			public void OnTriggerEnter(Collider other) 
			{

				//Just so the infinite hallway generates properly
				if (nextRoom != null) {
					if (nextRoom.tag == "InfiniteHallway" || nextRoom.tag == "InfiniteHallway2" ||nextRoom.tag == "InfiniteHallway3"|| nextRoom.tag == "InfiniteHallwaySecret") {
						nextRoom = null;
					}
				}

				if (nextRoom == null) {
					if (other.tag.Equals ("Player")) { // temp tag
						collisions++;
						Logging.Log("(BaseDoor) Collision "+collisions);
						if (collisions >= collisionsRequired && !loaded) {
							if (PlayerStatus.HasKey(exitDoorObject.key)) {
								if ( exitDoorObject.IsClosed ()) {
									LoadNextRoom ();
								}
								else {
									//StartCoroutine(WaitAndLoad(exitDoorObject)); // probably nt a good idea
									Logging.Log("(BaseDoor) Waiting for door to close to load next room.");
								} 
							}
							else 
								Logging.Log("(BaseDoor) Not loaded - no key");
						}
						else if (isInfiniteHallwayDoor){
							//Fix for infinite hallways
							LoadNextRoom ();
						}
					}
				}
			}

			// waits until the door is closed, then loads the next room
			IEnumerator WaitAndLoad(doorCloseScript door) 
			{
				while (door.open) {
					yield return new WaitForSeconds(0.1f);
				}
				LoadNextRoom ();
			}

			// sets a weight, initialises dictionary if this has not already been done
			public void SetWeight(string name, float weight) 
			{
				if (weights == null)
					weights = new Dictionary<string, float> ();

				weights[name] = weight;
			}
		}


	}
}
