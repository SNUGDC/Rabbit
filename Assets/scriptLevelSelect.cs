using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class scriptLevelSelect : MonoBehaviour {

	public struct LevelData{
		public string[] script;
		public bool[] condition;
		public int count;
		public string[][][] start;
		public string[][] target;
		public string[] targetText;
	}

	public static int level;
	public static List<GeneNode> geneList;
	public static List<LevelData> levelList;
	public static bool[] clearList = new bool[]{false, false, false, false, false, false, false, false, false, false};

	// Use this for initialization
	void Start(){
		//Cursor.SetCursor(Resources.Load<Texture2D>("cursor_click1"), new Vector2(-2, 15), CursorMode.Auto);
		JsonGene.init();
		levelList = new List<LevelData>();
		//System.IO.StringReader inFile = new System.IO.StringReader (Resources.Load<TextAsset> ("LevelData").text);
		System.IO.StreamReader inFile = new System.IO.StreamReader (Application.dataPath + "/LevelData.json");
		string read = null, json = null;
		// read each JsonGene from GeneFile
		while(inFile.Peek() >= 0){
			inFile.ReadLine();
			do{
				read = inFile.ReadLine();
				json += read + "\n";
			}while(read != "}");
			levelList.Add(JsonMapper.ToObject<scriptLevelSelect.LevelData>(json));
			json = null;
		}
		inFile.Close ();
		for(int i = 1; i <= clearList.Length; ++i){
			if(clearList[i - 1]){
				GameObject.Find("LevelBox" + i.ToString()).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("boxWithStar3");
			}
			else{
				GameObject.Find("LevelBox" + i.ToString()).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("box");
			}
		}
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
					case "LevelBox1" :
						load(1);
						break;
					case "LevelBox2" :
						load(2);
						break;
					case "LevelBox3" :
						load(3);
						break;
					case "LevelBox4" :
						load(4);
						break;
					case "LevelBox5" :
						load(5);
						break;
					case "LevelBox6" :
						load(6);
						break;
					case "LevelBox7" :
						load(7);
						break;
					case "LevelBox8" :
						load(8);
						break;
					case "LevelBox9" :
						load(9);
						break;
					case "LevelBox10" :
						load(10);
						break;
					case "ToCloth" :
						Application.LoadLevel("sceneCloth");
						break;
				}
			}
		}
	}

	private void load(int index){
		level = index;
		GameObject targetRabbit = (GameObject)Instantiate(Resources.Load<GameObject>("prefabRabbit"), new Vector2(-700, 0), Quaternion.identity);
		targetRabbit.GetComponent<Rabbit>().enabled = false;
		targetRabbit.GetComponent<Draggable>().enabled = false;
		targetRabbit.GetComponent<Gene>().create(null, null);
		if(levelList[level - 1].target != null){
			for(int i = 0; i < levelList[level - 1].target[0].Length; ++i){
				for(int j = 0; j < levelList[level - 1].target[i + 1].Length; j += 2){
					targetRabbit.GetComponent<Gene>().setField(levelList[level - 1].target[0][i],
															   j / 2,
															   levelList[level - 1].target[i + 1][j],
															   levelList[level - 1].target[i + 1][j + 1]);
				}
			}
		}
		geneList = targetRabbit.GetComponent<Gene>().list;
		Application.LoadLevel("sceneFarm");
	}
}
