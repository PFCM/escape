using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Escape.Core;
using Escape.Util;

namespace Escape.Rooms
{
	public class EntranceHallController : BaseRoomController
	{
		public Light switchableLight;
		public string key = "EntranceHallKey";

		public Key[] paintingKeys;

		public WallPaintingPosition[] paintingPositions;
		public AudioClip successSound;
		private IDictionary<InteractableObject, bool> paintingsCorrect;
		private bool solved = false;

		// Use this for initialization
		void Start ()
		{
			//this.doors [0].SetWeight ("Stairs", 1);
			//this.doors [0].SetWeight ("HallStairwayUp", 1);
			this.doors [0].SetWeight ("Attic",1);
			//this.doors [0].SetWeight ("HallStairwayDown", 1);
			this.doors [1].SetWeight ("Hallway2", 1);
			this.doors [1].SetWeight ("Bathroom", 1);
			this.doors [2].SetWeight ("Kitchen", 1);
			this.doors [3].SetWeight ("MazeRoom", 1);

			this.doors [1].LoadNextRoom (); // load up the unlocked one

			PlayerStatus.AddRoom (this); // first room

			paintingsCorrect = new Dictionary<InteractableObject, bool> ();
			foreach(InteractableObject i in paintingPositions) {
				WallPaintingPosition w = i as WallPaintingPosition;
				if (w != null) {
					paintingsCorrect[i] = w.correctPainting == w.currentPainting;
				} else {
					paintingsCorrect[i] = false;
				}
			}
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		public override Transform GetEntrance() 
		{
			return this.transform;
		}

		public override void Shuffle() 
		{
			base.Shuffle ();
			// TODO: stop having to fix this case by case
			doors [0].exitDoorObject.gameObject.SetActive (false);
			foreach (BaseDoor door in doors) {
				if (!door.exitDoorObject.locked)
					door.LoadNextRoom();
			}
		}

		public override void ItemPickedUp(string name)
		{
			if (name.Equals (key)) {
				switchableLight.enabled = true;
				doors [0].LoadNextRoom (); // get ahead
			} else if (name.Contains ("WallPainting")) {

			}
		}

		// called by the painting positions to tell the controller if they have the right paintings in them or not
		public void PaintingPlaced(InteractableObject position, bool correct)  
		{
			// do nothing if you've already solved the puzzle
			if (!solved && paintingsCorrect.ContainsKey (position)) {
				paintingsCorrect[position] = correct;
				CheckPaintings();
			}
		}

		// checks to see if the paintings are all in the correct position
		private void CheckPaintings()
		{
			bool win = true;
			foreach(InteractableObject i in paintingsCorrect.Keys) {
				win = win && paintingsCorrect[i];
				//Debug.Log("--" + i.name + "("+paintingsCorrect[i]+")");
			}
			if (win) {
				solved = true;
				doors [1].LoadNextRoom ();
				doors [2].LoadNextRoom ();
				foreach (WallPaintingPosition w in paintingPositions) {
					w.Throw (); // paintings drop to the floor
				}
				foreach (Key k in paintingKeys) {
					k.gameObject.SetActive(true);
				}
				paintingPositions[0].currentPainting.GetComponent<AudioSource> ().PlayOneShot (successSound);
				PlayerStatus.IncreaseAmbientIntensity();
				Logging.Log("Congratulations");
			}
		}
	}
}
