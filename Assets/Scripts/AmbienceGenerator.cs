	using UnityEngine;
using System.Collections;
using Escape.Util;

namespace Escape.Core
{
// ACTUALLY NEEDS TWO
	[RequireComponent(typeof(AudioSource))]
	public class AmbienceGenerator : MonoBehaviour
	{

		public AudioSource sourceA;
		public AudioSource sourceB;
		private AudioSource playing;
		public float volume = 0.05f; // keep this low! This is just for ambience!
		public float fadeTime = 2.0f; // IN SECONDS
		private bool fading = false;
		// actual clips we will choose from
		public AudioClip[] clips;

		private float HALF_PI = 1.57079633f;

		// Use this for initialization
		void Awake ()
		{
			SetupAudioSource (sourceA);
			SetupAudioSource (sourceB);
			playing = sourceA;
			sourceA.clip = RandomClip ();
			sourceA.Play ();
			sourceA.volume = volume;

			Logging.Log (string.Format ("(AmbienceGenerator) Beginning with clip '{0}'", sourceA.clip.name));
		}

		// random clip from the array
		private AudioClip RandomClip ()
		{
			return clips [Random.Range (0, clips.Length)];
		}

		private void SetupAudioSource (AudioSource a)
		{
			a.bypassEffects = true;
			a.bypassListenerEffects = true;
			a.bypassReverbZones = true;
			a.loop = false;
			a.mute = false;
			a.loop = false;
			a.spatialBlend = 0;
			a.volume = 0.0f;
		}
	
		// Update is called once per frame
		void Update ()
		{
			// do we need to fade yet?
			if ((playing.clip.length - playing.time) <= fadeTime && !fading) {
				fading = true;
				StartCoroutine ("Fade");
			}
		}

		IEnumerator Fade ()
		{
			// crossfades the two sources
			AudioSource newPlaying = null;
			if (playing == sourceA) { // choose the right source and clip && play
				newPlaying = sourceB;
				newPlaying.clip = RandomClip ();
				newPlaying.Play ();
			} else {
				newPlaying = sourceA;
				newPlaying.clip = RandomClip ();
				newPlaying.Play ();
			}

			Logging.Log (string.Format("(AmbienceGenerator) beginning fade in of '{0}'", newPlaying.clip.name));

			newPlaying.volume = 0.0f; // start at nothing
			float timeCounter = 0.0f;
			float oneOverT = 1.0f / fadeTime;
			while (!Mathf.Approximately(timeCounter, fadeTime)) {
				timeCounter = Mathf.Clamp (timeCounter + Time.deltaTime, 0.0f, fadeTime); // inc counter

				/*playing.volume = (1f - (timeCounter*oneOverT))*volume;
				newPlaying.volume = (timeCounter*oneOverT)*volume;*/
				float p = timeCounter * oneOverT * HALF_PI; // our phase
				playing.volume = volume * (Mathf.Cos(p));
				newPlaying.volume = volume * (Mathf.Sin (p));	

				yield return new WaitForSeconds (0.01f);
			}

			Logging.Log ( ("(AmbienceGenerator) fade complete."));

			playing = newPlaying; // done
			playing.volume = volume;
			fading = false;
		}
	}
}
