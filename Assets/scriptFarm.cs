using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class scriptFarm : MonoBehaviour {

	enum GameState{GAME, DICT, HELP};
	
	public static GameObject objRabbit = (GameObject)Resources.Load("prefabRabbit");
	public static GameObject objCarrot = (GameObject)Resources.Load("prefabCarrot");
	
	private bool mTestMode = false;
	private bool mShowPopup = false;
	private int mMoney = 1100;
	private int mSWidth = Screen.width;
	private int mSHeight = Screen.height;
	private List<Rabbit> mRabbitList = new List<Rabbit>();
	private GUIStyle mDictStyle = new GUIStyle();
	private GUIStyle mHelpStyle = new GUIStyle();
	private GUIStyle mPopupStyle = new GUIStyle();
	private GameState mCurState = GameState.GAME;
	private Rabbit mTargetRabbit = null;
	private Rabbit mTargetBuffer = null;
	
	void Start () {
		mDictStyle.fontSize = 50;
		mDictStyle.normal.background = new Texture2D(2, 2);
		mHelpStyle.fontSize = 50;
		mHelpStyle.normal.background = new Texture2D(2, 2);
		mPopupStyle.fontSize = 15;
		mPopupStyle.normal.background = new Texture2D(2, 2);
	}
	
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			mTargetRabbit = FarmFunc.selectRabbit();
			mShowPopup = (mTargetRabbit != null);
			mTargetBuffer = mTargetRabbit;
		}
		if(Input.GetMouseButtonDown(1)){
			FarmFunc.createCarrot(Input.mousePosition.x, Input.mousePosition.y);
			print("right click");
		}
		if (Input.GetMouseButtonUp (0)) {
			if(mTargetRabbit != null){
				mTargetRabbit.selected = false;
				Rabbit anotherRabbit = FarmFunc.findAnotherRabbit(mTargetRabbit);
				if(Input.mousePosition.x > mSWidth* 0.9f && Input.mousePosition.y < mSHeight * 0.1f){
					//in trash area
					DestroyImmediate (mTargetRabbit.gameObject);
					mMoney += 200;
				}
				else if(anotherRabbit != null && anotherRabbit.gender != mTargetRabbit.gender){
					//found rabbit with different gender
					if(mMoney >= 100 || mTestMode){
						if(anotherRabbit.gender == Rabbit.Gender.MALE){
							mRabbitList.Add (FarmFunc.createRabbit(anotherRabbit, mTargetRabbit));
						}
						else{
							mRabbitList.Add(FarmFunc.createRabbit(mTargetRabbit, anotherRabbit));
						}
						mMoney -= 100;
					}
				}
				mTargetRabbit = null;
			}
		}
	}
	void OnGUI(){
		GUI.Label (new Rect (mSWidth * 0.05f, mSHeight * 0.13f, mSWidth * 0.1f, mSHeight * 0.1f), "money : " + mMoney.ToString());
		//money - text
		if(mTestMode){
			GUI.Label (new Rect(mSWidth * 0.05f, mSHeight * 0.16f, mSWidth * 0.1f, mSHeight * 0.1f), "test mode");
			//test mode - text
		}
		GUI.Label (new Rect (mSWidth * 0.9f, mSHeight * 0.9f, mSWidth * 0.1f, mSHeight * 0.1f), "trash", mPopupStyle);
		//trash

		if (GUI.Button (new Rect (mSWidth * 0.0f, mSHeight * 0.0f, mSWidth * 0.1f, mSHeight * 0.1f), "Return") && (mCurState == GameState.GAME)) {
			//return button	
			Application.LoadLevel("sceneMainMenu");
		}
		if (GUI.Button (new Rect (mSWidth * 0.1f, mSHeight * 0.0f, mSWidth * 0.1f, mSHeight * 0.1f), "Dictionary") && (mCurState == GameState.GAME)) {
			//dict button
			mCurState = GameState.DICT;
			Time.timeScale = 0; // stop game time
		}
		if (GUI.Button (new Rect (mSWidth * 0.2f, mSHeight * 0.0f, mSWidth * 0.1f, mSHeight * 0.1f), "Help") && (mCurState == GameState.GAME)) {
			//help button
			mCurState = GameState.HELP;
			Time.timeScale = 0; // stop game time
		}
		if (GUI.Button (new Rect (mSWidth * 0.3f, mSHeight * 0.0f, mSWidth * 0.1f, mSHeight * 0.1f), "Buy") && (mCurState == GameState.GAME)) {
			//buy button
			if(mMoney >= 200 || mTestMode){
				mRabbitList.Add(FarmFunc.createRabbit(null, null));
				mMoney -= 200;
			}
		}
		if(GUI.Button (new Rect (mSWidth * 0.4f, mSHeight * 0.0f, mSWidth * 0.1f, mSHeight * 0.1f), "Test Mode") && (mCurState == GameState.GAME)){
			//test mode button
			mTestMode = !mTestMode;
		}
		if (mCurState == GameState.DICT) {
			//in dict mode
			GUI.Label(new Rect(mSWidth * 0.1f, mSHeight * 0.1f, mSWidth * 0.8f, mSHeight * 0.8f), "Dictionary", mDictStyle);
			if(GUI.Button(new Rect(mSWidth * 0.7f, mSHeight * 0.7f, mSWidth * 0.1f, mSHeight * 0.1f), "return")){
				mCurState = GameState.GAME;
				Time.timeScale = 1; // resume game
			}
		}
		else if (mCurState == GameState.HELP) {
			//in help mode
			GUI.Label(new Rect(mSWidth * 0.1f, mSHeight * 0.1f, mSWidth * 0.8f, mSHeight * 0.8f), "Help", mHelpStyle);
			if(GUI.Button(new Rect(mSWidth * 0.7f, mSHeight * 0.7f, mSWidth * 0.1f, mSHeight * 0.1f), "return")){
				mCurState = GameState.GAME;
				Time.timeScale = 1; // resume game
			}
		}
		if(mShowPopup){
			//popup
			string popupText = "";
			popupText += ("ID : " + mTargetBuffer.rabbitId + "\n");
			popupText += ("name : (none)\n");
			popupText += ("gender : " + mTargetBuffer.gender + "\n");
			for(int i = 0; i < mTargetBuffer.geneList.Count; ++i){
				popupText += mTargetBuffer.geneList[i].name + " : ";
				if(mTargetBuffer.geneList[i].type[0] == Gene.Type.DOMINANT){
					popupText += "X";
				}
				else{
					popupText += "x";
				}
				if(mTargetBuffer.geneList[i].type[1] == Gene.Type.DOMINANT){
					popupText += "X";
				}
				else{
					popupText += "x";
				}
				popupText += "\n";
			}
			GUI.Label (new Rect(mSWidth * 0.75f, mSHeight * 0.0f, mSWidth * 0.25f, mSHeight * 0.5f), popupText, mPopupStyle);
			if(GUI.Button (new Rect(mSWidth * 0.925f, mSHeight * 0.025f, mSWidth * 0.05f, mSHeight * 0.05f), "close")){
				mShowPopup = false;
			}
		}
	}
}