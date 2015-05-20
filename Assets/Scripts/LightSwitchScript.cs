using UnityEngine;
using System.Collections;

public class LightSwitchScript : MonoBehaviour {

	public bool on = false;
	public Light light;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void toggleOn(){
		//make sound
		on = !on;
		light.enabled = on;
		print ("player hit switch");
	}

}
