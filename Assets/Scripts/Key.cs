using UnityEngine;
using System.Collections;

namespace Escape.Core
{
	public class Key : MonoBehaviour
	{
		public string name;

		public void PickUp()
		{
			BaseRoomController controller = GetComponentInParent<BaseRoomController> ();
			if (controller != null) {
				controller.ItemPickedUp (name);
			} else {
				Debug.LogError("Can't find controller");
			}
		}
	}
}