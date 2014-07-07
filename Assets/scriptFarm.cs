using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class scriptFarm : MonoBehaviour {

	enum GameState{GAME, DICT, HELP};
	
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
	public static List<Rabbit> rabbitList{
		get{
			return mRabbitList;
		}
	}
	public static Rabbit targetBuffer{
		get{
			return mTargetBuffer;
		}
	}
	
	private static bool mTestMode = false;
	private static bool mShowPopup = false;
	private static int mMoney = 1100;
	private static int mSWidth = Screen.width;
	private static int mSHeight = Screen.height;
	private static List<Carrot> mCarrotList = new List<Carrot>();
	private static List<Rabbit> mRabbitList = new List<Rabbit>();
	private static GUIStyle mDictStyle = new GUIStyle();
	private static GUIStyle mHelpStyle = new GUIStyle();
	private static GUIStyle mPopupStyle = new GUIStyle();
	private static GameState mCurState = GameState.GAME;
	private static Rabbit mTargetRabbit = null;
	private static Rabbit mTargetBuffer = null;
	
	void Start () {
		objRabbit = (GameObject)Resources.Load("prefabRabbit");
		objCarrot = (GameObject)Resources.Load("prefabCarrot");
		Rabbit.initRabbit();
		FarmFunc.init();
		mDictStyle.fontSize = 50;
		mDictStyle.normal.background = new Texture2D(2, 2);
		mHelpStyle.fontSize = 50;
		mHelpStyle.normal.background = new Texture2D(2, 2);
		mPopupStyle.fontSize = 15;
		mPopupStyle.normal.background = new Texture2D(2, 2);
		InvokeRepeating("IncreaseHunger", 0.4f, 0.4f);
	}
	
	void IncreaseHunger(){
		foreach(Rabbit element in mRabbitList){
			++(element.hunger);
		}
	}
	
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			mTargetRabbit = FarmFunc.selectRabbit();
			mShowPopup = (mTargetRabbit != null);
			mTargetBuffer = mTargetRabbit;
		}
		if(Input.GetMouseButtonDown(1)){
			mCarrotList.Add(FarmFunc.createCarrot(Input.mousePosition.x, Input.mousePosition.y));
		}
		if (Input.GetMouseButtonUp (0)) {
			if(mTargetRabbit != null){
				mTargetRabbit.selected = false;
				Rabbit anotherRabbit = FarmFunc.findAnotherRabbit(mTargetRabbit);
				if(Input.mousePosition.x > mSWidth* 0.9f && Input.mousePosition.y < mSHeight * 0.1f){
					//in trash area
					mRabbitList.Remove(mTargetRabbit);
					DestroyImmediate (mTargetRabbit.gameObject);
					mMoney += 200;
				}
				else if(anotherRabbit != null && anotherRabbit.gender != mTargetRabbit.gender
					 && anotherRabbit.grow && mTargetRabbit.grow){
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
			popupText += ("hunger : " + ((mTargetBuffer.hunger != Rabbit.maxHunger + 1) ? mTargetBuffer.hunger.ToString() : "dead") + "\n");
			popupText += ("gender : " + (mTargetBuffer.grow ? mTargetBuffer.gender.ToString() : "???") + "\n");
			for(int i = 0; i < mTargetBuffer.geneList.Count; ++i){
				popupText += mTargetBuffer.geneList[i].name + " : ";
				if(mTargetBuffer.grow){
					for(int j = 0; j < mTargetBuffer.geneList[i].factor.GetLength(0); ++j){
						for(int k = 0; k < mTargetBuffer.geneList[i].factor.GetLength(1); ++k){
							popupText += mTargetBuffer.geneList[i].factor[j, k];
						}
						popupText += ", ";
					}
					popupText = popupText.Remove(popupText.Length - 2, 2);
				}
				else{
					popupText += "???";
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