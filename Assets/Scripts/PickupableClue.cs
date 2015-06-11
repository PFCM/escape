using UnityEngine;
using System.Collections;

namespace Escape.Core
{
// something you can pick up that shows you a note
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(AudioSource))] // for collision sounds
	public class PickupableClue : PickupableObject
	{
		playerGUIScript gui;

		public string message; // the message to display

		// Use this for initialization
		void Start ()
		{
			base.Init ();
			gui = GameObject.FindGameObjectWithTag ("Player").GetComponent<playerGUIScript> ();
		}
	
		public override void PickUp() 
		{
			base.PickUp ();
			gui.displayGuiText(string.Format("\"{0}\"", message));
		}
	}
}