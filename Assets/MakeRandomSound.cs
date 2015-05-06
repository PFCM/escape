using UnityEngine;
using System.Collections;

public class MakeRandomSound : MonoBehaviour {
	
	//SOUNDS
	public AudioClip[] soundList  = new AudioClip[20];//= {"fastFootsteps","whisper1","cry"};
	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		//choose sound
		int chosenIndex = Mathf.RoundToInt (Random.Range (0, Mathf.RoundToInt(soundList.Length)));
		AudioClip chosenSound = soundList [chosenIndex];
		//play sound
		audioSource = gameObject.GetComponent<AudioSource> ();
		audioSource.clip = chosenSound;
		audioSource.Play();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
