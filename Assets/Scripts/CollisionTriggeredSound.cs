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
		private AudioClip lastClip;

		private AudioSource src;

		// Use this for initialization
		void Start ()
		{
			src = GetComponent<AudioSource> ();
		}

		void OnTriggerEnter(Collider other) {
			if (other.tag == "Player") {
				AudioTools.PlayRandomSound(src, clips, lastClip);
			}
		}
	}
}