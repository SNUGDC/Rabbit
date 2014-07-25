using UnityEngine;
using System.Collections;

public class scriptLevelSelect : MonoBehaviour {

	private static int mSWidth = Screen.width;
	private static int mSHeight = Screen.height;

	// Use this for initialization
	void Start(){
	}
	
	// Update is called once per frame
	void Update(){
	
	}

	void OnGUI(){
		if(GUI.Button (new Rect (mSWidth * 0.075f, mSHeight * 0.1f, mSWidth * 0.1f, mSHeight * 0.1f), "1")) {
			Application.LoadLevel("sceneFarm");
		}
		if(GUI.Button (new Rect (mSWidth * 0.225f, mSHeight * 0.1f, mSWidth * 0.1f, mSHeight * 0.1f), "2")) {
		}
		if(GUI.Button (new Rect (mSWidth * 0.375f, mSHeight * 0.1f, mSWidth * 0.1f, mSHeight * 0.1f), "3")) {
		}
		if(GUI.Button (new Rect (mSWidth * 0.525f, mSHeight * 0.1f, mSWidth * 0.1f, mSHeight * 0.1f), "4")) {
		}
		if(GUI.Button (new Rect (mSWidth * 0.675f, mSHeight * 0.1f, mSWidth * 0.1f, mSHeight * 0.1f), "5")) {
		}
		if(GUI.Button (new Rect (mSWidth * 0.825f, mSHeight * 0.1f, mSWidth * 0.1f, mSHeight * 0.1f), "6")) {
		}
		if(GUI.Button (new Rect (mSWidth * 0.075f, mSHeight * 0.3f, mSWidth * 0.1f, mSHeight * 0.1f), "7")) {
		}
		if(GUI.Button (new Rect (mSWidth * 0.225f, mSHeight * 0.3f, mSWidth * 0.1f, mSHeight * 0.1f), "8")) {
		}
		if(GUI.Button (new Rect (mSWidth * 0.375f, mSHeight * 0.3f, mSWidth * 0.1f, mSHeight * 0.1f), "9")) {
		}
		if(GUI.Button (new Rect (mSWidth * 0.525f, mSHeight * 0.3f, mSWidth * 0.1f, mSHeight * 0.1f), "10")) {
		}
		if(GUI.Button (new Rect (mSWidth * 0.675f, mSHeight * 0.3f, mSWidth * 0.1f, mSHeight * 0.1f), "11")) {
		}
		if(GUI.Button (new Rect (mSWidth * 0.825f, mSHeight * 0.3f, mSWidth * 0.1f, mSHeight * 0.1f), "12")) {
		}
		if(GUI.Button (new Rect (mSWidth * 0.075f, mSHeight * 0.5f, mSWidth * 0.1f, mSHeight * 0.1f), "13")) {
		}
		if(GUI.Button (new Rect (mSWidth * 0.225f, mSHeight * 0.5f, mSWidth * 0.1f, mSHeight * 0.1f), "14")) {
		}
		if(GUI.Button (new Rect (mSWidth * 0.375f, mSHeight * 0.5f, mSWidth * 0.1f, mSHeight * 0.1f), "15")) {
		}
		if(GUI.Button (new Rect (mSWidth * 0.525f, mSHeight * 0.5f, mSWidth * 0.1f, mSHeight * 0.1f), "16")) {
		}
		if(GUI.Button (new Rect (mSWidth * 0.675f, mSHeight * 0.5f, mSWidth * 0.1f, mSHeight * 0.1f), "17")) {
		}
		if(GUI.Button (new Rect (mSWidth * 0.825f, mSHeight * 0.5f, mSWidth * 0.1f, mSHeight * 0.1f), "18")) {
		}
		if(GUI.Button (new Rect (mSWidth * 0.075f, mSHeight * 0.7f, mSWidth * 0.1f, mSHeight * 0.1f), "19")) {
		}
		if(GUI.Button (new Rect (mSWidth * 0.225f, mSHeight * 0.7f, mSWidth * 0.1f, mSHeight * 0.1f), "20")) {
		}
		if(GUI.Button (new Rect (mSWidth * 0.375f, mSHeight * 0.7f, mSWidth * 0.1f, mSHeight * 0.1f), "21")) {
		}
		if(GUI.Button (new Rect (mSWidth * 0.525f, mSHeight * 0.7f, mSWidth * 0.1f, mSHeight * 0.1f), "22")) {
		}
		if(GUI.Button (new Rect (mSWidth * 0.675f, mSHeight * 0.7f, mSWidth * 0.1f, mSHeight * 0.1f), "23")) {
		}
		if(GUI.Button (new Rect (mSWidth * 0.825f, mSHeight * 0.7f, mSWidth * 0.1f, mSHeight * 0.1f), "24")) {
		}
		if(GUI.Button (new Rect (mSWidth * 0.4f, mSHeight * 0.85f, mSWidth * 0.2f, mSHeight * 0.05f), "Back")) {
			Application.LoadLevel("sceneMainMenu");
		}
	}
}
