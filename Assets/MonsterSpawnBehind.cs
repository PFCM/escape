using UnityEngine;
using System.Collections;

public class MonsterSpawnBehind : MonoBehaviour {
	
	private int timer = 300;
	private Vector3 targetPosition;
	public int speed = 2;
	
	private bool enraged = false;
	
	// Use this for initialization
	void Start () {
		targetPosition = GameObject.FindGameObjectWithTag ("Player").transform.position;
		targetPosition = targetPosition - (GameObject.FindGameObjectWithTag ("Player").transform.forward * 2);
		//targetPosition = new Vector3 (0,0,0);
		transform.position = targetPosition;
		
		print (targetPosition.x);
		print (targetPosition.z);
	}
	
	// Update is called once per frame
	void Update () {
		//timer--;
		//	if(timer <1){
		//		Destroy (gameObject);
		//	}
		targetPosition = GameObject.FindGameObjectWithTag ("Player").transform.position;
		targetPosition = targetPosition - (GameObject.FindGameObjectWithTag ("Player").transform.forward * 2);
		//transform.position = Vector3.MoveTowards(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position,   speed*Time.deltaTime);
		transform.position = Vector3.MoveTowards(transform.position, targetPosition,   speed*Time.deltaTime);
		
		if (enraged) {
			timer--;
			if(timer<1){
				Destroy (gameObject);
			}
		}
	}
	
	public void startChasingPlayer(){
		enraged = true;
		targetPosition = GameObject.FindGameObjectWithTag ("Player").transform.position;
		speed = 4;
	}
}
