using UnityEngine;
using System.Collections;
using Escape.Core;
using Escape.Util;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
public class OpenablePhysicsDrawers : OpenableObject {

	public Transform[] drawers;
	public float openDistance = 0.2f;

	public AudioClip openSound;
	public AudioClip closeSound;

	private AudioSource src;

	private enum State : int {
		OPENING,
		CLOSING, 
		OPEN,
		CLOSED 
	};
	private State state = State.CLOSED; // state of the current drawer
	private int currentDrawer;
	private Vector3 startPos;

	// Use this for initialization
	void Start () {
	
		src = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void Interact(PickupableObject with=null) {
		if (with != null) {
			PlayerStatus.GiveObjectToHold(with); // ignore it
		}
		// do the thing
		if (state != State.OPENING && state != State.CLOSING) {
			if (state == State.CLOSED) {
				PlaySound (openSound);
				state = State.OPENING;
				startPos = drawers[currentDrawer].position;
				StartCoroutine(MoveDrawer(startPos,
				                          startPos + openDistance * drawers[currentDrawer].forward,
				                          openSound.length,
				                          State.OPEN));
			} else if (state == State.OPEN) {
				state = State.CLOSING;
				PlaySound (closeSound);
				StartCoroutine(MoveDrawer(drawers[currentDrawer].position,
				                          startPos,
				                          closeSound.length,
				                          State.CLOSED,
				                          true));
			}
		}
	}

	// moves the current drawer's x position from from to to in time seconds, leaves state appropriately
	IEnumerator MoveDrawer(Vector3 from, Vector3 to, float time, State finalState, bool increment=false) {
		float counter = 0;
		float step = 1.0f / (time * 10f); // doesn't quite match up with the length of the sounds, but that was awkwardly slow
		//Debug.Log (step);
		while (counter < 1.0f) {
			drawers[currentDrawer].position = Vector3.Lerp(from, to, counter);
			counter += step;
			yield return new WaitForSeconds(0.005f);
		}
		state = finalState;
		if (increment)
			currentDrawer = (currentDrawer + 1) % drawers.Length;
	}

	void PlaySound(AudioClip clip) {
		src.pitch = Random.Range (0.9f, 1.1f);
		src.clip = clip;
		src.Play ();
	}
}
