using UnityEngine;
using System.Collections;

public class Draggable : MonoBehaviour {

	public bool select{
		get{
			return mSelect;
		}
		set{
			mSelect = value;
		}
	}

	private bool mSelect;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnMouseDrag(){
		if(mSelect){
			transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
		}
	}
}
