using UnityEngine;
using System.Collections;
using Escape.Util;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AudioSource))]
public class MazeMonster : MonoBehaviour {

	private GameObject player;
	public float acquisitionDistance; // the distance at which we start chasing
	public bool chasing = false; // starts off wandering, not chasing
	public bool firstChase = true;
	public Transform[] waypoints; // Wanders by choosing random waypoints
	private NavMeshAgent agent;
	private int currentWaypoint = 0;
	public AudioClip[] chaseSounds;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		agent = GetComponent<NavMeshAgent> ();
		currentWaypoint = Random.Range (0, waypoints.Length);
		agent.destination = waypoints [currentWaypoint].position;

	}
	
	// Update is called once per frame
	void Update () {
		// check if player is in line of sight
		RaycastHit hit;
		if (Physics.Raycast (transform.position, 
		                    Vector3.Normalize (-transform.position + player.transform.position), 
		                    out hit, 
		                    acquisitionDistance)) {
			chasing = hit.collider.tag == "Player";
			if(chasing) {
				Logging.Log("(MazeMonster) Now chasing player");
				if (firstChase) {// first time
					AudioSource source = GetComponent<AudioSource> ();
					AudioTools.PlayRandomSound(source, chaseSounds);
					firstChase = false;
				}
			}
		}

		if (chasing) {
			agent.destination = player.transform.position;
		} else {
			//Debug.Log(Vector3.Distance(transform.position, agent.destination));
			if (Vector3.Distance(transform.position, agent.destination) <= 
			    transform.position.y - agent.destination.y) {
				currentWaypoint = Random.Range (0, waypoints.Length);
				agent.destination = waypoints[currentWaypoint].position;
				Logging.Log ("(MazeMonster) Reached destination, new objective: " + waypoints[currentWaypoint].name);
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			RaycastHit hit;
			// check line of sight
			if (Physics.Raycast(this.transform.position,
			                    Vector3.Normalize(-transform.position + player.transform.position),
			                    out hit,
			                    Vector3.Distance(transform.position, player.transform.position))) {
				if (hit.collider.tag == "Player") {
					other.GetComponent<PlayerStatus> ().killPlayer ();
				}
			}
		}
	}
}
