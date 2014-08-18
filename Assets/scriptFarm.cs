using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class scriptFarm : MonoBehaviour {
	public enum State{MAIN, MONEY, COUNT, STORE};

	private static readonly int MONEY_START = 10000;

	private int mMoney;
	private State mCurState;
	private Camera mCurCam;
	private GameObject mSelObj;

	void Start(){
		mMoney = MONEY_START;
		mCurCam = Camera.main;
		mCurState = State.MAIN;
		Rabbit.init();
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
							case "GUI" :
								switch(mSelObj.name){
									case "MoneyButton" :
										mCurState = State.MONEY;
										mCurCam = GameObject.Find("List Camera").GetComponent<Camera>();
										mCurCam.enabled = true;
										break;
									case "CountButton" :
										mCurState = State.COUNT;
										mCurCam = GameObject.Find("List Camera").GetComponent<Camera>();
										mCurCam.enabled = true;
										break;
									case "StoreButton" :
										mCurState = State.STORE;
										mCurCam = GameObject.Find("List Camera").GetComponent<Camera>();
										mCurCam.enabled = true;
										break;
								}
								break;
							case "Rabbit" :
								break;
						}
						break;
					case State.MONEY :
						switch(mSelObj.tag){
							case "GUI" :
								switch(mSelObj.name){
									case "ExitButton" :
										mCurState = State.MAIN;
										mCurCam.enabled = false;
										mCurCam = Camera.main;
										break;
								}
								break;
							case "Dummy" :
								break;
						}
						break;
					case State.COUNT :
						switch(mSelObj.tag){
							case "GUI" :
								switch(mSelObj.name){
									case "ExitButton" :
										mCurState = State.MAIN;
										mCurCam.enabled = false;
										mCurCam = Camera.main;
										break;
								}
								break;
							case "Dummy" :
								break;
						}
						break;
					case State.STORE :
						switch(mSelObj.tag){
							case "GUI" :
								switch(mSelObj.name){
									case "ExitButton" :
										mCurState = State.MAIN;
										mCurCam.enabled = false;
										mCurCam = Camera.main;
										break;
								}
								break;
							case "Dummy" :
								break;
						}
						break;
				}
			}
			else{
				mSelObj = null;
			}
		}
		// updating texts
		GameObject.Find("MoneyButton").transform.
				   Find("Text").GetComponent<TextMesh>().text = mMoney.ToString() + "G" + "(-" + 0.ToString() + "G)";
		GameObject.Find("CountButton").transform.
				   Find("Text").GetComponent<TextMesh>().text = Rabbit.rabbitList.Count.ToString() + "마리";
	}
}