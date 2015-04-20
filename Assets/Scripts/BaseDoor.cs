using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

			public Dictionary<string, float> weights;
			// this is the actual door position
			// the center of the doorway, on the very edge, facing out of the room
			public Transform doorPosition;

			private GameObject nextRoom;

			void Start () 
			{
				doorPosition = transform;
				if (weights == null) weights = new Dictionary<string, float> ();
			}

			// chooses a room probabilistically according to the weights
			public BaseRoomController LoadNextRoom () 
			{
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
				GameObject newRoom = Instantiate(Resources.Load (result) as GameObject);
				BaseRoomController newRoomController = newRoom.GetComponentInChildren<BaseRoomController> ();
				newRoomController.SetParentRoom (this.GetComponentInParent<BaseRoomController> ());

				LineUpFacing (newRoom.transform);

				//this.GetComponentInParent<BaseRoomController> ().gameObject.SetActive (false);

				return newRoom.GetComponent<BaseRoomController> ();
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
				if (nextRoom == null) {
					if (other.tag.Equals ("Player")) { // temp tag
						nextRoom = LoadNextRoom ().gameObject;
					}
				}
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
