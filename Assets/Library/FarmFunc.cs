using UnityEngine;
using LitJson;
using System.Collections;
using System.Collections.Generic;

public struct JsonGene{
	public string name;
	public string[] domGene;
	public string[] recGene;
	public uint numFactor;
	public uint numDominant;
}

public struct Gene{
	public string name;
	public int index;
	public string[,] factor;
	public Gene(JsonGene original){
		name = original.name;
		index = FarmFunc.jsonGeneList.IndexOf(original);
		if(original.domGene == null && original.recGene == null){
			factor = null;
			return;
		}
		factor = new string[original.numDominant, original.numFactor];
		int tempRandom;
		for(int i = 0; i < original.numDominant; ++i){
			for(int j = 0; j < original.numFactor; ++j){
				tempRandom = Random.Range(0, original.domGene.Length + original.recGene.Length);
				factor[i, j] = (tempRandom >= original.domGene.Length) ? original.recGene[tempRandom - original.domGene.Length] : original.domGene[tempRandom];
			}
		}
	}
	public Gene(Gene father, Gene mother){
		if(father.name != mother.name || father.index != mother.index){
			name = "error!";
			index = -1;
			return;
		}
		name = father.name;
		index = father.index;
		factor = new string[father.factor.GetLength(0), father.factor.GetLength(1)];
		for(int i = 0; i < factor.GetLength(0); ++i){
			for(int j = 0; j < factor.GetLength(1); ++j){
				factor[i, j] = (Random.Range(0, 2) == 0) ? father.factor[i, j] : mother.factor[i, j];
			}
		}
	}
}

public class FarmFunc : MonoBehaviour {
	
	public static readonly ulong carrotSeeDistance = 200;
	public static List<JsonGene> jsonGeneList = new List<JsonGene>();

	// Use this for initialization
	void Start () {
	}
	// Update is called once per frame
	void Update () {
	}
	
	public static void init(){
		jsonGeneList.Clear();
		System.IO.StreamReader inFile = new System.IO.StreamReader("Assets/GeneFile.json");
		string read = null, json = null;
		while(inFile.Peek() >= 0){
			do{
				read = inFile.ReadLine();
				json += read + "\n";
			}while(read != "}");
			jsonGeneList.Add(JsonMapper.ToObject<JsonGene>(json));
			json = null;
		}
		inFile.Close ();
	}
	
	public static Rabbit selectRabbit(){
		Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		RaycastHit2D[] hit = Physics2D.RaycastAll(ray, Vector2.zero);
		Rabbit result = null;
		foreach(RaycastHit2D element in hit){
			if(element.collider != null && element.transform.tag == "rabbit"){
				result = element.transform.GetComponent<Rabbit>();
				result.selected = true;
				break;
			}
		}
		return result;
	}
	
	public static Rabbit findAnotherRabbit(Rabbit target){
		Rabbit result = null;
		RaycastHit2D[] hit;
		Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		hit = Physics2D.RaycastAll(ray, Vector2.zero);
		foreach(RaycastHit2D element in hit){
			if(element.collider != null && element.transform.GetComponent<Rabbit>() != target){
				result = element.transform.GetComponent<Rabbit>();
				break;
			}
		}
		return result;
	}
	
	public static Rabbit createRabbit(Rabbit father, Rabbit mother){
		Vector3 worldLeftBottom = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
		Vector3 worldRightTop = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.9f, Screen.height * 0.9f, 0));
		Vector3 tempPosition = new Vector3(Random.Range (worldLeftBottom.x, worldRightTop.x),
		                                   Random.Range (worldLeftBottom.y, worldRightTop.y), 0);
		GameObject newRabbit = (GameObject)Instantiate(scriptFarm.objRabbit, tempPosition, Quaternion.identity);
		if(father == null || mother == null){
			foreach(JsonGene element in jsonGeneList){
				newRabbit.GetComponent<Rabbit>().geneList.Add (new Gene(element));
			}
		}
		else{
			for(int i = 0; i < father.geneList.Count; ++i){
				newRabbit.GetComponent<Rabbit>().geneList.Add(new Gene(father.geneList[i], mother.geneList[i]));
			}
		}
		return newRabbit.GetComponent<Rabbit>();
	}
	
	public static Carrot createCarrot(float x, float y){
		Vector3 tempPosition = Camera.main.ScreenToWorldPoint(new Vector3(x, y, 0));
		tempPosition.z = 0;
		GameObject newCarrot = (GameObject)Instantiate(scriptFarm.objCarrot, tempPosition, Quaternion.identity);
		return newCarrot.GetComponent<Carrot>();
	}
	
	public static Carrot findNearCarrot(float rabbitX, float rabbitY){
		Carrot result = null;
		float minDistance = carrotSeeDistance;
		float tempDistance = 0;
		foreach(Carrot element in scriptFarm.carrotList){
			tempDistance = Vector2.Distance((Vector2)(element.transform.position), new Vector2(rabbitX, rabbitY));
			if(tempDistance < minDistance){
				minDistance = tempDistance;
				result = element;
			}
		}
		return result;
	}
}