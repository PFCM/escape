using UnityEngine;
using System.Collections;

public class MonsterStanding : MonoBehaviour {
	
	public int timer = 100;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		timer--;
		if (timer < 1) {
			Destroy (gameObject);
		}
	}
}
