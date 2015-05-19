using UnityEngine;
using System.Collections;
using Escape.Util;

// defines an object the player can pickup in some sense
// currently the only sense is something that can be picked up, one at a time
namespace Escape.Core
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(AudioSource))] // for collision sounds
	public class PickupableObject : MonoBehaviour
	{
		private BaseRoomController room;
		private Rigidbody rigidBody;
		private AudioSource audio;

		public AudioClip[] collisionSounds;

		void Start () 
		{
			room = GetComponentInParent<BaseRoomController> ();
			rigidBody = GetComponent<Rigidbody> ();
			audio = GetComponent<AudioSource> ();
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

		void OnCollisionEnter(Collision other) 
		{
			if (collisionSounds.Length > 0) {
				// twiddle (technical term) the speed a little bit to pretend like we have more samples
				audio.pitch = Random.Range(0.9f,1.1f);
				audio.PlayOneShot(collisionSounds[Random.Range(0,collisionSounds.Length)]);
			}
		}

	}
}