using UnityEngine;
using System.Collections;

//does stuff when player is halfway through a hallway in the infinite hallway
public class InfiniteHallwayHalfWayTrigger : MonoBehaviour {
	
	GameObject player;
	public GameObject exitTrigger;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnTriggerEnter(Collider other) 
	{
		//if (!player.GetComponent<PlayerStatus> ().isHallwayMonsterSpawned ()) {
			//create monster behind player
		//}
		//sets the trigger to just behind this game object

		//if it hasnt been triggered before
		if (other.tag == "Player") {
			exitTrigger.transform.position = new Vector3 (exitTrigger.transform.position.x + 2, exitTrigger.transform.position.y, exitTrigger.transform.position.z);
		}
		//start timer to set it as not been triggered before
	}
}
