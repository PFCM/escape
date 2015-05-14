using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class MonsterSpawnBehind : MonoBehaviour {
	
	private int timer = 300;
	private Vector3 targetPosition;
	public int speed = 2;
	
	private bool enraged = false;
	private AudioSource audioSource;
	public AudioClip[] footstepSounds;


	private int footStepSoundTimer = 60;
	// Use this for initialization
	void Start () {
		targetPosition = GameObject.FindGameObjectWithTag ("Player").transform.position;
		targetPosition = targetPosition - (GameObject.FindGameObjectWithTag ("Player").transform.forward * 2);
		//targetPosition = new Vector3 (0,0,0);
		transform.position = targetPosition;
		audioSource = GetComponent<AudioSource>();
		print (targetPosition.x);
		print (targetPosition.z);
	}
	
	// Update is called once per frame
	void Update () {
		//timer--;
		//	if(timer <1){
		//		Destroy (gameObject);
		//	}
		targetPosition = GameObject.FindGameObjectWithTag ("Player").transform.position;
		targetPosition = targetPosition - (GameObject.FindGameObjectWithTag ("Player").transform.forward * 2);
		//transform.position = Vector3.MoveTowards(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position,   speed*Time.deltaTime);
		transform.position = Vector3.MoveTowards(transform.position, targetPosition,   speed*Time.deltaTime);
		
		if (enraged) {
			timer--;
			if(timer<1){
				Destroy (gameObject);
			}
		}
	}

	void FixedUpdate(){

		//float movingSpeed = gameObject.GetComponent<Rigidbody> ().velocity.magnitude; //.velocity.magnitude;
			if(GameObject.FindGameObjectWithTag ("Player").GetComponent<FirstPersonController> ().moving){
			playFootStepSound ();
		}
	}
	
	public bool startChasingPlayer(){
		//this check stops it from trigger multiple times when it shouldn't
		if (enraged == false) {
			enraged = true;
			targetPosition = GameObject.FindGameObjectWithTag ("Player").transform.position;
			speed = 4;
			return true;
		}
		return false;
	}

	private void playFootStepSound(){
		footStepSoundTimer--;
		if (footStepSoundTimer < 1) {
			int n = Random.Range (1, footstepSounds.Length);
			audioSource.clip = footstepSounds [n];
			audioSource.PlayOneShot (audioSource.clip);
			// move picked sound to index 0 so it's not picked next time
			footstepSounds [n] = footstepSounds [0];
			footstepSounds [0] = audioSource.clip;
			//reset sound timer
			footStepSoundTimer = 60;
			if(enraged){
				footStepSoundTimer = 20;
			}
		}
	}
}
