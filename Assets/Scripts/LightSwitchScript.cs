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

	private Transform lightSwitch;

	void Start()
	{
		audioSrc = GetComponent<AudioSource> ();
		lights = lightGroup.GetComponentsInChildren<Light> ();
		lightSwitch = transform.Find ("Light_Switch_Switch");
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

		if (lightSwitch != null) { // if we found it, get it sorted
			float angle = on? -60f : 60f;
			Logging.Log("" + (int)(transform.localEulerAngles.y+1) / 90);
			int which = (int)(transform.localEulerAngles.y+1) / 90; // which quadrant?
			if (which == 3) { // weird fix for an inexplicable problem
				Debug.Log("Flipping");
				angle *= -1f;
			}
			lightSwitch.RotateAround(lightSwitch.position,
			                         transform.right,
			                         angle);// transform.TransformDirection(Vector3.up));
		//	lightSwitch.Rotate(new Vector3(0f, on? 60f : -60f, 0f));
		}

		Logging.Log (string.Format("(LightSwitch)({0})({1}) {2}", transform.root.name, name, on? "On" : "Off"));
	}

	private void playSound(AudioClip clip) {
		audioSrc.pitch = Random.Range (0.9f, 1.1f); // some slight variation
		audioSrc.clip = clip;
		audioSrc.Play ();
	}
}
