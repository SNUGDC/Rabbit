using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class scriptFarm : MonoBehaviour {
	public enum State{MAIN, MONEY, MONEY_CONFIRM, COUNT, STORE};

	private static readonly int MONEY_START = 10000;
	private static readonly int COST_RABBIT = 200;
	private static readonly int COST_MAINTENANCE = 200;
	private static readonly int MENU_DISTANCE = 50;
	private static readonly int TEXT_MARGIN = 50;

	public static GameObject objRabbit;
	public static GameObject objDummy;
	public static GameObject objText;

	private int mMoney;
	private State mCurState;
	private Camera mCurCam;
	private GameObject mSelObj;
	private Diamond mFieldArea;

	void Start(){
		objRabbit = Resources.Load<GameObject>("prefabRabbit");
		objDummy = Resources.Load<GameObject>("prefabDummy");
		objText = Resources.Load<GameObject>("prefabText");
		mMoney = MONEY_START;
		mCurCam = Camera.main;
		mCurState = State.MAIN;
		// make field area from experience
		mFieldArea = new Diamond(new Vector2(0, 9), 170, 87);
		JsonGene.init();
		Rabbit.init();
		InvokeRepeating("decMoney", 10, 10);
	}
	void Update(){
		// for inputs
		if(Input.GetMouseButtonDown(0)){
			Vector2 ray = mCurCam.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);
			if(hit.collider != null){
				mSelObj = hit.collider.gameObject;
				switch(mCurState){
					case State.MAIN :
						switch(mSelObj.tag){
							case "Rabbit" :
								mSelObj.GetComponent<Draggable>().select = true;
								break;
						}
						break;
					case State.MONEY :
						break;
					case State.COUNT :
						break;
					case State.STORE :
						break;
				}
			}
			else if(mCurState != State.MONEY_CONFIRM && !(mSelObj != null && mCurState == State.MONEY && mSelObj.tag == "Dummy")){
				mSelObj = null;
			}
		}
		if(Input.GetMouseButtonUp(0)){
			Vector2 ray = mCurCam.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);
			GameObject mUpObj = (hit.collider == null) ? null : hit.collider.gameObject;
			if(mUpObj != null){
				switch(mCurState){
					case State.MAIN :
						if(mSelObj != null && mSelObj.tag == "Rabbit"){
							mSelObj.GetComponent<Draggable>().select = false;
							if(!mFieldArea.Contains(mSelObj.transform.position)){
								Rabbit.remove(mSelObj);
							}
						}
						switch(mUpObj.tag){
							case "GUI" :
								switch(mUpObj.name){
									case "MoneyButton" :
										mCurState = State.MONEY;
										foreach(GameObject element in Rabbit.rabbitList){
											Rabbit.createDummy(element.GetComponent<Rabbit>());
										}
										for(int i = 0; i < Rabbit.dummyList.Count; ++i){
											Rabbit.dummyList[i].transform.position = new Vector2(520, 120 - i * MENU_DISTANCE );
											Rabbit.textList[i].transform.position = new Vector2(520 + TEXT_MARGIN, 130 - i * MENU_DISTANCE);
										}
										Time.timeScale = 0;
										mCurCam = GameObject.Find("List Camera").GetComponent<Camera>();
										mCurCam.gameObject.transform.position = new Vector3(700, 0, -10);
										mCurCam.enabled = true;
										break;
									case "CountButton" :
										mCurState = State.MONEY;
										foreach(GameObject element in Rabbit.rabbitList){
											Rabbit.createDummy(element.GetComponent<Rabbit>());
										}
										for(int i = 0; i < Rabbit.dummyList.Count; ++i){
											Rabbit.dummyList[i].transform.position = new Vector2(1220, 120 - i * MENU_DISTANCE );
											Rabbit.textList[i].transform.position = new Vector2(1220 + TEXT_MARGIN, 130 - i * MENU_DISTANCE);
										}
										mCurState = State.COUNT;
										mCurCam = GameObject.Find("List Camera").GetComponent<Camera>();
										mCurCam.gameObject.transform.position = new Vector3(1400, 0, -10);
										mCurCam.enabled = true;
										Time.timeScale = 0;
										break;
									case "StoreButton" :
										mCurState = State.STORE;
										mCurCam = GameObject.Find("List Camera").GetComponent<Camera>();
										mCurCam.gameObject.transform.position = new Vector3(2100, 0, -10);
										mCurCam.enabled = true;
										Time.timeScale = 0;
										break;
								}
								break;
						}
						break;
					case State.MONEY :
						if(mSelObj != null && mSelObj.tag == "Dummy" && mUpObj.tag == "Dummy"){
							mCurState = State.MONEY_CONFIRM;
							break;
						}
						switch(mUpObj.tag){
							case "GUI" :
								switch(mUpObj.name){
									case "ExitButton" :
										mCurState = State.MAIN;
										mCurCam.enabled = false;
										mCurCam = Camera.main;
										Time.timeScale = 1;
										Rabbit.clearDummy();
										break;
								}
								break;
							case "Dummy" :
								break;
						}
						break;
					case State.COUNT :
						switch(mUpObj.tag){
							case "GUI" :
								switch(mUpObj.name){
									case "ExitButton" :
										mCurState = State.MAIN;
										mCurCam.enabled = false;
										mCurCam = Camera.main;
										Time.timeScale = 1;
										Rabbit.clearDummy();
										break;
								}
								break;
							case "Dummy" :
								mCurState = State.MAIN;
								GameObject.Find("Light").GetComponent<SpriteRenderer>().enabled = true;
								Vector3 newPos = Rabbit.rabbitList[Rabbit.dummyList.IndexOf(mUpObj)].transform.position;
								newPos.z = 0.2f;
								GameObject.Find("Light").transform.position = newPos;
								Invoke("disableLight", 3);
								mCurCam.enabled = false;
								mCurCam = Camera.main;
								Time.timeScale = 1;
								Rabbit.clearDummy();
								break;
						}
						break;
					case State.STORE :
						switch(mUpObj.tag){
							case "GUI" :
								switch(mUpObj.name){
									case "ExitButton" :
										mCurState = State.MAIN;
										mCurCam.enabled = false;
										mCurCam = Camera.main;
										Time.timeScale = 1;
										break;
									case "BuyIcon" :
										Rabbit.create(null, null);
										mMoney -= COST_RABBIT;
										mCurState = State.MAIN;
										mCurCam.enabled = false;
										mCurCam = Camera.main;
										Time.timeScale = 1;
										break;
								}
								break;
						}
						break;
				}
				if(mCurState != State.MONEY_CONFIRM && !(mCurState == State.MONEY_CONFIRM && mUpObj.tag == "Dummy")){
					mSelObj = null;
				}
			}
		}
		// updating texts
		GameObject.Find("MoneyButton").transform.
				   Find("Text").GetComponent<TextMesh>().text = mMoney.ToString() + "G" + "(-"
				   											  + (Rabbit.rabbitList.Count * COST_MAINTENANCE).ToString() + "G)";
		GameObject.Find("CountButton").transform.
				   Find("Text").GetComponent<TextMesh>().text = Rabbit.rabbitList.Count.ToString() + "마리";
	}

	void OnGUI(){
		if(mCurState == State.MONEY_CONFIRM){
			if(GUI.Button(new Rect(0, Screen.height * 0.5f, Screen.width * 0.1f, Screen.height * 0.1f), "SELL!!")){
				mMoney += COST_RABBIT;
				int index = Rabbit.dummyList.IndexOf(mSelObj);
				Destroy(Rabbit.rabbitList[index]);
				Destroy(Rabbit.dummyList[index]);
				Destroy(Rabbit.textList[index]);
				Rabbit.rabbitList.RemoveAt(index);
				Rabbit.dummyList.RemoveAt(index);
				Rabbit.textList.RemoveAt(index);
				for(int i = index; i < Rabbit.dummyList.Count; ++i){
					Vector2 cusPos = Rabbit.dummyList[i].transform.position;
					Rabbit.dummyList[i].transform.position = new Vector2(cusPos.x, cusPos.y + MENU_DISTANCE);
					Rabbit.textList[i].transform.position = new Vector2(cusPos.x + TEXT_MARGIN, cusPos.y + MENU_DISTANCE);
				}
				mCurState = State.MONEY;
			}
			if(GUI.Button(new Rect(0, Screen.height * 0.6f, Screen.width * 0.1f, Screen.height * 0.1f), "NOOO!!")){
				mCurState = State.MONEY;
			}
		}
	}

	void decMoney(){
		mMoney -= Rabbit.rabbitList.Count * COST_MAINTENANCE;
	}

	void disableLight(){
		GameObject.Find("Light").GetComponent<SpriteRenderer>().enabled = false;
	}

// for field area test
/*
	void OnDrawGizmos(){
		Gizmos.DrawLine(mFieldArea.top(), mFieldArea.right());
		Gizmos.DrawLine(mFieldArea.right(), mFieldArea.bottom());
		Gizmos.DrawLine(mFieldArea.bottom(), mFieldArea.left());
		Gizmos.DrawLine(mFieldArea.left(), mFieldArea.top());
		Gizmos.DrawLine(mFieldArea.left(), mFieldArea.right());
		Gizmos.DrawLine(mFieldArea.top(), mFieldArea.bottom());
	}
*/
}