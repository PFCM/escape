	using UnityEngine;
using System.Collections;
using Escape.Util;

namespace Escape.Core
{
	// this is a workaround for more convenient use in the editor
	// due to http://forum.unity3d.com/threads/2d-arrays-in-c.54707/
	[System.Serializable]
	public class AudioClipArray
	{
		public AudioClip[] clips;

		// override array access operator and we can treat an array of these just like a 
		// 2D array
		public AudioClip this [int index]
		{
			get
			{
				return clips [index];
			}
			set
			{
				clips [index] = value;
			}
		}
	};

	// ACTUALLY NEEDS TWO
	[RequireComponent(typeof(AudioSource))]
	public class AmbienceGenerator : MonoBehaviour
	{

		public AudioSource sourceA;
		public AudioSource sourceB;
		private AudioSource playing;
		public float startVolume = 0.05f; // keep this low! This is just for ambience!
		private float volume; // actual volume used by clips
		public float volumeStep = 0.33f; // how far the volume jumps up (as percentage) with intensity
		private float nextVolume; // for volume changes due to bumps in intensity
		public float fadeTime; // As a proportion of the current sound (see ShouldFade, slightly more complex)
		private float fadeLength; // Actual crossfade length in seconds
		private bool fading = false;
		// clips we will choose from
		public AudioClipArray[] clips;

		private int level = 0; // what level of intense?

		private float HALF_PI = 1.57079633f;

		// manually ups the intense, if possible
		public void IncreaseIntensity() 
		{
			level++;
			if (level >= clips.Length)
				level = clips.Length - 1;
			nextVolume = volume * (1.0f + volumeStep * level);
			Logging.Log ("(AmbienceGenerator) intensity level " + level);
			StartCoroutine (FadeVolume ());
		}

		// manually set the level of intensity
		public void SetIntensity(int value)
		{
			level = Mathf.Clamp (value, 0, clips.Length);
			nextVolume = volume * (1.0f + volumeStep * level);
			Logging.Log ("(AmbienceGenerator) intensity level " + level);
			StartCoroutine (FadeVolume ());
		}

		// coroutine to fade the volume up over a couple of seconds
		// just linear, should be fine for this
		// (could easily do smoothstep or smoothdamp, but not convinced
		// it is necessary)
		IEnumerator FadeVolume() 
		{
			Logging.Log ("(AmbienceGenerator) Beginning fade from " + volume + " to " + nextVolume);
			float length = 2.0f; // length of fade in secs
			float timeCounter = 0.0f;
			float oneOverT = 1.0f / length;
			float startVol = volume;
			while (!Mathf.Approximately(timeCounter, length)) {
				timeCounter = Mathf.Clamp (timeCounter + Time.deltaTime, 0.0f, length); // inc counter
				
				volume = Mathf.Lerp(startVol, nextVolume, oneOverT * timeCounter);

				yield return new WaitForSeconds (0.01f);
			}
		}

		// Use this for initialization
		void Awake ()
		{
			nextVolume = startVolume;
			volume = startVolume;

			SetupAudioSource (sourceA);
			SetupAudioSource (sourceB);
			playing = sourceA;
			sourceA.clip = RandomClip ();
			sourceA.Play ();
			sourceA.volume = volume;

			sourceB.clip = RandomClip ();

			Logging.Log (string.Format ("(AmbienceGenerator) Beginning with clip '{0}'", sourceA.clip.name));
		}

		// random clip from the array
		private AudioClip RandomClip ()
		{
			return clips [level][Random.Range (0, clips.Length)];
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
			if (!fading && ShouldFade ()) {
				fading = true;
				StartCoroutine ("Fade");
			}
		}

		// this is a little bit complex,
		// but we should fade if we have <= fadeTime left 
		// as a proportion of the current clip
		// UNLESS that is going to be longer than half the
		// next sound we are going to play, in which
		// case we only fade for fadeTime as a proportion
		// of the new clip
		bool ShouldFade()
		{
			float threshold = playing.clip.length * (1f - fadeTime);
			if (playing.time >= threshold) { // then we have to think about it
				AudioClip other = null;
				if (playing == sourceA)
					other = sourceB.clip;
				else
					other = sourceA.clip;

				if ((playing.clip.length-threshold) > (other.length/2)) {
					// ok, we need to use the next clip to determine the fade time
					threshold = playing.clip.length - (fadeTime * other.length);
					if (playing.time >= threshold) {
						fadeLength = playing.clip.length - threshold;
						return true;
					} // refactor please? Pretty ugly
				} else {
					fadeLength = playing.clip.length - threshold;
					return true;
				}
			}
			return false;
		}

		IEnumerator Fade ()
		{
			// crossfades the two sources
			AudioSource newPlaying = null;
			if (playing == sourceA) { // choose the right source and clip && play
				newPlaying = sourceB;
				newPlaying.Play ();
			} else {
				newPlaying = sourceA;
				newPlaying.Play ();
			}

			Logging.Log (string.Format("(AmbienceGenerator) beginning fade in of '{0}'", newPlaying.clip.name));

			newPlaying.volume = 0.0f; // start at nothing
			float timeCounter = 0.0f;
			float oneOverT = 1.0f / fadeLength;
			while (!Mathf.Approximately(timeCounter, fadeLength)) {
						timeCounter = Mathf.Clamp (timeCounter + Time.deltaTime, 0.0f, fadeLength); // inc counter

				/*playing.volume = (1f - (timeCounter*oneOverT))*volume;
				newPlaying.volume = (timeCounter*oneOverT)*volume;*/
				float p = timeCounter * oneOverT * HALF_PI; // our phase
				playing.volume = volume * (Mathf.Cos(p));
				newPlaying.volume = volume * (Mathf.Sin (p));	

				yield return new WaitForSeconds (0.01f);
			}

			Logging.Log ( ("(AmbienceGenerator) fade complete."));

			// set up a new random clip for next time
			playing.clip = RandomClip ();

			playing = newPlaying; // done
			playing.volume = volume;
			fading = false;
		}
	}
}
