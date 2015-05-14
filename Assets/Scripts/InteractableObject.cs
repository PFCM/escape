using UnityEngine;
using System.Collections;

namespace Escape.Core
{
// This represents an object that doesn't get moved around but can be interacted with in some way
// note that it doesn't have to be a physical object -- could be a position (see paintings in EntranceHall)
	public abstract class InteractableObject : MonoBehaviour
	{
		public abstract void Interact (PickupableObject with=null);
	}
}