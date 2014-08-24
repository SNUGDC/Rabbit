using UnityEngine;
using System.Collections;

public class scriptCloth : MonoBehaviour {

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonUp(0)){
			Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);
			GameObject mUpObj = (hit.collider == null) ? null : hit.collider.gameObject;
			if(mUpObj != null){
				switch(mUpObj.name){
					case "Left" :
						break;
					case "Right" :
						break;
					case "Apply" :
						break;
				}
			}
		}
	}
}
