using UnityEngine;
using System.Collections;

public class playerGUIScript : MonoBehaviour {
	
	public Color guiCol = new Color (255, 255, 255, 0);
	private string guiDisplayedText = "";
	private int guiFadeTimer = 0; //how long the fade in/fade out goes for
	private GUIStyle startStyle = new GUIStyle ();
	public UnityEngine.UI.Text guiText; //displays the text
	public UnityEngine.UI.Text endCreditText;
	
	// Use this for initialization
	void Start () {
		endCreditText.color = guiCol;
		guiText.color = guiCol; //new Color(255,255,255,0);
	}
	
	// Update is called once per frame
	void Update () {
		fadeGuiText ();
	}
	
	//fades and redisplays text based on timer value
	private void fadeGuiText ()
	{
		guiCol = guiText.color;
		if (guiFadeTimer > 350) {
			//fade in
			guiCol.a = guiCol.a + 0.005f;
		} else {
			//fade out
			guiCol.a = guiCol.a - 0.005f;
		}
		guiText.color = guiCol;
		guiFadeTimer --;
	}
	
	//displays some new text for a time until it fades
	public void displayGuiText(string text){
		guiText.text = text;
		guiFadeTimer = 600;
		//reset opacity
		guiCol = guiText.color;
		guiCol.a = 0;
		guiText.color = guiCol;
	}
	
	public void displayEndCredits(){
		endCreditText.color = new Color (255, 255, 255, 1);
	}
}