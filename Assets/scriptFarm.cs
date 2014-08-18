using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class scriptFarm : MonoBehaviour {
	public enum State{MAIN, MONEY, COUNT, STORE};

	State mCurState;
	Camera mCurCam;
	GameObject mSelObj;

	void Start(){
		mCurCam = Camera.main;
		mCurState = State.MAIN;
	}
	void Update(){
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
						}
						break;
				}
			}
			else{
				mSelObj = null;
			}
		}
	}
}