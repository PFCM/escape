using UnityEngine;
using System.Collections;
using Escape.Util;

// defines an object the player can pickup in some sense
// currently the only sense is something that can be picked up, one at a time
namespace Escape.Core
{
	[RequireComponent(typeof(Rigidbody))]
	public class PickupableObject : MonoBehaviour
	{
		private BaseRoomController room;
		private Rigidbody rigidBody;

		void Start () 
		{
			room = GetComponentInParent<BaseRoomController> ();
			rigidBody = GetComponent<Rigidbody> ();
		}

		public void PickUp ()
		{
			rigidBody.isKinematic = true;
			room.ItemPickedUp (name);
		}

		public void Drop (Transform from)
		{
			rigidBody.isKinematic = false;
			transform.SetParent (room.transform);
			rigidBody.AddForce(from.forward * 100f);// fine tune the magic number
		}
	}
}