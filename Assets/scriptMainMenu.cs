using UnityEngine;
using System.Collections;

public class scriptMainMenu : MonoBehaviour {
	public int sWidth;
	public int sHeight;

	private Texture2D txtrMainLogo;
	// Use this for initialization
	void Start () {
		txtrMainLogo = (Texture2D)Resources.Load ("txtrMainLogo");
		sWidth = Screen.width;
		sHeight = Screen.height;
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnGUI(){
		if(sWidth != Screen.width || sHeight != Screen.height){
		}
		sWidth = Screen.width;
		sHeight = Screen.height;
		GUIStyle style = new GUIStyle ();
		style.alignment = TextAnchor.MiddleCenter;

		GUI.Label (new Rect ( sWidth * 0.15f, sHeight * 0.2f, sWidth * 0.7f, sHeight * 0.3f),
		           txtrMainLogo, style);
		if (GUI.Button (new Rect(sWidth * 0.3f, sHeight * 0.6f ,sWidth * 0.4f, sHeight * 0.05f),
		                "startButton")) {
			Application.LoadLevel("sceneLevelSelect");
		}
		if (GUI.Button (new Rect (sWidth * 0.3f, sHeight * 0.7f, sWidth * 0.4f, sHeight * 0.05f),
		                "exitButton")) {
			Application.Quit();
		}
	}
}
