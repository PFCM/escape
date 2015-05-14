using UnityEngine;
using System.Collections;

public class MonsterFlashAcross : MonoBehaviour {
	
	private int timer = 300;
	private Vector3 targetPosition;
	public int speed = 5;
	
	
	// Use this for initialization
	void Start () {
		targetPosition = GameObject.FindGameObjectWithTag ("Player").transform.position;
		targetPosition = new Vector3 (targetPosition.x + Random.Range (-100, 100),targetPosition.y,  targetPosition.z+ Random.Range (-100, 100));
		//targetPosition = new Vector3 (0,0,0);
		
		print (targetPosition.x);
		print (targetPosition.z);
	}
	
	// Update is called once per frame
	void Update () {
		timer--;
		if(timer <1){
			Destroy (gameObject);
		}
		
		//transform.position = Vector3.MoveTowards(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position,   speed*Time.deltaTime);
		transform.position = Vector3.MoveTowards(transform.position, targetPosition,   speed*Time.deltaTime);
	}
}
