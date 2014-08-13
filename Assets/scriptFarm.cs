using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class scriptFarm : MonoBehaviour {

	/*-----readonly variables-----*/
	public static readonly uint mMaxEndCount = 7;
	public static readonly uint pixToUnit = 3;

	/*-----public data types-----*/
	public enum State {GAME, DICT, HELP};
	
	/*-----public static variables-----*/
	public static GameObject objRabbit;
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
	public static bool showPopup{
		get{
			return mShowPopup;
		}
		set{
			mShowPopup = value;
		}
	}
	
	/*-----private static variables-----*/
	private static bool mTestMode;
	private static bool mShowPopup;
	private static long mMoney;
	private static uint mEndCount;
	private static int mSWidth = Screen.width;
	private static int mSHeight = Screen.height;
	private static GUIStyle mMoneyStyle = new GUIStyle();
	private static GUIStyle mEndStyle = new GUIStyle();
	private static GUIStyle mDictStyle = new GUIStyle();
	private static GUIStyle mHelpStyle = new GUIStyle();
	private static GUIStyle mPopupStyle = new GUIStyle();
	private static State mCurState = State.GAME;
	private static GameObject mTargetRabbit = null;
	private static List<GameObject> mRoomList = new List<GameObject>();
	
	/*-----public static functions-----*/
	// find gameobject at mouse position with condition
	public static GameObject clickedObject(string tag, System.Func<GameObject, bool> condition){
		Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		RaycastHit2D[] hit = Physics2D.RaycastAll(ray, Vector2.zero);
		GameObject result = null;
		foreach(RaycastHit2D element in hit){
			if(element.collider != null && element.transform.tag == tag && condition(element.collider.gameObject)){
				result = element.collider.gameObject;
				break;
			}
		}
		return result;
	}

	/*-----public member functions-----*/
	void Start () {
		objRabbit = (GameObject)Resources.Load("prefabRabbit");
		mTestMode = false;
		mShowPopup = false;
		mMoney = 10000;
		// class init
		Rabbit.init();
		JsonGene.init();
		// style init
		mEndCount = 0;
		mEndStyle.fontSize = 50;
		mEndStyle.normal.background = new Texture2D(2, 2);
		mDictStyle.fontSize = 50;
		mDictStyle.normal.background = new Texture2D(2, 2);
		mHelpStyle.fontSize = 50;
		mHelpStyle.normal.background = new Texture2D(2, 2);
		mPopupStyle.fontSize = 15;
		mPopupStyle.normal.background = new Texture2D(2, 2);
		InvokeRepeating("rabbitCost", 6, 6);
	}
	
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			// select rabbit
			mTargetRabbit = clickedObject("rabbit", delegate(GameObject input){return true;});
			mShowPopup = (mTargetRabbit != null);
		}
		// move rabbit to LoveRoom
		if(Input.GetMouseButtonDown(1)){
			// select rabbit
			mTargetRabbit = clickedObject("rabbit", delegate(GameObject input){return true;});
			mShowPopup = (mTargetRabbit != null);
			if(mTargetRabbit != null && mTargetRabbit.GetComponent<Rabbit>().isAdult){
				// if rabbit is in LoveRoom, move rabbit to Random place on Farm
				if(mTargetRabbit.GetComponent<Rabbit>().inRoom){
					Vector3 worldLeftBottom = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
					Vector3 worldRightTop = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.9f, Screen.height * 0.9f, 0));
					mTargetRabbit.transform.position = new Vector3(Random.Range (worldLeftBottom.x, worldRightTop.x),
																   Random.Range (worldLeftBottom.y, worldRightTop.y), 0);
					int tempIndex = mRoomList.IndexOf(mTargetRabbit);
					mRoomList.Remove(mTargetRabbit);
					for(int i = tempIndex; i < mRoomList.Count; ++i){
						mRoomList[i].transform.position += new Vector3(0, 30, 0);
					}
					mTargetRabbit.GetComponent<Rabbit>().inRoom = false;
					mTargetRabbit.GetComponent<Draggable>().noDrag = false;
				}
				// if rabbit is not in Love Room, move rabbit to LoveRoom
				else if(mRoomList.Count < 2){
					Vector3 worldLeftBottom = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
					Vector3 worldRightTop = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
					mTargetRabbit.transform.position = new Vector3(worldLeftBottom.x * 0.05f + worldRightTop.x * 0.95f,
																   worldLeftBottom.y * 0.5f + worldRightTop.y * 0.5f - 30 * mRoomList.Count,
																   0);
					mRoomList.Add(mTargetRabbit);
					mTargetRabbit.GetComponent<Rabbit>().inRoom = true;
					mTargetRabbit.GetComponent<Draggable>().noDrag = true;
				}
			}
		}
		if (Input.GetMouseButtonUp (0)) {
			if(mTargetRabbit != null){
				GameObject anotherRabbit = clickedObject("rabbit", delegate(GameObject input){return input.GetComponent<Rabbit>() != mTargetRabbit;});
				// in trash area
				if(Input.mousePosition.x > mSWidth * 0.9f && Input.mousePosition.y < mSHeight * 0.1f && !mTargetRabbit.GetComponent<Rabbit>().inRoom){
					Rabbit.rabbitList.Remove(mTargetRabbit.GetComponent<Rabbit>());
					DestroyImmediate(mTargetRabbit);
					mMoney += 200;
				}
				// found rabbit with different gender & both are grown
				else if(anotherRabbit != null && anotherRabbit.GetComponent<Rabbit>().gender != mTargetRabbit.GetComponent<Rabbit>().gender
					 && anotherRabbit.GetComponent<Rabbit>().isAdult && mTargetRabbit.GetComponent<Rabbit>().isAdult){
					if(mMoney >= 100 || mTestMode){
						if(anotherRabbit.GetComponent<Rabbit>().gender == Rabbit.Gender.MALE){
							Rabbit.create(anotherRabbit, mTargetRabbit);
						}
						else{
							Rabbit.create(mTargetRabbit, anotherRabbit);
						}
						mMoney -= 100;
					}
				}
			}
		}
	}

	void OnGUI(){
		// money - text
		if(mMoney >= 0){
			mMoneyStyle.normal.textColor = Color.white;
		}
		else{
			mMoneyStyle.normal.textColor = Color.red;
		}
		GUI.Label (new Rect (mSWidth * 0.05f, mSHeight * 0.13f, mSWidth * 0.1f, mSHeight * 0.1f), "money : " + mMoney.ToString(), mMoneyStyle);
		// test mode - text
		if(mTestMode){
			GUI.Label (new Rect(mSWidth * 0.05f, mSHeight * 0.16f, mSWidth * 0.1f, mSHeight * 0.1f), "test mode");
		}
		// trash
		GUI.Label (new Rect (mSWidth * 0.9f, mSHeight * 0.9f, mSWidth * 0.1f, mSHeight * 0.1f), "trash", mPopupStyle);
		// return button	
		if (GUI.Button (new Rect (mSWidth * 0.0f, mSHeight * 0.0f, mSWidth * 0.1f, mSHeight * 0.1f), "Return") && (mCurState == State.GAME)) {
			Application.LoadLevel("sceneLevelSelect");
		}
		// dict button
		if (GUI.Button (new Rect (mSWidth * 0.1f, mSHeight * 0.0f, mSWidth * 0.1f, mSHeight * 0.1f), "Dictionary") && (mCurState == State.GAME)) {
			mCurState = State.DICT;
			Time.timeScale = 0; // stop game time
		}
		// help button
		if (GUI.Button (new Rect (mSWidth * 0.2f, mSHeight * 0.0f, mSWidth * 0.1f, mSHeight * 0.1f), "Help") && (mCurState == State.GAME)) {
			mCurState = State.HELP;
			Time.timeScale = 0; // stop game time
		}
		// buy button
		if (GUI.Button (new Rect (mSWidth * 0.3f, mSHeight * 0.0f, mSWidth * 0.1f, mSHeight * 0.1f), "Buy") && (mCurState == State.GAME)) {
			if(mMoney >= 200 || mTestMode){
				Rabbit.create(null, null);
				mMoney -= 200;
			}
		}
		// test mode button
		if(GUI.Button (new Rect (mSWidth * 0.4f, mSHeight * 0.0f, mSWidth * 0.1f, mSHeight * 0.1f), "Test Mode") && (mCurState == State.GAME)){
			mTestMode = !mTestMode;
		}
		if(mRoomList.Count >= 2){
			if(GUI.Button (new Rect (mSWidth * 0.5f, mSHeight * 0.0f, mSWidth * 0.1f, mSHeight * 0.1f), "Reproduce") && (mCurState == State.GAME)){
				if(mRoomList[0].GetComponent<Rabbit>().gender != mRoomList[1].GetComponent<Rabbit>().gender){
					if(mRoomList[0].GetComponent<Rabbit>().gender == Rabbit.Gender.MALE){
						Rabbit.create(mRoomList[0].gameObject, mRoomList[1].gameObject);
					}
					else{
						Rabbit.create(mRoomList[1].gameObject, mRoomList[0].gameObject);
					}
				}
				Vector3 worldLeftBottom = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
				Vector3 worldRightTop = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.9f, Screen.height * 0.9f, 0));
				foreach(GameObject element in mRoomList){
					element.transform.position = new Vector3(Random.Range (worldLeftBottom.x, worldRightTop.x),
																   Random.Range (worldLeftBottom.y, worldRightTop.y), 0);
					element.GetComponent<Rabbit>().inRoom = false;
					element.GetComponent<Draggable>().noDrag = false;
				}
				mRoomList.Clear();
			}
		}
		// in dict mode
		if (mCurState == State.DICT) {
			GUI.Label(new Rect(mSWidth * 0.1f, mSHeight * 0.1f, mSWidth * 0.8f, mSHeight * 0.8f), "Dictionary", mDictStyle);
			if(GUI.Button(new Rect(mSWidth * 0.7f, mSHeight * 0.7f, mSWidth * 0.1f, mSHeight * 0.1f), "return")){
				mCurState = State.GAME;
				Time.timeScale = 1; // resume game
			}
		}
		// in help mode
		else if (mCurState == State.HELP) {
			GUI.Label(new Rect(mSWidth * 0.1f, mSHeight * 0.1f, mSWidth * 0.8f, mSHeight * 0.8f), "Help", mHelpStyle);
			if(GUI.Button(new Rect(mSWidth * 0.7f, mSHeight * 0.7f, mSWidth * 0.1f, mSHeight * 0.1f), "return")){
				mCurState = State.GAME;
				Time.timeScale = 1; // resume game
			}
		}
		// game over
		if(mEndCount >= mMaxEndCount){
			Time.timeScale = 0;
			GUI.Label(new Rect(mSWidth * 0.1f, mSHeight * 0.1f, mSWidth * 0.8f, mSHeight * 0.8f), "Game Over", mEndStyle);
			if(GUI.Button(new Rect(mSWidth * 0.7f, mSHeight * 0.7f, mSWidth * 0.1f, mSHeight * 0.1f), "return")){
				Time.timeScale = 1; // resume game
				Application.LoadLevel("sceneLevelSelect");
			}
		}
		// popup
		mShowPopup &= mTargetRabbit != null;
		if(mShowPopup){
			string popupText = "";
			// basic information
			popupText += ("ID : " + mTargetRabbit.GetComponent<Rabbit>().id + "\n");
			popupText += ("name : (none)\n");
			popupText += ("life : " + mTargetRabbit.GetComponent<Rabbit>().life + "\n");
			popupText += ("gender : " + (mTargetRabbit.GetComponent<Rabbit>().isAdult ? mTargetRabbit.GetComponent<Rabbit>().gender.ToString() : "???") + "\n");
			// add all gene's text in geneList
			for(int i = 0; i < mTargetRabbit.GetComponent<Gene>().list.Count; ++i){
				popupText += mTargetRabbit.GetComponent<Gene>().list[i].name + " : ";
				if(mTargetRabbit.GetComponent<Rabbit>().isAdult){
					for(int j = 0; j < mTargetRabbit.GetComponent<Gene>().list[i].factor.GetLength(0); ++j){
						for(int k = 0; k < mTargetRabbit.GetComponent<Gene>().list[i].factor.GetLength(1); ++k){
							popupText += mTargetRabbit.GetComponent<Gene>().list[i].factor[j, k];
						}
						popupText += ", ";
					}
					// remove last comma
					popupText = popupText.Remove(popupText.Length - 2, 2);
				}
				// don't show genes when rabbit is not grown
				else{
					popupText += "???";
				}
				popupText += "\n";
			}
			GUI.Label (new Rect(mSWidth * 0.75f, mSHeight * 0.0f, mSWidth * 0.25f, mSHeight * 0.5f), popupText, mPopupStyle);
			// popup close button
			if(GUI.Button (new Rect(mSWidth * 0.925f, mSHeight * 0.025f, mSWidth * 0.05f, mSHeight * 0.05f), "close")){
				mShowPopup = false;
			}
		}
	}

	void rabbitCost(){
		mMoney -= Rabbit.rabbitList.Count * 100;
		if(mMoney < 0){
			mEndCount++;
		}
		else if(0 < mEndCount && mEndCount < mMaxEndCount && !mTestMode){
			mEndCount--;
		}
	}
}