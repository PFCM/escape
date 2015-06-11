using UnityEngine;
using System.Collections;
using System;
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
		private float threshold;
		private Vector3 oldVelocity;
		private ContactPoint[] contactPoints;

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
			contactPoints = other.contacts;
		}

		// note that this gets called a lot, it could definitely be more efficient
		void OnCollisionStay(Collision collision)
		{
			// if the contact points have changed we should play a sound
			// because even though we haven't hit something new, we have hit it in a 
			// different way
			bool play = false;//contactPoints.Length != collision.contacts.Length;
			//Debug.Log (string.Format ("{0} prev : {1} new", contactPoints.Length, collision.contacts.Length));
			if (!play) { // if the lengths are the same, check if what's in there is
				foreach (ContactPoint c in collision.contacts) {
					if (!Array.Exists(contactPoints, x => { 
						return Vector3.Distance(x.point, c.point) < 0.2f; // this threshold hsould probably be in relation to the size of the object
					})) { // specifically check positions rather than anything else
						play = true;
						break;
					}
				}
				// if the length is the same and there are no elements in A that aren't in B,
				// then A = B
			}
			if (play) { // we have had a chance to change our mind
				lastSound = AudioTools.PlayRandomSound(audio, collisionSounds, lastSound);
			}
			contactPoints = collision.contacts;
		}

	}
}