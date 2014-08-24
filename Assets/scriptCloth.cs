using UnityEngine;
using System.Collections;

public class scriptCloth : MonoBehaviour {

	public static readonly int MAX_INDEX = 2;
	public static int clothIndex = 0;

	private int mCurIndex;

	// Use this for initialization
	void Start () {
		mCurIndex = 0;
		GameObject.Find("Sister").GetComponent<Animator>().SetInteger("ClothIndex", mCurIndex);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonUp(0)){
			Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);
			GameObject mUpObj = (hit.collider == null) ? null : hit.collider.gameObject;
			if(mUpObj != null){
				switch(mUpObj.name){
					case "Left" :
						mCurIndex = (MAX_INDEX + mCurIndex - 1) % MAX_INDEX;
						GameObject.Find("Sister").GetComponent<Animator>().SetInteger("ClothIndex", mCurIndex);
						break;
					case "Right" :
						mCurIndex = (mCurIndex + 1) % MAX_INDEX;
						GameObject.Find("Sister").GetComponent<Animator>().SetInteger("ClothIndex", mCurIndex);
						break;
					case "Apply" :
						clothIndex = mCurIndex;
						break;
					case "Return" :
						Application.LoadLevel("sceneLevelSelect");
						break;
				}
			}
		}
	}
}
