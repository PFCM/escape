using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using Escape.Core;
using Escape.Util;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {

	public Button exitButton;
	public Button startButton;

	// Use this for initialization
	void Start () {
		exitButton.onClick.AddListener(() => { Application.Quit (); });
		startButton.onClick.AddListener(() => { Application.LoadLevel ("EntranceHall"); });
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
