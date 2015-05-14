using UnityEngine;
using System.Collections;
using Escape.Core;
using Escape.Rooms;
using Escape.Util;

// A position on the wall where a painting should go
public class WallPaintingPosition : InteractableObject {

	EntranceHallController room;

	public PickupableObject correctPainting; // the painting that SHOULD be here
	public PickupableObject currentPainting; // the painting that is here (if any)


	// Use this for initialization
	void Start () {
		room = GetComponentInParent<EntranceHallController> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void Interact(PickupableObject with=null) {
		if (with == null && currentPainting != null) { // then the player should pick up the current painting
			PlayerStatus.GiveObjectToHold (currentPainting);
			currentPainting = null;
		} else if (with != null && currentPainting == null) {
			currentPainting = with;
			currentPainting.transform.SetParent (this.transform);

			currentPainting.transform.position = this.transform.position;
			currentPainting.transform.rotation = this.transform.rotation;
			Logging.Log (string.Format ("(EntranceHall)(Paintings) {0} put in {1} ({2})", 
			                          currentPainting.name, 
			                          this.name,
			                          (currentPainting == correctPainting) ?
			                          "correct" : "wrong"));
			room.PaintingPlaced (this, currentPainting == correctPainting); // tell the room
		} else if (with != null && currentPainting != null) {
			with.Drop(with.transform);
		}
	}
}
