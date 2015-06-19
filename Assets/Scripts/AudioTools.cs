
using UnityEngine;
using System.Collections;

namespace Escape.Util
{
	public class AudioTools
	{
		private AudioTools ()
		{
		}

		/* Plays a random sound from the array. Returns the chosen sound.
		 * Optional third argument is an audio clip to not choose, to avoid
		 * repetition
		 */
		public static AudioClip PlayRandomSound (AudioSource src, AudioClip[] clips, AudioClip except=null, float lower=0.85f, float higher=1.15f) 
		{
			if (clips.Length == 0)
				return null;
			int index = Random.Range (0, clips.Length);
			if (clips [index] == except) { // avoid rejection sampling, by choosing one arbitrarily
				index = (index+1) % clips.Length; // some of the arrays are likely to be small
			}
			src.clip = clips [index];
			src.pitch = Random.Range (lower, higher);
			src.Play ();
			return clips[index];
		}

		/* Returns RMS value of a block of samples */
		public static float GetRMS(float[] samples)
		{
			float sum = 0;
			for (int i = 0; i < samples.Length; i++) {
				sum += samples[i] * samples[i]; // sum of squares
			}
			return Mathf.Sqrt (sum / samples.Length); // root of the mean of the squares :)
		}
	}
}

