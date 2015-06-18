﻿using UnityEngine;
using System.Collections;

public class MonsterChase : MonoBehaviour {
	
	public int timer = 500;
	private Vector3 targetPosition;
	public int speed = 4;
	
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
		targetPosition = GameObject.FindGameObjectWithTag ("Player").transform.position;
		//transform.position = Vector3.MoveTowards(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position,   speed*Time.deltaTime);
		transform.position = Vector3.MoveTowards(transform.position, targetPosition,   speed*Time.deltaTime);
		
		timer--;
		if(timer<1){
			//Destroy (gameObject);
		}
		
	}

	public bool speedUp(){
		if (speed < 4) {
			speed = 4;
			return true;
		}
		return false;
	}
}
