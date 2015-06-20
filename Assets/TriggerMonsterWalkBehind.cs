using UnityEngine;
using System.Collections;

public class TriggerMonsterWalkBehind : MonoBehaviour {

	private bool triggered;

	// Use this for initialization
	void Start () {
		triggered = false;
	}
	
	// Update is called once per frame
	void Update () {


	}

	public void OnTriggerEnter(Collider other){
		if(other.tag =="Player" && !triggered){
			triggered = true;
			GameObject monster = Instantiate(Resources.Load("Monsters/MonsterSpawnBehind")) as GameObject; 
		}
	}
}
