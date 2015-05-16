using UnityEngine;
using System.Collections;

public class MonsterHallwayChase : MonoBehaviour {

	private Vector3 targetPosition;
	public float speed = 2f;
	
	
	// Use this for initialization
	void Start () {
		targetPosition = GameObject.FindGameObjectWithTag ("Player").transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
		targetPosition = GameObject.FindGameObjectWithTag ("Player").transform.position;
		//targetPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y,gameObject.transform.forward); 
		//targetPosition = new Vector3(gameObject.transform.forward.x,gameObject.transform.forward.y,gameObject.transform.forward.z);

		//transform.position = Vector3.MoveTowards(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position,   speed*Time.deltaTime);
		//transform.position = Vector3.MoveTowards(transform.position, targetPosition,   speed*Time.deltaTime);
		//targetPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z-speed);
	
		transform.position = Vector3.MoveTowards(transform.position, targetPosition,   speed*Time.deltaTime);
	}
	
	public bool speedUp(){
		if (speed < 4) {
			speed = 4;
			return true;
		}
		return false;
	}

	public void playerSolvedPuzzle(){
	
	}
}
