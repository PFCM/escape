
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
	}
}

