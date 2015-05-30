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
		private AudioClip lastSound;

		public void Init () 
		{
			room = GetComponentInParent<BaseRoomController> ();
			rigidBody = GetComponent<Rigidbody> ();
			audio = GetComponent<AudioSource> ();
		}

		void Start () 
		{
			Init ();
		}

		public virtual void PickUp ()
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
			lastSound = AudioTools.PlayRandomSound (audio, collisionSounds, lastSound);
		}

	}
}