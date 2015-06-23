using UnityEngine;
using System.Collections;
using Escape.Rooms;

//this generates random events in a room when it is spawned on when the player entres it
//one of these would be generated with each room
public class RoomEventGenerator : MonoBehaviour {
	
	//the chosen event
	private string eventChoice = "null";

	private string[] events = {"monsterSpawnBehind", "monsterStanding", "flickerLights", "turnLightsRed", "spawnBatteries", "randomSound"}; 

	//lights in the room, used for flickering, turning red, etc. 
	public Light[] lights;

	//lists of openable objects used for spawning batteries on and for randomly opening
	public GameObject[] openableObjects;
	
	// Use this for initialization
	void Start () {
		
	//	chooseEvent();
		
		//increase player event chance 

	}

	void  OnEnable()
	{
		//GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerStatus> ().addEventChance (5);
		chooseEvent ();
	}

	// Update is called once per frame
	void Update () {

		//continuosly flicker lights
		if(eventChoice == "flickerLights"){

			//for each light
			//on or off = random
			print ("" + lights.Length);
			for(int i =0;i<lights.Length;i++){
				print ("FLICKERING LIGHT");
				//lights[i].intensity=0;
				if(lights[i].intensity <1){
					lights[i].intensity =0.05f + lights[i].intensity*1.2f;
				}else{
				if(Random.Range (0,100) > 90){
					lights[i].intensity=0;
				}
				
				}
			}
		}
		}

	
	public void OnTriggerEnter(Collider other){
		//chooseEvent ();
		//when the player enteres the room
		if (other.tag == "Player"){ //&& eventType =="entry") {

			print (eventChoice + " TRIGGERED EVENT");

			if(eventChoice == "randomSound"){
				GameObject sound = Instantiate(Resources.Load("Events/RandomSoundGenerator")) as GameObject; 
				sound.transform.position = gameObject.transform.position;
				eventChoice = null;
			}


			if(eventChoice == "turnLightsRed"){
				GameObject sound = Instantiate(Resources.Load("Events/RandomSoundGenerator")) as GameObject; 
				sound.transform.position = gameObject.transform.position;
				turnLightsRed();
				eventChoice = null;
			}

			//this changes the players controls and makes their screen go red
			if(eventChoice == "delusion"){
				GameObject sound = Instantiate(Resources.Load("Events/RandomSoundGenerator")) as GameObject; 
				sound.transform.position = gameObject.transform.position;
				activateDelusionMode();
				eventChoice = null;
			}

	
			//change room wall type
			if(eventChoice == "ageRoom"){
				eventChoice = null;
			}


			if(eventChoice == "monsterStanding"){
				GameObject sound = Instantiate(Resources.Load("Events/RandomSoundGenerator")) as GameObject; 
				sound.transform.position = gameObject.transform.position;
				//spawn a monster
				GameObject monster = Instantiate(Resources.Load("Monsters/MonsterStanding")) as GameObject; 

				Vector3 targetPosition = GameObject.FindGameObjectWithTag ("Player").transform.position;
				targetPosition = targetPosition + (GameObject.FindGameObjectWithTag ("Player").transform.forward * 1.5f);
				monster.transform.position = targetPosition;
				eventChoice = null;
			}

			if(eventChoice == "monsterSpawnBehind"){

				print (eventChoice + " spawned monster");
				//spawn a monster
				GameObject monster = Instantiate(Resources.Load("Monsters/MonsterSpawnBehind")) as GameObject; 
				//monster.transform.position = gameObject.transform.position;
				eventChoice = null;
			}

		}
		
	}
	

	public void chooseEvent(){
		eventChoice = "null";
		//choose an event and its activation type (timed, player-enter or on-spawn)
		
		//get player event chance
		//if event chance higher than random value then select one of 3 event types
		//choose a random event from the array matching the selected even type 
		float eventChance = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerStatus> ().getEventChance ();
		
		//size of the list of events chosen
		int chosenArraySize = 0;
		
		//index of chosen event
		int eventIndex = 0;

			string[] events = {"monsterSpawnBehind", "monsterStanding", "flickerLights", "turnLightsRed", "randomSound"}; 
		

		//roll to choose event
		if (eventChance > Random.Range (0, 100)) {

			eventIndex = Mathf.RoundToInt (Random.Range (0, Mathf.RoundToInt(events.Length)));
			eventChoice = events[eventIndex];
		

		}else{
			eventChoice = "null";
		}
		
	}
	//on spawn events
	private void doEvent(){
	}


	//turns all of the lights in the room red
	private void turnLightsRed(){
		for(int i = 0; i<lights.Length;i++){
			lights[i].color = Color.red;
		}
	}

	private void openObjects(){
		for(int i = 0; i<openableObjects.Length;i++){
			//openableObjects[i].open();
		}
	}

	private void activateDelusionMode(){
		//lights go red, player movement changes
		//have to reset when player enters new room
	}

	private void spawnBatteries(){
		GameObject battery = Instantiate(Resources.Load("Misc/Battery")) as GameObject; 
		Vector3 targetPosition = GameObject.FindGameObjectWithTag ("Player").transform.position;
		targetPosition = targetPosition + (GameObject.FindGameObjectWithTag ("Player").transform.forward * 2);
		//targetPosition = new Vector3 (0,0,0);
		battery.transform.position = targetPosition;
		eventChoice = "null";
		//battery.transform.position.Set(gameObject.transform.position.x-2,gameObject.transform.position.y,gameObject.transform.position.z-2);
	}

}
