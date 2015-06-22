using UnityEngine;
using System.Collections;
using System;
using Escape.Core;
using Escape.Util;

// a pickupable object that pitches the sounds based on its scale
public class ScaledPickupableObject : PickupableObject {

	private float pitchMin, pitchMax;
	private bool canPlay = false;

	// Use this for initialization
	void Start () {
		base.Init ();
		float scale = transform.localScale.x;//Vector3.Magnitude (transform.localScale);
		pitchMin = 1.0f / (scale*scale);
		pitchMax = pitchMin * 1.1f;
		pitchMin = pitchMin * 0.9f;

		// don't play for a second after load (cheap hack to stop the initial collide triggering massive amounts of sound)
		StartCoroutine (WaitToPlay ());
	}

	IEnumerator WaitToPlay()
	{
		yield return new WaitForSeconds (1.0f);
		canPlay = true;
	}

	void OnCollisionEnter(Collision other) 
	{
		if (!canPlay)
			return;
		if (other.gameObject.tag != "Player") {
			lastSound = AudioTools.PlayRandomSound (audio, collisionSounds, lastSound, pitchMin, pitchMax);
			contactPoints = other.contacts;
		}
	}
	
	// note that this gets called a lot, it could definitely be more efficient
	void OnCollisionStay(Collision collision)
	{
		if (audio.isPlaying || !canPlay)
			return;
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
					if (c.otherCollider.tag != "Player") {
						play = true;
						break;
					}
				}
			}
			// if the length is the same and there are no elements in A that aren't in B,
			// then A = B
		}
		if (play) { // we have had a chance to change our mind
			lastSound = AudioTools.PlayRandomSound(audio, collisionSounds, lastSound, pitchMin, pitchMax);
		}
		contactPoints = collision.contacts;
	}

}
