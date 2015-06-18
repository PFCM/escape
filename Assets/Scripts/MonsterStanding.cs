using UnityEngine;
using System.Collections;

public class MonsterStanding : MonoBehaviour {
	
	public int timer = 200;
	
	// Use this for initialization
	void Start () {
		timer = 150;
	}
	
	// Update is called once per frame
	void Update () {
		timer--;
		if (timer < 1) {
			Destroy (gameObject);
		}
	}

	public void spotted(){
		if(timer>20){
		timer = 20;
		}
	}
}
