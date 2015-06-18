using UnityEngine;
using System.Collections;
using Escape.Util;

namespace Escape
{
	[RequireComponent(typeof(AudioSource))]
	[RequireComponent(typeof(Collider))]
	public class CollisionTriggeredSound : MonoBehaviour
	{
		public AudioClip[] clips;
		public bool onExit = false; // play one of the sounds when collider is exited as well?
		private AudioClip lastClip;

		private AudioSource src;

		// Use this for initialization
		void Start ()
		{
			src = GetComponent<AudioSource> ();
		}

		void OnTriggerEnter(Collider other) {
			if (other.tag == "Player") {
				lastClip = AudioTools.PlayRandomSound(src, clips, lastClip);
			}
		}

		void OnTriggerExit(Collider other) {
			if (other.tag == "Player" && onExit) {
				lastClip = AudioTools.PlayRandomSound(src, clips, lastClip);
			}
		}
	}
}