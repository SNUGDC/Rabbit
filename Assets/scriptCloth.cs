using UnityEngine;
using System.Collections;

public class scriptCloth : MonoBehaviour {

	public static readonly int MAX_INDEX = 7;
	public static readonly string[] clothList = new string[]{"Bunny", "China", "Dot", "Guna", "Nurse", "Seeth", "Wedding"};
	public static int clothIndex = 0;

	private int mCurIndex;

	// Use this for initialization
	void Start () {
		mCurIndex = 0;
		GameObject.Find("Sister").GetComponent<Animator>().SetInteger("ClothIndex", mCurIndex);
		//Cursor.SetCursor(Resources.Load<Texture2D>("cursor_click1"), new Vector2(-2, 15), CursorMode.Auto);
	}
	
	// Update is called once per frame
	void Update () {
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
			GameObject objSister;
			if(mUpObj != null){
				switch(mUpObj.name){
					case "Left" :
						mCurIndex = (MAX_INDEX + mCurIndex - 1) % MAX_INDEX;
						Destroy(GameObject.Find("Sister"));
						objSister = (GameObject)Instantiate(Resources.Load("prefabSister" + clothList[mCurIndex]), new Vector3(0, -20, -1), Quaternion.identity);
						objSister.name = "Sister";
						break;
					case "Right" :
						mCurIndex = (mCurIndex + 1) % MAX_INDEX;
						Destroy(GameObject.Find("Sister"));
						objSister = (GameObject)Instantiate(Resources.Load("prefabSister" + clothList[mCurIndex]), new Vector3(0, -20, -1), Quaternion.identity);
						objSister.name = "Sister";
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
