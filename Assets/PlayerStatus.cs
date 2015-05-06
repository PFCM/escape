using UnityEngine;
using System.Collections;

//keys, event chance management
public class PlayerStatus : MonoBehaviour {
	
	private int eventChance = 0;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public int getEventChance(){
		return eventChance;
	}
	
	public void addEventChance(int add){
		eventChance = eventChance + add;
	}
}
