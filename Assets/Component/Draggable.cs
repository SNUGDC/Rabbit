using UnityEngine;
using System.Collections;

public class Draggable : MonoBehaviour {

	public bool noDrag{
		get{
			return mNoDrag;
		}
		set{
			mNoDrag = value;
		}
	}

	private Selectable mSelectable;
	private bool mNoDrag;

	// Use this for initialization
	void Start () {
		mSelectable = GetComponent<Selectable>();
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnMouseDrag(){
		bool selected = (mSelectable == null) ? true : mSelectable.selected;
		if (selected && !mNoDrag) {
			Vector2 temp = Input.mousePosition;
			// limit draggable area
			temp.x = Mathf.Max(temp.x, 0);
			temp.x = Mathf.Min(temp.x, scriptFarm.sWidth * 0.9f);
			temp.y = Mathf.Max(temp.y, 0);
			temp.y = Mathf.Min(temp.y, scriptFarm.sHeight * 0.9f);
			transform.position = (Vector2)Camera.main.ScreenToWorldPoint(temp);
		}
	}
}
