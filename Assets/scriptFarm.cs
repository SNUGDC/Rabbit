using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class scriptFarm : MonoBehaviour {

	/*-----readonly variables-----*/
	public static readonly uint mMaxEndCount = 7;
	public static readonly uint pixToUnit = 3;

	/*-----public data types-----*/
	public enum State {GAME, DICT};
	
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
	
	/*-----private static variables-----*/
	private static long mMoney;
	private static uint mEndCount;
	private static int mSWidth = Screen.width;
	private static int mSHeight = Screen.height;
	private static GUIStyle mMoneyStyle = new GUIStyle();
	private static GUIStyle mEndStyle = new GUIStyle();
	private static GUIStyle mDictStyle = new GUIStyle();
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
		InvokeRepeating("rabbitCost", 6, 6);
	}
	
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			// select rabbit
			mTargetRabbit = clickedObject("rabbit", delegate(GameObject input){return true;});
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
		// dict button
		if (GUI.Button (new Rect (mSWidth * 0.1f, mSHeight * 0.0f, mSWidth * 0.1f, mSHeight * 0.1f), "Dictionary") && (mCurState == State.GAME)) {
			mCurState = State.DICT;
			Time.timeScale = 0; // stop game time
		}
		// buy button
		if (GUI.Button (new Rect (mSWidth * 0.3f, mSHeight * 0.0f, mSWidth * 0.1f, mSHeight * 0.1f), "Buy") && (mCurState == State.GAME)) {
			if(mMoney >= 200){
				Rabbit.create(null, null);
				mMoney -= 200;
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
		// game over
		if(mEndCount >= mMaxEndCount){
			Time.timeScale = 0;
			GUI.Label(new Rect(mSWidth * 0.1f, mSHeight * 0.1f, mSWidth * 0.8f, mSHeight * 0.8f), "Game Over", mEndStyle);
			if(GUI.Button(new Rect(mSWidth * 0.7f, mSHeight * 0.7f, mSWidth * 0.1f, mSHeight * 0.1f), "return")){
				Time.timeScale = 1; // resume game
				Application.LoadLevel("sceneLevelSelect");
			}
		}
	}

	void rabbitCost(){
		mMoney -= Rabbit.rabbitList.Count * 100;
		if(mMoney < 0){
			mEndCount++;
		}
		else if(0 < mEndCount && mEndCount < mMaxEndCount){
			mEndCount--;
		}
	}
}