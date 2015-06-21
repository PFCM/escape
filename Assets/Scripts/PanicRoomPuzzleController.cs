using UnityEngine;
using System.Collections;
using Escape.Core;

public class PanicRoomPuzzleController : MonoBehaviour {

	public GameObject [] keyPositions;
	public Light [] lights;

	private bool triggered = false;

	private AudioSource audioSource;
	public AudioSource monsterScreamAudioSource;

	public AudioClip horrorSound;

	private int spawnMonsterTimer;

	public GameObject key;
	private bool keySpawned;
	private bool playerSolvedPuzzle;

	public BaseRoomController livingRoomController;

	// Use this for initialization
	void Start () {
		audioSource = gameObject.GetComponent<AudioSource> ();
		spawnMonsterTimer=-1;
		triggered = false;
	}
	
	// Update is called once per frame
	void Update () {
		spawnMonsterTimer--;

		//if key Something
		//player solved puzzle

		if(spawnMonsterTimer ==0 && key!=null){//!playerSolvedPuzzle){
			GameObject monster = Instantiate(Resources.Load("Monsters/MonsterChase")) as GameObject; 
			monster.transform.position = new Vector3(transform.position.x,transform.position.y,transform.position.z +4);
		}
//		if(keySpawned && key == null){
//			PlayerStatus.addMainDoorKey();
//			GameObject.FindGameObjectWithTag("Player").GetComponent<playerGUIScript>().displayGuiText(PlayerStatus.getMainDoorKeys() + "/" + PlayerStatus.getTotalMainDoorKeys() + " exit keys found");
//		}
	//	if(key ==null){
		//	playerSolvedPuzzle=true;
		//}
	}

	public void OnTriggerEnter(Collider other){
		//all doors close
		//lights turn red
		//sounds play
		if(other.gameObject.tag == "Player" && !triggered){
		triggered = true;
		spawnMonsterTimer = 200;	
		spawnKey();
		for(int i =0;i<lights.Length;i++){
			lights[i].color = Color.red;
		}
		monsterScreamAudioSource.Play();
		audioSource.clip = horrorSound;
		audioSource.Play ();
		}

	}

	private void spawnKey(){
		//chooses a random position and makes a key from it
		int chosenPosition = Random.Range (0,keyPositions.Length-1);
		key = Instantiate(Resources.Load("Keys/Key")) as GameObject; 
		key.transform.position = keyPositions[chosenPosition].transform.position;
		key.GetComponent<Key>().name = "LivingRoomKey";
		key.GetComponent<Key>().mainDoorKey = true;
		key.GetComponent<Key>().controller = livingRoomController;
		keySpawned = true;
	}
	
}
