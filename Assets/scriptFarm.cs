using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class scriptFarm : MonoBehaviour {
	public enum State{MAIN, MONEY, COUNT, STORE};

	private static readonly int MONEY_START = 10000;
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
			else{
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
						if(mSelObj.tag == "Rabbit"){
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
										mCurState = State.MAIN;
										mCurCam.enabled = false;
										mCurCam = Camera.main;
										Time.timeScale = 1;
										break;
								}
								break;
							case "Dummy" :
								break;
						}
						break;
				}
			}
			mSelObj = null;
		}
		// updating texts
		GameObject.Find("MoneyButton").transform.
				   Find("Text").GetComponent<TextMesh>().text = mMoney.ToString() + "G" + "(-"
				   											  + (Rabbit.rabbitList.Count * COST_MAINTENANCE).ToString() + "G)";
		GameObject.Find("CountButton").transform.
				   Find("Text").GetComponent<TextMesh>().text = Rabbit.rabbitList.Count.ToString() + "마리";
	}

	void decMoney(){
		mMoney -= Rabbit.rabbitList.Count * COST_MAINTENANCE;
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