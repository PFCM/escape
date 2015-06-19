using UnityEngine;
using System.Collections;

public class InfiniteRoomEndTrigger : MonoBehaviour {
	private bool triggered;

	// Use this for initialization
	void Start () {
		triggered = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// default -- choose a new room when the collider is triggered
	// would have to have an attached collider with 'isTrigger' checked
	// this is here for convenience, some doors may be triggered by other means
	public void OnTriggerEnter(Collider other){
	if (other.tag == "Player" && !triggered) {
			other.gameObject.GetComponent<PlayerStatus>().stopRunning();
			//stops monster
			if(GameObject.FindGameObjectWithTag("MonsterHallwayChase") !=null){
			GameObject.FindGameObjectWithTag("MonsterHallwayChase").GetComponent<MonsterHallwayChase>().playerSolvedPuzzle();
			}
			triggered = true;
		}
	}
}
