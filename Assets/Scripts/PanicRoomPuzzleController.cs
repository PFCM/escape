using UnityEngine;
using System.Collections;

public class PanicRoomPuzzleController : MonoBehaviour {

	public GameObject [] keyPositions;
	public Light [] lights;

	private AudioSource audioSource;

	public AudioClip horrorSound;

	private int spawnMonsterTimer;

	// Use this for initialization
	void Start () {
	

	}
	
	// Update is called once per frame
	void Update () {
		spawnMonsterTimer--;

		if(spawnMonsterTimer <0){
			GameObject monster = Instantiate(Resources.Load("Monsters/MonsterChase")) as GameObject; 
			monster.transform.position = new Vector3(transform.position.x,transform.position.y,transform.position.z +4);
		}
	}

	public void OnTriggerEnter(Collider other){
		//all doors close
		//lights turn red
		//sounds play
		spawnMonsterTimer = 3000;	
		spawnKey();
		for(int i =0;i<lights.Length;i++){
			lights[i].color = Color.red;
		}

		audioSource.clip = horrorSound;
		audioSource.Play ();

	}

	private void spawnKey(){
		//chooses a random position and makes a key from it
		int chosenPosition = Random.Range (0,keyPositions.Length-1);
		GameObject key = Instantiate(Resources.Load("")) as GameObject; 
		key.transform.position = keyPositions[chosenPosition].transform.position;
	}
	
}
