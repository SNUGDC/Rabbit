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

	private static int mSWidth = Screen.width;
	private static int mSHeight = Screen.height;

	// Use this for initialization
	void Start(){
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
	}
	
	// Update is called once per frame
	void Update(){}

	void OnGUI(){
		if(GUI.Button (new Rect (mSWidth * 0.075f, mSHeight * 0.1f, mSWidth * 0.1f, mSHeight * 0.1f), "1")) {
			load(1);
		}
		if(GUI.Button (new Rect (mSWidth * 0.225f, mSHeight * 0.1f, mSWidth * 0.1f, mSHeight * 0.1f), "2")) {
			load(2);
		}
		if(GUI.Button (new Rect (mSWidth * 0.375f, mSHeight * 0.1f, mSWidth * 0.1f, mSHeight * 0.1f), "3")) {
			load(3);
		}
		if(GUI.Button (new Rect (mSWidth * 0.525f, mSHeight * 0.1f, mSWidth * 0.1f, mSHeight * 0.1f), "4")) {
			load(4);
		}
		if(GUI.Button (new Rect (mSWidth * 0.675f, mSHeight * 0.1f, mSWidth * 0.1f, mSHeight * 0.1f), "5")) {
			load(5);
		}
		if(GUI.Button (new Rect (mSWidth * 0.825f, mSHeight * 0.1f, mSWidth * 0.1f, mSHeight * 0.1f), "6")) {
			load(6);
		}
		if(GUI.Button (new Rect (mSWidth * 0.075f, mSHeight * 0.3f, mSWidth * 0.1f, mSHeight * 0.1f), "7")) {
			load(7);
		}
		if(GUI.Button (new Rect (mSWidth * 0.225f, mSHeight * 0.3f, mSWidth * 0.1f, mSHeight * 0.1f), "8")) {
			load(8);
		}
		if(GUI.Button (new Rect (mSWidth * 0.375f, mSHeight * 0.3f, mSWidth * 0.1f, mSHeight * 0.1f), "9")) {
			load(9);
		}
		if(GUI.Button (new Rect (mSWidth * 0.525f, mSHeight * 0.3f, mSWidth * 0.1f, mSHeight * 0.1f), "10")) {
			load(10);
		}
		if(GUI.Button (new Rect (mSWidth * 0.4f, mSHeight * 0.85f, mSWidth * 0.2f, mSHeight * 0.05f), "Back")) {
			Application.LoadLevel("sceneMainMenu");
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
