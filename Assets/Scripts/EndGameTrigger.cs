using UnityEngine;
using System.Collections;

public class EndGameTrigger : MonoBehaviour {
	
	private int backToMenuTimer  = -1;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		backToMenuTimer--;
		
		if(backToMenuTimer == 300){
			Time.timeScale = 0;
			GameObject.FindGameObjectWithTag ("Player").GetComponent<playerGUIScript>().displayEndCredits();
		}
		
		if(backToMenuTimer == 0){
			Application.LoadLevel ("MainMenu_Redo");
		}
		
	}
	
	public void OnTriggerEnter(Collider other){
		if(other.tag =="Player"){
			backToMenuTimer = 500;
			//credits.active = true
			//audioSource.clip = screamSound;
			//audioSource.Play();
			//other.GetComponent<PlayerStatus>().killPlayer();
		}
	}
}