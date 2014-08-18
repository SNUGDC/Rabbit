using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class scriptFarm : MonoBehaviour {
	public enum State{MAIN, MONEY, COUNT, STORE};

	State mCurState;
	Camera mCurCam;

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

	void Start(){
		mCurCam = Camera.main;
		mCurState = State.MAIN;
	}
	void Update(){
		if(Input.GetMouseButtonDown(0)){
			Vector2 ray = mCurCam.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);
			if(hit.collider != null){
				GameObject selected = hit.collider.gameObject;
				switch(mCurState){
					case State.MAIN :
						switch(selected.tag){
							case "gui" :
								switch(selected.name){
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
						switch(selected.tag){
							case "gui" :
								switch(selected.name){
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
						switch(selected.tag){
							case "gui" :
								switch(selected.name){
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
						switch(selected.tag){
							case "gui" :
								switch(selected.name){
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
		}
	}
}