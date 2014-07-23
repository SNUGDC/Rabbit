using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class scriptFarm : MonoBehaviour {

	public enum GameState {GAME, DICT, HELP};
	
	public static GameObject objRabbit;
	public static GameObject objCarrot;
	public static int sWidth{
		get{
			return mSWidth;
		}
	}
	public static int sHeight{
		get{
			return mSHeight;
		}
	}
	public static List<Carrot> carrotList{
		get{
			return mCarrotList;
		}
	}
	
	private static bool mTestMode = false;
	private static bool mShowPopup = false;
	private static int mMoney = 1100;
	private static int mSWidth = Screen.width;
	private static int mSHeight = Screen.height;
	private static List<Carrot> mCarrotList = new List<Carrot>();
	private static GUIStyle mDictStyle = new GUIStyle();
	private static GUIStyle mHelpStyle = new GUIStyle();
	private static GUIStyle mPopupStyle = new GUIStyle();
	private static GameState mCurState = GameState.GAME;
	private static Rabbit mTargetRabbit = null;
	
	void Start () {
		objRabbit = (GameObject)Resources.Load("prefabRabbit");
		objCarrot = (GameObject)Resources.Load("prefabCarrot");
		//class init
		Rabbit.init();
		FarmFunc.init();
		//style init
		mDictStyle.fontSize = 50;
		mDictStyle.normal.background = new Texture2D(2, 2);
		mHelpStyle.fontSize = 50;
		mHelpStyle.normal.background = new Texture2D(2, 2);
		mPopupStyle.fontSize = 15;
		mPopupStyle.normal.background = new Texture2D(2, 2);
	}
	
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			//select rabbit
			mTargetRabbit = FarmFunc.selectRabbit();
			mShowPopup = (mTargetRabbit != null);
		}
		if(Input.GetMouseButtonDown(1)){
			//create carrot
			if(Input.mousePosition.x <= mSWidth * 0.9f && Input.mousePosition.y <= mSHeight * 0.9f){
				mCarrotList.Add(FarmFunc.createCarrot(Input.mousePosition.x, Input.mousePosition.y));
			}
		}
		if (Input.GetMouseButtonUp (0)) {
			if(mTargetRabbit != null){
				mTargetRabbit.selected = false;
				Rabbit anotherRabbit = FarmFunc.findAnotherRabbit(mTargetRabbit);
				//in trash area
				if(Input.mousePosition.x > mSWidth * 0.9f && Input.mousePosition.y < mSHeight * 0.1f){
					Rabbit.delete(mTargetRabbit);
					mMoney += 200;
				}
				//found rabbit with different gender & both are grown
				else if(anotherRabbit != null && anotherRabbit.gender != mTargetRabbit.gender
					 && anotherRabbit.grow && mTargetRabbit.grow){
					if(mMoney >= 100 || mTestMode){
						if(anotherRabbit.gender == Rabbit.Gender.MALE){
							Rabbit.rabbitList.Add (FarmFunc.createRabbit(anotherRabbit, mTargetRabbit));
						}
						else{
							Rabbit.rabbitList.Add(FarmFunc.createRabbit(mTargetRabbit, anotherRabbit));
						}
						mMoney -= 100;
					}
				}
			}
		}
	}

	void OnGUI(){
		//money - text
		GUI.Label (new Rect (mSWidth * 0.05f, mSHeight * 0.13f, mSWidth * 0.1f, mSHeight * 0.1f), "money : " + mMoney.ToString());
		//test mode - text
		if(mTestMode){
			GUI.Label (new Rect(mSWidth * 0.05f, mSHeight * 0.16f, mSWidth * 0.1f, mSHeight * 0.1f), "test mode");
		}
		//trash
		GUI.Label (new Rect (mSWidth * 0.9f, mSHeight * 0.9f, mSWidth * 0.1f, mSHeight * 0.1f), "trash", mPopupStyle);
		//return button	
		if (GUI.Button (new Rect (mSWidth * 0.0f, mSHeight * 0.0f, mSWidth * 0.1f, mSHeight * 0.1f), "Return") && (mCurState == GameState.GAME)) {
			Application.LoadLevel("sceneMainMenu");
		}
		//dict button
		if (GUI.Button (new Rect (mSWidth * 0.1f, mSHeight * 0.0f, mSWidth * 0.1f, mSHeight * 0.1f), "Dictionary") && (mCurState == GameState.GAME)) {
			mCurState = GameState.DICT;
			Time.timeScale = 0; // stop game time
		}
		//help button
		if (GUI.Button (new Rect (mSWidth * 0.2f, mSHeight * 0.0f, mSWidth * 0.1f, mSHeight * 0.1f), "Help") && (mCurState == GameState.GAME)) {
			mCurState = GameState.HELP;
			Time.timeScale = 0; // stop game time
		}
		//buy button
		if (GUI.Button (new Rect (mSWidth * 0.3f, mSHeight * 0.0f, mSWidth * 0.1f, mSHeight * 0.1f), "Buy") && (mCurState == GameState.GAME)) {
			if(mMoney >= 200 || mTestMode){
				Rabbit.rabbitList.Add(FarmFunc.createRabbit(null, null));
				mMoney -= 200;
			}
		}
		//test mode button
		if(GUI.Button (new Rect (mSWidth * 0.4f, mSHeight * 0.0f, mSWidth * 0.1f, mSHeight * 0.1f), "Test Mode") && (mCurState == GameState.GAME)){
			mTestMode = !mTestMode;
		}
		//in dict mode
		if (mCurState == GameState.DICT) {
			GUI.Label(new Rect(mSWidth * 0.1f, mSHeight * 0.1f, mSWidth * 0.8f, mSHeight * 0.8f), "Dictionary", mDictStyle);
			if(GUI.Button(new Rect(mSWidth * 0.7f, mSHeight * 0.7f, mSWidth * 0.1f, mSHeight * 0.1f), "return")){
				mCurState = GameState.GAME;
				Time.timeScale = 1; // resume game
			}
		}
		//in help mode
		else if (mCurState == GameState.HELP) {
			GUI.Label(new Rect(mSWidth * 0.1f, mSHeight * 0.1f, mSWidth * 0.8f, mSHeight * 0.8f), "Help", mHelpStyle);
			if(GUI.Button(new Rect(mSWidth * 0.7f, mSHeight * 0.7f, mSWidth * 0.1f, mSHeight * 0.1f), "return")){
				mCurState = GameState.GAME;
				Time.timeScale = 1; // resume game
			}
		}
		//popup
		if(mShowPopup){
			string popupText = "";
			//basic information
			popupText += ("ID : " + mTargetRabbit.id + "\n");
			popupText += ("name : (none)\n");
			popupText += ("hunger : " + ((mTargetRabbit.hunger != Rabbit.maxHunger + 1) ? mTargetRabbit.hunger.ToString() : "dead") + "\n");
			popupText += ("gender : " + (mTargetRabbit.grow ? mTargetRabbit.gender.ToString() : "???") + "\n");
			//add all gene's text in geneList
			for(int i = 0; i < mTargetRabbit.geneList.Count; ++i){
				popupText += mTargetRabbit.geneList[i].name + " : ";
				if(mTargetRabbit.grow){
					for(int j = 0; j < mTargetRabbit.geneList[i].factor.GetLength(0); ++j){
						for(int k = 0; k < mTargetRabbit.geneList[i].factor.GetLength(1); ++k){
							popupText += mTargetRabbit.geneList[i].factor[j, k];
						}
						popupText += ", ";
					}
					//remove last comma
					popupText = popupText.Remove(popupText.Length - 2, 2);
				}
				//don't show genes when rabbit is not grown
				else{
					popupText += "???";
				}
				popupText += "\n";
			}
			GUI.Label (new Rect(mSWidth * 0.75f, mSHeight * 0.0f, mSWidth * 0.25f, mSHeight * 0.5f), popupText, mPopupStyle);
			//popup close button
			if(GUI.Button (new Rect(mSWidth * 0.925f, mSHeight * 0.025f, mSWidth * 0.05f, mSHeight * 0.05f), "close")){
				mShowPopup = false;
			}
		}
	}
}