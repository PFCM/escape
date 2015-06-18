﻿using UnityEngine;
using System.Collections;

public class BatterySpawnPositionScript : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		SpawnBattery ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void OnTriggerEnter(Collider other){
		
	}
	
	private void SpawnBattery(){
		if(Random.Range (0,100) >0){
			GameObject battery = Instantiate(Resources.Load("Misc/Battery")) as GameObject; 
			battery.transform.position = transform.position;
		}
	}
}