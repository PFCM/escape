using UnityEngine;
using System.Collections;
using Escape.Rooms;

//this generates random events in a room when it is spawned on when the player entres it
//one of these would be generated with each room
public class RoomEventGenerator : MonoBehaviour {
	
	//room this event generator is attached to
	public GameObject room;
	
	//the chosen event
	private string eventChoice = "null";

	private string[] events = {"monsterSpawnBehind", "monsterStanding", "flickerLights", "turnLightsRed", "spawnBatteries", "randomSound", "delusion", "ageRoom", "openObjects"}; 

	//lights in the room, used for flickering, turning red, etc. 
	public Light[] lights;

	//lists of openable objects used for spawning batteries on and for randomly opening
	public GameObject[] openableObjects;
	
	// Use this for initialization
	void Start () {
		
		chooseEvent();
		
		//increase player event chance 
		GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerStatus> ().addEventChance (10);
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
						print ("turned light " + i + " off");
					lights[i].intensity=0;
				}
				//if(Random.Range (0,100) > 50){
				//	print ("turned light " + i + " off");
			//		lights[i].intensity=0;
			//	}
			//	else{
			//		print ("turned light " + i + " on");
			//		lights[i].intensity=1;
				}
			}
		}
		}

	
	public void OnTriggerEnter(Collider other){
		//when the player enteres the room
		if (other.tag == "Player"){ //&& eventType =="entry") {

			print (eventChoice + " TRIGGERED EVENT");

			if(eventChoice == "randomSound"){
				GameObject sound = Instantiate(Resources.Load("Events/RandomSoundGenerator")) as GameObject; 
				sound.transform.position = gameObject.transform.position;
				eventChoice = null;
			}


			if(eventChoice == "turnLightsRed"){
				turnLightsRed();
				eventChoice = null;
			}

			//this changes the players controls and makes their screen go red
			if(eventChoice == "delusion"){
				activateDelusionMode();
				eventChoice = null;
			}

	
			//change room wall type
			if(eventChoice == "ageRoom"){
				eventChoice = null;
			}
			if(eventChoice == "openObjects"){
				openObjects();
				eventChoice = null;
			}


			if(eventChoice == "monsterStanding"){
				
				//spawn a monster
				GameObject monster = Instantiate(Resources.Load("Monsters/MonsterSpawnBehind")) as GameObject; 
				
				//set its position to generators position
				monster.transform.position = gameObject.transform.position;
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
		//choose an event and its activation type (timed, player-enter or on-spawn)
		
		//get player event chance
		//if event chance higher than random value then select one of 3 event types
		//choose a random event from the array matching the selected even type 
		float eventChance = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerStatus> ().getEventChance ();
		
		//size of the list of events chosen
		int chosenArraySize = 0;
		
		//index of chosen event
		int eventIndex = 0;
		eventChance = 110; //TODO get rid of this

		//roll to include rare events
		if(eventChance > Random.Range (0, 100)){
			string[] events = {"monsterSpawnBehind", "monsterStanding", "flickerLights", "turnLightsRed", "spawnBatteries", "randomSound", "delusion", "ageRoom"}; 
		}
		else{
			string[] events = {"flickerLights", "turnLightsRed", "spawnBatteries", "randomSound", "ageRoom"}; 
		}

		//roll to choose event
		if (eventChance > Random.Range (0, 100)) {

			eventIndex = Mathf.RoundToInt (Random.Range (0, Mathf.RoundToInt(events.Length)));
			eventChoice = events[eventIndex];

		
			print (eventChoice + "EVENT CHOICE");
		
			if(eventChoice == "spawnBatteries"){
				spawnBatteries();
			}
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
		//GameObject battery = Instantiate(Resources.Load("Misc/Battery")) as GameObject; 
		//battery.transform.position.Set(gameObject.transform.position.x-2,gameObject.transform.position.y,gameObject.transform.position.z-2);
	}

}
