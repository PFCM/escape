using UnityEngine;
using System.Collections;

public class DinningRoomPuzzleControl : MonoBehaviour {

	public Light light;
	public GameObject keySpawnPosition;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(!light.enabled){
			GameObject monster = Instantiate(Resources.Load("Monsters/MonsterStanding")) as GameObject; 
			monster.transform.position = gameObject.transform.position;

			//spawn key
			//spawn creepy sound
			GameObject key = Instantiate(Resources.Load("Keys/mainDoorKey")) as GameObject; 
			key.transform.position =keySpawnPosition.transform.position;


			GameObject sound= Instantiate(Resources.Load("Events/RandomSoundGenerator")) as GameObject; 
			sound.transform.position =keySpawnPosition.transform.position;
			//GameObject.destroy(light);
		}
	}
}
