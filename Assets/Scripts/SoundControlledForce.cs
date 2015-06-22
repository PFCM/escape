using UnityEngine;
using System.Collections;
using Escape.Util;

[RequireComponent(typeof(AudioClip))]
public class SoundControlledForce : MonoBehaviour {

	public Cloth leftCurtain;
	public Cloth rightCurtain;
	public int bufferSize = 1024; // the size of the buffer used to calculate RMS
	private float[] samples;
	private AudioSource audio;
	private Vector3[] randomAccelerations;
	private Vector3[] externalAccelerations;
	private bool doUpdate = false;

	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource> ();
		samples = new float[bufferSize];

		// grab the starting values
		randomAccelerations = new Vector3[2];
		externalAccelerations = new Vector3[2];
		randomAccelerations [0] = rightCurtain.randomAcceleration;
		randomAccelerations [1] = leftCurtain.randomAcceleration;
		externalAccelerations [0] = rightCurtain.externalAcceleration;
		externalAccelerations [1] = leftCurtain.externalAcceleration;
		//UpdateDirection ();
	}

	// attempt to update the direction in case the room has moved
	public void UpdateDirection () 
	{
		if (randomAccelerations != null) { // in case this gets called before start
			randomAccelerations [0] = rightCurtain.transform.TransformVector (randomAccelerations [0]);
			randomAccelerations [1] = leftCurtain.transform.TransformVector (randomAccelerations [1]);
			externalAccelerations [0] = rightCurtain.transform.TransformVector (externalAccelerations [0]);
			externalAccelerations [1] = leftCurtain.transform.TransformVector (externalAccelerations [1]);
			Logging.Log ("(SoundControlledForce) Updated direction");
		} else {
			doUpdate = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (doUpdate) {
			UpdateDirection ();
			doUpdate = false;
		}
		audio.GetOutputData (samples, 0); // grab a block of samples
		float vol = AudioTools.GetRMS (samples); // get the volume
		rightCurtain.externalAcceleration = externalAccelerations[0] * vol;
		rightCurtain.randomAcceleration = randomAccelerations[0] * vol;
		leftCurtain.externalAcceleration = externalAccelerations[1] * vol;
		leftCurtain.randomAcceleration = randomAccelerations[1] * vol;
	}
}
