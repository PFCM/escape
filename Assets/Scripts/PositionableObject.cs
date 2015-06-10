using UnityEngine;
using System.Collections;
using Escape.Util;

namespace Escape.Core
{
	public class PositionableObject : MonoBehaviour
	{
	
		// possible positions (position 0 should be the initial position)
		public Transform[] positions;	

		private int index=0;
	
		// Use this for initialization
		void Start ()
		{
			// if you aren't expecting this you get what you deserve
			SetPosition(positions[index]);
		}

		// moves & rotates this object to match t
		private void SetPosition(Transform t) 
		{
			this.transform.position = t.position;
			this.transform.forward = t.forward;
		}

		// chooses a new position, with uniformly distributed probability
		// note that this has an equal chance of choosing the current position
		public void ChooseRandomPositionUniform()
		{
			index = Random.Range (0, positions.Length);
			SetPosition (positions [index]);
		}

		// moves the current posision along by one, wraps when it gets to the end
		public void AdvancePosition() 
		{
			index = (index + 1) % positions.Length;
			SetPosition (positions [index]);
		}
	}
}