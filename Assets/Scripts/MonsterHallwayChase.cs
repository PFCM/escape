using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class MonsterHallwayChase : MonoBehaviour {
	
	private Vector3 targetPosition;
	public float speed = 4.2f;
	private int despawnTimer = 0; //timer to delete the monster after the puzzle is done
	private GameObject player;
	
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		targetPosition = player.transform.position;
		player.GetComponent<PlayerStatus> ().startRunning (60); //make player run immediatly

		
	}
	
	// Update is called once per frame
	void Update () {
		//may have to check player to see if they completed puzzle here
		
		targetPosition = GameObject.FindGameObjectWithTag ("Player").transform.position;
		transform.position = Vector3.MoveTowards(transform.position, targetPosition,   speed*Time.deltaTime);
		
		if (despawnTimer == 1) {
			Destroy (gameObject);
		}
		despawnTimer--;
	}
	
	public bool speedUp(){
		if (speed < 4) {
			speed = 4.2f;
			return true;
		}
		return false;
	}
	
	//when player enters wardrobe/escapes the infinite hallway
	public void playerSolvedPuzzle(){
		speed = 0;
		despawnTimer = 300;
		player.GetComponent<PlayerStatus> ().stopRunning ();
	}
}
