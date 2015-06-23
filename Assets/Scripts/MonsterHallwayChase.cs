using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using Escape.Util;

public class MonsterHallwayChase : MonoBehaviour {
	
	private Vector3 targetPosition;
	public float speed = 5.3f;
	private int despawnTimer = 0; //timer to delete the monster after the puzzle is done
	private GameObject player;
	private AudioSource audioSource;
	public AudioClip[] scream;
	public AudioClip killPlayerScream;

	private float startingY;

	// Use this for initialization
	void Start () {
		startingY = transform.position.y;
		player = GameObject.FindGameObjectWithTag ("Player");
		targetPosition = player.transform.position;
		player.GetComponent<PlayerStatus> ().startRunning (9999); //make player run immediatly
		//plays a sound
		audioSource = gameObject.GetComponent<AudioSource> ();
		AudioTools.PlayRandomSound (audioSource, scream, null, 1.0f, 1.1f);
	}
	
	// Update is called once per frame
	void Update () {
		//may have to check player to see if they completed puzzle here
		speed = 4.5f;
		targetPosition = GameObject.FindGameObjectWithTag ("Player").transform.position;
		transform.position = Vector3.MoveTowards(transform.position, targetPosition,   speed*Time.deltaTime);
		//transform.position = new Vector3(transform.position.x,startingY,transform.position.z);
		transform.position = new Vector3(transform.position.x,targetPosition.y-0.75f,transform.position.z);
		transform.LookAt(GameObject.FindGameObjectWithTag ("Player").transform.position);
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

	public void OnTriggerEnter(Collider other){
		if(other.tag =="Player"){
			audioSource.clip = killPlayerScream;
			audioSource.Play();
			other.GetComponent<PlayerStatus>().killPlayer();
		}
	}
}
