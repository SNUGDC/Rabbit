using UnityEngine;
using System.Collections;

public class scriptMainMenu : MonoBehaviour {
	private bool inCredit;
	// Use this for initialization
	void Start(){
		//Cursor.SetCursor(Resources.Load<Texture2D>("cursor_click1"), new Vector2(-2, 15), CursorMode.Auto);
		inCredit = false;
	}
	// Update is called once per frame
	void Update(){
		if(Input.GetMouseButton(0)){
			//Cursor.SetCursor(Resources.Load<Texture2D>("cursor_click2"), new Vector2(-2, 15), CursorMode.Auto);
		}
		else{
			//Cursor.SetCursor(Resources.Load<Texture2D>("cursor_click1"), new Vector2(-2, 15), CursorMode.Auto);
		}
		if(Input.GetMouseButtonUp(0)){
			Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);
			GameObject mUpObj = (hit.collider == null) ? null : hit.collider.gameObject;
			if(mUpObj != null){
				switch(mUpObj.name){
					case "Start" :
						Application.LoadLevel("sceneLevelSelect");
						break;
					case "CreditButton" :
						if(inCredit){
							GameObject.Find("Start").GetComponent<BoxCollider2D>().enabled = true;
							GameObject.Find("Credit").GetComponent<SpriteRenderer>().enabled = false;
							inCredit = false;
						}
						else{
							GameObject.Find("Start").GetComponent<BoxCollider2D>().enabled = false;
							GameObject.Find("Credit").GetComponent<SpriteRenderer>().enabled = true;
							inCredit = true;
						}
						break;
				}
			}
		}
	}
}
