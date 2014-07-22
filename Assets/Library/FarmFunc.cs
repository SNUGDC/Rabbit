using UnityEngine;
using LitJson;
using System.Collections;
using System.Collections.Generic;

public struct JsonGene{
	public string name;
	public string[][] factorList;
	public uint numDominant;
	public object[][][] factorHashTable;
}

public struct Gene{
	public string name;
	public string[ , ] factor;
	//multi-dominant gene x 2
	private int[ , , ] factorIndex;
	private JsonGene originalGene;
	public Gene(JsonGene original){
		name = original.name;
		factor = new string[original.numDominant, 2];
		factorIndex = new int[original.numDominant, 2, 2];
		originalGene = original;
		int totalIndex = 0;
		foreach(string[] element in original.factorList){
			totalIndex += element.Length;
		}
		for(int i = 0; i < original.numDominant; ++i){
			for(int j = 0; j < 2; ++j){
				int count = 0, tempIndex = Random.Range(0, totalIndex);
				while(tempIndex >= original.factorList[count].Length){
					tempIndex -= original.factorList[count++].Length;
				}
				factor[i, j] = original.factorList[count][tempIndex];
				factorIndex[i, j, 0] = count;
				factorIndex[i, j, 1] = tempIndex;
			}
		}
	}
	
	public Gene(Gene father, Gene mother){
		if(father.name != mother.name){
			name = null;
			factor = null;
			factorIndex = null;
			return;
		}
		name = father.name;
		factor = new string[father.factor.GetLength(0), 2];
		factorIndex = new int[father.factor.GetLength(0), 2, 2];
		originalGene = father.originalGene;
		for(int i = 0; i < father.factor.GetLength(0); ++i){
			if(Random.Range(0, 2) == 0){
				factor[i, 0] = father.factor[i, 0];
				factorIndex[i, 0, 0] = father.factorIndex[i, 0, 0];
				factorIndex[i, 0, 1] = father.factorIndex[i, 0, 1];
				factor[i, 1] = mother.factor[i, 1];
				factorIndex[i, 1, 0] = mother.factorIndex[i, 1, 0];
				factorIndex[i, 1, 1] = mother.factorIndex[i, 1, 1];
			}
			else{
				factor[i, 0] = mother.factor[i, 0];
				factorIndex[i, 0, 0] = mother.factorIndex[i, 0, 0];
				factorIndex[i, 0, 1] = mother.factorIndex[i, 0, 1];
				factor[i, 1] = father.factor[i, 1];
				factorIndex[i, 1, 0] = father.factorIndex[i, 1, 0];
				factorIndex[i, 1, 1] = father.factorIndex[i, 1, 1];
			}
		}
	}
	
	public T Phenotype<T>(T baseObject, System.Func<T, T, T> Add, System.Func<T, float, T> Divide){
		T result = baseObject;
		for(int i = 0; i < factor.GetLength(0); ++i){
			if(factorIndex[i, 0, 0] == factorIndex[i, 1, 0]){
				result = Add(result, (T)(originalGene.factorHashTable[factorIndex[i, 0, 0]][factorIndex[i, 0, 1]][factorIndex[i, 1, 1]]));
			}
		}
		result = Divide(result, (float)(factor.GetLength(0)));
		return result;
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
		System.IO.StringReader inFile = new System.IO.StringReader (Resources.Load<TextAsset> ("GeneFile").text);
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
		foreach(JsonGene element in jsonGeneList){
			switch(element.name){
				case "size" :
					break;
				case "color" :
					foreach(object[][] arElement in element.factorHashTable){
						foreach(object[] arArElement in arElement){
							for(int i = 0; i < arArElement.Length; ++i){
								switch(arArElement[i].ToString()){
									case "Black" :
										arArElement[i] = new Color(0.0f, 0.0f, 0.0f);
										break;
									case "Gray" :
										arArElement[i] = new Color(0.5f, 0.5f, 0.5f);
										break;
									case "White" :
										arArElement[i] = new Color(1.0f, 1.0f, 1.0f);
										break;
									case "Pink" :
										arArElement[i] = new Color(1.0f, 0.75f, 0.8f);
										break;
									case "SkyBlue" :
										arArElement[i] = new Color(0.53f, 0.8f, 0.92f);
										break;
									case "Primrose" :
										arArElement[i] = new Color(1.0f, 1.0f, 0.5f);
										break;
									case "Red" :
										arArElement[i] = new Color(1.0f, 0.0f, 0.0f);
										break;
									case "Purple" :
										arArElement[i] = new Color(0.5f, 0.0f, 0.5f);
										break;
									case "Orange" :
										arArElement[i] = new Color(1.0f, 0.65f, 0.0f);
										break;
									case "Blue" :
										arArElement[i] = new Color(0.0f, 0.0f, 1.0f);
										break;
									case "Green" :
										arArElement[i] = new Color(0.0f, 0.5f, 0.0f);
										break;
									case "Yellow" :
										arArElement[i] = new Color(1.0f, 1.0f, 0.0f);
										break;
									default :
										arArElement[i] = new Color(1.0f, 1.0f, 1.0f);
										break;
								}
							}
						}
					}
					break;
				case "pattern" :
					break;
				case "ear" :
					break;
				case "eyecolor" :
					foreach(object[][] arElement in element.factorHashTable){
						foreach(object[] arArElement in arElement){
							for(int i = 0; i < arArElement.Length; ++i){
								switch(arArElement[i].ToString()){
								case "Black" :
									arArElement[i] = new Color(0.0f, 0.0f, 0.0f);
									break;
								case "Blue" :
									arArElement[i] = new Color(0.0f, 0.0f, 1.0f);
									break;
								case "Jade" : 
									arArElement[i] = new Color(0.61f, 0.83f, 0.76f);
									break;
								default :
									arArElement[i] = new Color(1.0f, 1.0f, 1.0f);
									break;
								}
							}
						}
					}
					break;
				case "teeth" :
					break;
				case "length" :
					break;
				default :
					break;
			}
		}
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