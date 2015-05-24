using UnityEngine;
using System.Collections;
using Escape.Core;
using Escape.Util;

// hopefully eventually this will be where bits and pieces get animated opening and closing
// at the moment it just goes a bit wild
[RequireComponent(typeof(Rigidbody))]
public class OpenableObject : InteractableObject {

	Rigidbody rigidBody;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void Interact (PickupableObject with=null)
	{
		if (with != null) {
			PlayerStatus.GiveObjectToHold(with); // no thank you
		}
		rigidBody.AddExplosionForce (50f, GameObject.FindGameObjectWithTag ("Player").transform.position, 5f);
	}
}
