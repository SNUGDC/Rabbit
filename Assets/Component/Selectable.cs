using UnityEngine;
using System.Collections;

public class Selectable : MonoBehaviour {

	public bool selected{
		get{
			return mSelected;
		}
		set{
			mSelected = value;
		}
	}

	private bool mSelected;

	// Use this for initialization
	void Start () {
		mSelected = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);
			mSelected = (hit.collider.gameObject == gameObject);
		}
		else if(Input.GetMouseButtonUp(0)){
			mSelected = false;
		}
	}
}
