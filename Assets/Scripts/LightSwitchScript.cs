using UnityEngine;
using System.Collections;

using Escape.Core;
using Escape.Util;

[RequireComponent(typeof(AudioSource))]
public class LightSwitchScript : InteractableObject {

	public bool on = false;
	public GameObject lightGroup;
	private Light[] lights;

	private AudioSource audioSrc;
	public AudioClip onSound;
	public AudioClip offSound;

	void Start()
	{
		audioSrc = GetComponent<AudioSource> ();
		lights = lightGroup.GetComponentsInChildren<Light> ();
	}


	public override void Interact(PickupableObject with=null){
		if (with != null)
			PlayerStatus.GiveObjectToHold (with);
		//make sound
		on = !on;

		foreach (Light light in lights) {
			light.enabled = on;
		}
		playSound (on ? onSound : offSound);
		Logging.Log (string.Format("(LightSwitch)({0}) {1}", name, on? "On" : "Off"));
	}

	private void playSound(AudioClip clip) {
		audioSrc.pitch = Random.Range (0.9f, 1.1f); // some slight variation
		audioSrc.clip = clip;
		audioSrc.Play ();
	}
}
