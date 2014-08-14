using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class scriptFarm : MonoBehaviour {

	/*-----readonly variables-----*/
	public static readonly uint mMaxEndCount = 7;
	public static readonly uint pixToUnit = 30;

	/*-----public data types-----*/
	public enum State {GAME, MONEY, SELECT, DICT, MENU, STORE};
	
	/*-----public static variables-----*/
	public static GameObject objRabbit;
	public static GameObject objDummy;
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
	public static Diamond fieldArea{
		get{
			return mFieldArea;
		}
	}
	/*-----private static variables-----*/
	private static long mMoney;
	private static uint mEndCount;
	private static int mSWidth = Screen.width;
	private static int mSHeight = Screen.height;
	private static Diamond mFieldArea;
	private static GameObject mTarget;
	private static GUIStyle mMoneyStyle = new GUIStyle();
	private static GUIStyle mEndStyle = new GUIStyle();
	private static GUIStyle mDictStyle = new GUIStyle();
	private static State mCurState = State.GAME;
	private static Vector2 mScrollPos = new Vector2(0, 0);
	public static Camera mListCamera;
	private static bool mSelling;
	public static List<Rabbit> mRoomList;
	
	/*-----public static functions-----*/
	// find gameobject at mouse position with condition
	public static GameObject clickedObject(Camera cam, string tag, System.Func<GameObject, bool> condition){
		Vector2 ray = cam.ScreenToWorldPoint(Input.mousePosition);
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
		objDummy = (GameObject)Resources.Load("prefabDummy");
		mMoney = 10000;
		// class init
		Rabbit.init();
		JsonGene.init();
		// style init
		mEndCount = 0;
		mSelling = false;
		mRoomList = new List<Rabbit>();
		mEndStyle.fontSize = 50;
		mEndStyle.normal.background = new Texture2D(2, 2);
		mDictStyle.fontSize = 50;
		mDictStyle.normal.background = new Texture2D(2, 2);
		mFieldArea = new Diamond(Camera.main.ScreenToWorldPoint(new Vector2(mSWidth * 0.5f, mSHeight * 0.5f)), mSWidth * 0.3f, mSHeight * 0.2f);
		Rabbit.create(null, null);
		mListCamera = GameObject.Find("List Camera").camera;
		InvokeRepeating("rabbitCost", 6, 6);
	}
	
	void offLight(){
		GameObject.Find("Light").GetComponent<SpriteRenderer>().enabled = false;
	}

	void Update () {
		if(Input.GetMouseButtonDown(0)){
			GameObject selDum;
			switch(mCurState){
				case State.GAME :
					mTarget = clickedObject(Camera.main, "rabbit", delegate(GameObject arg1){return true;});
					break;
				case State.MONEY :
					selDum = clickedObject(mListCamera, "dummy", delegate(GameObject arg1){return true;});
					if(selDum != null && Rabbit.dummyList.IndexOf(selDum) != -1){
						int index = Rabbit.dummyList.IndexOf(selDum);
						DestroyImmediate(Rabbit.rabbitList[index].gameObject);
						DestroyImmediate(Rabbit.dummyList[index]);
						Rabbit.rabbitList.RemoveAt(index);
						Rabbit.dummyList.RemoveAt(index);
						for(int i = index; i < Rabbit.dummyList.Count; ++i){
							Rabbit.dummyList[i].transform.position = new Vector2(Rabbit.dummyList[i].transform.position.x,
																				 Rabbit.dummyList[i].transform.position.y + sHeight * 0.1f);
						}
						mMoney += 200;
					}
					break;
				case State.SELECT :
					selDum = clickedObject(mListCamera, "dummy", delegate(GameObject arg1){return true;});
					if(selDum != null && Rabbit.dummyList.IndexOf(selDum) != -1){
						mTarget = Rabbit.rabbitList[Rabbit.dummyList.IndexOf(selDum)].gameObject;
						foreach(GameObject element in Rabbit.dummyList){
							DestroyImmediate(element);
						}
						Rabbit.dummyList.Clear();
						mListCamera.enabled = false;
						mCurState = State.GAME;
						Time.timeScale = 1;
						GameObject.Find("Light").GetComponent<SpriteRenderer>().enabled = true;
						GameObject.Find("Light").transform.position = new Vector3(mTarget.transform.position.x, mTarget.transform.position.y, 2);
						Invoke("offLight", 1);
					}
					break;
			}
		}
		if(Input.GetMouseButtonUp(0)){
			if(mTarget != null && GameObject.Find("House") == clickedObject(Camera.main, "house", delegate(GameObject arg1){return true;})){
				if(mRoomList.Count <= 2){
					mRoomList.Add(mTarget.GetComponent<Rabbit>());
				}
			}
			else if(mTarget != null && !mFieldArea.Contains(mTarget.transform.position)){
				Rabbit.rabbitList.Remove(mTarget.GetComponent<Rabbit>());
				DestroyImmediate(mTarget);
				mMoney += 200;
			}
		}
	}

	void OnDrawGizmos(){
		Gizmos.DrawLine(mFieldArea.left(), mFieldArea.top());
		Gizmos.DrawLine(mFieldArea.top(), mFieldArea.right());
		Gizmos.DrawLine(mFieldArea.right(), mFieldArea.bottom());
		Gizmos.DrawLine(mFieldArea.bottom(), mFieldArea.left());
	}

	void OnGUI(){
		if(GUI.Button(new Rect(mSWidth * 0.1f, mSHeight * 0.05f, mSWidth * 0.15f, mSHeight * 0.1f), mMoney.ToString() + "G") && (mCurState == State.GAME)){
			mCurState = State.MONEY;
			mListCamera.transform.position = new Vector3(700, 0, -10);
			foreach(Rabbit element in Rabbit.rabbitList){
				Rabbit.createDummy(element);
			}
			for(int i = 0; i < Rabbit.dummyList.Count; ++i){
				Rabbit.dummyList[i].transform.position = new Vector3(700 - mSWidth * 0.3f + mSWidth * 0.05f,
																	 0 + mSHeight * 0.3f - mSHeight * (0.05f + 0.1f * i), 0);
			}
			mListCamera.enabled = true;
			Time.timeScale = 0;
		}
		if(GUI.Button(new Rect(mSWidth * 0.3f, mSHeight * 0.05f, mSWidth * 0.15f, mSHeight * 0.1f), Rabbit.rabbitList.Count.ToString() + "마리") && (mCurState == State.GAME)){
			mCurState = State.SELECT;
			mListCamera.transform.position = new Vector3(700, 0, -10);
			foreach(Rabbit element in Rabbit.rabbitList){
				Rabbit.createDummy(element);
			}
			for(int i = 0; i < Rabbit.dummyList.Count; ++i){
				Rabbit.dummyList[i].transform.position = new Vector3(700 - mSWidth * 0.3f + mSWidth * 0.05f,
																	 0 + mSHeight * 0.3f - mSHeight * (0.05f + 0.1f * i), 0);
			}
			mListCamera.enabled = true;
			Time.timeScale = 0;
		}
		if(GUI.Button(new Rect(mSWidth * 0.5f, mSHeight * 0.05f, mSWidth * 0.15f, mSHeight * 0.1f), "도감") && (mCurState == State.GAME)){
			mCurState = State.DICT;
			Time.timeScale = 0;
		}
		if(GUI.Button(new Rect(mSWidth * 0.7f, mSHeight * 0.05f, mSWidth * 0.15f, mSHeight * 0.1f), "상점") && (mCurState == State.GAME)){
			Rabbit.create(null, null);
			//mCurState = State.STORE;
			//Time.timeScale = 0;
		}
		if(GUI.Button(new Rect(mSWidth * 0.7f, mSHeight * 0.15f, mSWidth * 0.15f, mSHeight * 0.1f), "메뉴") && (mCurState == State.GAME)){
			mCurState = State.MENU;
			Time.timeScale = 0;
		}
		switch(mCurState){
			case State.GAME :
				if(mRoomList.Count >= 2){
					if(GUI.Button(new Rect(mSWidth * 0.9f, mSHeight * 0.6f, mSWidth * 0.05f, mSHeight * 0.05f), "!!!")){
						if(mRoomList[0].gender != mRoomList[1].gender){
							if(mRoomList[0].gender == Rabbit.Gender.MALE){
								Rabbit.create(mRoomList[0].gameObject, mRoomList[1].gameObject);
							}
							else{
								Rabbit.create(mRoomList[1].gameObject, mRoomList[0].gameObject);
							}
						}
						mRoomList[0].gameObject.transform.position = new Vector2(0, 0);
						mRoomList[1].gameObject.transform.position = new Vector2(0, 0);
						mRoomList.Clear();
					}
				}
				break;
			case State.MONEY :
				if(GUI.Button(new Rect(mSWidth * 0.4f, mSHeight * 0.1f, mSWidth * 0.2f, mSHeight * 0.1f), "↑")){
					mListCamera.transform.position = new Vector3(mListCamera.transform.position.x, mListCamera.transform.position.y + mSHeight * 0.1f, -10);
				}
				if(GUI.Button(new Rect(mSWidth * 0.4f, mSHeight * 0.8f, mSWidth * 0.2f, mSHeight * 0.1f), "↓")){
					mListCamera.transform.position = new Vector3(mListCamera.transform.position.x, mListCamera.transform.position.y - mSHeight * 0.1f, -10);
				}
				if(GUI.Button(new Rect(mSWidth * 0.6f, mSHeight * 0.15f, mSWidth * 0.05f, mSHeight * 0.05f), "X")){
					foreach(GameObject element in Rabbit.dummyList){
						DestroyImmediate(element);
					}
					Rabbit.dummyList.Clear();
					mListCamera.enabled = false;
					mCurState = State.GAME;
					Time.timeScale = 1;
				}
				break;
			case State.SELECT :
				if(GUI.Button(new Rect(mSWidth * 0.4f, mSHeight * 0.1f, mSWidth * 0.2f, mSHeight * 0.1f), "↑")){
					mListCamera.transform.position = new Vector3(mListCamera.transform.position.x, mListCamera.transform.position.y + mSHeight * 0.1f, -10);
				}
				if(GUI.Button(new Rect(mSWidth * 0.4f, mSHeight * 0.8f, mSWidth * 0.2f, mSHeight * 0.1f), "↓")){
					mListCamera.transform.position = new Vector3(mListCamera.transform.position.x, mListCamera.transform.position.y - mSHeight * 0.1f, -10);
				}
				if(GUI.Button(new Rect(mSWidth * 0.6f, mSHeight * 0.15f, mSWidth * 0.05f, mSHeight * 0.05f), "X")){
					foreach(GameObject element in Rabbit.dummyList){
						DestroyImmediate(element);
					}
					Rabbit.dummyList.Clear();
					mListCamera.enabled = false;
					mCurState = State.GAME;
					Time.timeScale = 1;
				}
				break;
			case State.DICT :
				break;
			case State.STORE :
				break;
			case State.MENU :
				break;
		}
		/*
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
		*/
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