using UnityEngine;
using System.Collections;

//this generates random events in a room when it is spawned on when the player entres it
//one of these would be generated with each room
public class RoomEventGenerator : MonoBehaviour {
	
	//room this event generator is attached to
	public GameObject Room;
	
	//the chosen event
	private string eventChoice = "null";
	
	//the type of event out of entry, spawn or timed
	private string eventType = "null";
	
	//list of events which happen when player enters a room
	private string[] entryEvents = {"sound","flashAcross","monsterSpawnBehind","monsterChase"};
	//when the room spawns
	private string[] spawnEvents = {"sound","monsterStanding","monsterSpawnBehind", "monsterChase"};
	//when a timer runs out
	private string[] timedEvents = {"sound","monsterSpawnBehind","monsterChase"};
	
	public int timer =0;
	
	// Use this for initialization
	void Start () {
		
		chooseEvent();
		
		//increase player event chance 
		GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerStatus> ().addEventChance (10);
	}
	
	// Update is called once per frame
	void Update () {
		//timed events
		if (timer < 1 && eventType == "timed") {
			
			if (eventChoice == "sound") {
				//Play sound
			}
			if (eventChoice == "monsterSpawnBehind") {
				
				//spawn a monster
				GameObject monster = Instantiate (Resources.Load ("Monsters/MonsterSpawnBehind")) as GameObject; 
				
				//set its position to generators position
				monster.transform.position = gameObject.transform.position;
			}
			if(eventChoice == "monsterChase"){
				//spawn a monster
				GameObject monster = Instantiate(Resources.Load("Monsters/MonsterChase")) as GameObject; 
				
				//set its position to generators position
				monster.transform.position = gameObject.transform.position;
			}
			eventChoice = "null";
			eventType = "null";
		} else {
			timer--;
		}
	}
	
	public void OnTriggerEnter(Collider other){
		//when the player enteres the room
		if (other.tag == "Player"){ //&& eventType =="entry") {
			
			if(eventChoice == "sound"){
				//Play sound
			}
			
			if(eventChoice == "monsterChase"){
				
				//spawn a monster
				GameObject monster = Instantiate(Resources.Load("Monsters/MonsterChase")) as GameObject; 
				
				//set its position to generators position
				monster.transform.position = gameObject.transform.position;
			}
			
			if(eventChoice == "flashAcross"){
				//spawn a monster
				GameObject monster = Instantiate(Resources.Load("Monsters/MonsterFlashAcross")) as GameObject; 
				
				//set its position to generators position
				monster.transform.position = gameObject.transform.position;
				print ("event: monster flash across");
			}
			if(eventChoice == "monsterSpawnBehind"){
				//spawn a monster
				GameObject monster = Instantiate(Resources.Load("Monsters/MonsterSpawnBehind")) as GameObject; 
				//monster.transform.position = gameObject.transform.position;
			}
			eventChoice = null;
			eventType = null;
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
		eventChance = 80;
		//roll to generate an event
		if (eventChance > Random.Range (0, 100)) {
			
			int typeChoice = Mathf.RoundToInt (Random.Range (0,3));
			
			//select eventType
			if(typeChoice == 0){
				eventType = "entry";
			}else if(typeChoice ==1){
				eventType = "onSpawn";
			}else{
				eventType = "timed";
			}
			
			//get the size of the list matching the eventType
			if(eventType == "entry"){
				chosenArraySize = entryEvents.Length;
			}else if(eventType == "onSpawn"){
				chosenArraySize = spawnEvents.Length;
			}
			else{
				chosenArraySize = timedEvents.Length;
			}
			
			
			//get index of chosen event, selects from first half
			eventIndex = Mathf.RoundToInt (Random.Range (0, Mathf.RoundToInt(chosenArraySize/2)));
			
			
			//increase eventIndex by scaled amount based on players event chance, making more intense the further the player goes
			//increase by 1 until roll to increase fails
			while((eventChance > Random.Range(0,100)) && (eventIndex<chosenArraySize-1)){
				eventIndex++;
			}
			
			//get the size of the list matching the eventType
			if(eventType == "entry"){
				eventChoice = entryEvents[eventIndex];
			}else if(eventType == "onSpawn"){
				eventChoice = spawnEvents[eventIndex];
			}
			else{
				eventChoice = timedEvents[eventIndex];
			}
			
			
			print (eventIndex);
			print (eventChoice);
			print (eventType);
			
			//call an action based on even type
			if (eventType == "onSpawn") {
				doEvent ();
			} else if (eventType == "timed") {
				
				startTimedEvent ();
			}
		}
		
	}
	
	private void doEvent(){
		if(eventChoice == "monsterStanding"){
			
			//spawn a monster
			GameObject monster = Instantiate(Resources.Load("Monsters/MonsterStanding")) as GameObject; 
			
			//set its position to generators position
			monster.transform.position = gameObject.transform.position;
		}
		if(eventChoice == "monsterSpawnBehind"){
			
			//spawn a monster
			GameObject monster = Instantiate(Resources.Load("Monsters/MonsterSpawnBehind")) as GameObject; 
			
			//set its position to generators position
			monster.transform.position = gameObject.transform.position;
		}
		if(eventChoice == "monsterChase"){
			
			//spawn a monster
			GameObject monster = Instantiate(Resources.Load("Monsters/MonsterChase")) as GameObject; 
			
			//set its position to generators position
			monster.transform.position = gameObject.transform.position;
		}
		
		eventChoice = "null";
		eventType = "null";
	}
	
	private void startTimedEvent(){
		
		//could customize timer for each event here
		timer = 150;
		
	}
}
