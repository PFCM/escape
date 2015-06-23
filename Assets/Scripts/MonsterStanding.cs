using UnityEngine;
using System.Collections;

public class MonsterStanding : MonoBehaviour {
	
	public int timer = 150;

	Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent <Animator> ();
		//anim.SetInteger ("AnimState",0);
		//anim.SetInteger ("AnimState",2);
		anim.SetInteger ("AnimState",9);
		timer = 150;
	}
	
	// Update is called once per frame
	void Update () {
		timer--;
		if (timer < 1) {
			Destroy (gameObject);
		}
	}

	public void spotted(){
		if(timer>20){
		timer = 20;
		}
	}
}
