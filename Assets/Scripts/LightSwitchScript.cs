using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Escape.Core;
using Escape.Util;

[RequireComponent(typeof(AudioSource))]
public class LightSwitchScript : InteractableObject {

	public bool onByDefault = false;

	public bool on = false;
	public GameObject lightGroup;
	private Light[] lights;

	private AudioSource audioSrc;
	public AudioClip onSound;
	public AudioClip offSound;

	private Transform lightSwitch;

	private IDictionary<Renderer, Color> emissions;
	private Color black;
	private IList<Renderer> keys;

	void Start()
	{
		black = new Color (0f, 0f, 0f);
		audioSrc = GetComponent<AudioSource> ();
		lights = lightGroup.GetComponentsInChildren<Light> ();
		emissions = new Dictionary<Renderer, Color> ();
		Renderer[] renderers = lightGroup.GetComponentsInChildren<Renderer> ();
		foreach (Renderer r in renderers) {
			if (r.material.HasProperty("_EmissionColor")) // only interested in these ones
				emissions[r] = black;
		}

		lightSwitch = transform.Find ("Light_Switch_Switch");
		if(!onByDefault && Random.Range (0,100)>75){
		Interact (null);
		}
	}


	public override void Interact(PickupableObject with=null){
		if (with != null)
			PlayerStatus.GiveObjectToHold (with);
		//make sound
		on = !on;

		for (int i = 0; i < lights.Length; i++) {
			lights[i].enabled = on;
			// does it have any emissive materials?
		/*	Renderer ren = lights[i].gameObject.GetComponent<Renderer> ();
			if (ren != null) {
				Material mat = ren.material;
				Color emit = mat.GetColor("_Emission");
				mat.SetColor("_Emission", emissions[i]);
				emissions[i] = emit;
			}*/
		}

		 keys = new List<Renderer>(emissions.Keys);
		foreach (Renderer r in keys) {
			Debug.Log("changing material");
			Material mat = r.material;
			Color emit = mat.GetColor("_EmissionColor");
			mat.SetColor("_EmissionColor", emissions[r]);
			r.material = mat;
			emissions[r] = emit;
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
