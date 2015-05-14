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

		public InteractableObject[] paintingPositions;
		private IDictionary<InteractableObject, bool> paintingsCorrect;

		// Use this for initialization
		void Start ()
		{
			// locked door is door0
			this.doors [0].SetWeight ("Bedroom", 1);
			this.doors [1].SetWeight ("Bathroom", 1);


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
		}

		public override void ItemPickedUp(string name)
		{
			if (name.Equals (key)) {
				switchableLight.enabled = true;
			} else if (name.Contains ("WallPainting")) {

			}
		}

		// called by the painting positions to tell the controller if they have the right paintings in them or not
		public void PaintingPlaced(InteractableObject position, bool correct)  
		{
			if (paintingsCorrect.ContainsKey (position)) {
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
				Logging.Log("Congratulations");
			}
		}
	}
}
