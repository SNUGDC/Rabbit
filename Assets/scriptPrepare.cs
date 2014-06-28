using UnityEngine;
using System.Collections;

public class scriptPrepare : MonoBehaviour {
	public int sWidth;
	public int sHeight;

	//private Texture2D txtrPrepareButton;
	// Use this for initialization
	void Start () {
		//txtrPrepareButton = (Texture2D)Resources.Load ("txtrPrepareButton");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI(){
		sWidth = Screen.width;
		sHeight = Screen.height;
		GUIStyle style = new GUIStyle ();
		style.alignment = TextAnchor.MiddleCenter;

		if (GUI.Button (new Rect(sWidth * 0.3f, sHeight * 0.3f ,sWidth * 0.4f, sHeight * 0.2f),
		                "Prepared")) {
			Application.LoadLevel("sceneFarm");
		}
		if (GUI.Button (new Rect (sWidth * 0.3f, sHeight * 0.6f, sWidth * 0.4f, sHeight * 0.2f),
		                "Return to Main")) {
			Application.LoadLevel("sceneMainMenu");
		}
	}
}
