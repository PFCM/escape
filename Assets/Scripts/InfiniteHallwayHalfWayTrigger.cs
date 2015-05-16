using UnityEngine;
using System.Collections;

//does stuff when player is halfway through a hallway in the infinite hallway
public class InfiniteHallwayHalfWayTrigger : MonoBehaviour {
	
	GameObject player;
	private bool triggered = false;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnTriggerEnter(Collider other) 
	{
		if (other.tag == "Player") {
			if (!triggered) {
				GameObject monster = Instantiate (Resources.Load ("Monsters/MonsterHallwayChase")) as GameObject; 
				monster.transform.position = gameObject.transform.position;
				triggered = true;
			}
		}
		//if (!player.GetComponent<PlayerStatus> ().isHallwayMonsterSpawned ()) {
			//create monster behind player
		//}

	
	}
}
