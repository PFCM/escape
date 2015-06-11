using UnityEngine;
using System.Collections;
using Escape.Util;
using Escape.Core;

// this could be quite handy for figuring out which room the character is actually in and hence figuring out 
// whether or not to do loads
// don't know if can be bothered though
public class CharacterControllerCollisions : MonoBehaviour {
	BaseRoomController currentRoom;
	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		GameObject root = hit.gameObject.transform.root.gameObject;
		BaseRoomController room = PlayerStatus.GetRoomByTag (root.tag);
		if (room!=null && room != currentRoom) { 
			Logging.Log ("Player in room: " + root.tag + "?");
			currentRoom = room;
		}
	}
}
