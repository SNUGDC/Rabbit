using UnityEngine;
using LitJson;
using System.Collections;
using System.Collections.Generic;

// Gene = factor + factor

// reference data of Gene class
public struct JsonGene{
	public string name; // name of gene
	public string[][] factorList; // list of factors
								  // 1st index : dominancy(priority of factor)
	public uint numDominant; // number of multi-dominant genes
	public object[][][] factorHashTable; // table of combination of factors
										 // 1st index : dominancy
										 // 2nd, 3rd index : index of factors
}

public class Gene{
	/*-----public static variable-----*/
	public static List<JsonGene> jsonGeneList = new List<JsonGene>();

	/*-----public member variable-----*/
	public string name; // name of gene
	public string[ , ] factor; // list of name of factors
							   // 1st index : numDominant
							   // 2nd index : one of two factors in gene (gene always have two factors)

	/*-----private member variable-----*/
	private int[ , , ] factorIndex; // list of index of factors in JsonGene
									// 1st index : numDominant
									// 2nd index : one of two factors in gene
									// 3rd index : one of two indexes of factorList of JsonGene
	private JsonGene originalGene; // reference data

	/*-----public static function-----*/
	public static void init(){
		// initialize
		jsonGeneList.Clear();
		System.IO.StringReader inFile = new System.IO.StringReader (Resources.Load<TextAsset> ("GeneFile").text);
		string read = null, json = null;
		// read each JsonGene from GeneFile
		while(inFile.Peek() >= 0){
			do{
				read = inFile.ReadLine();
				json += read + "\n";
			}while(read != "}");
			jsonGeneList.Add(JsonMapper.ToObject<JsonGene>(json));
			json = null;
		}
		inFile.Close ();
		// set factorHashTable of each JsonGene
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

	/*-----public member function-----*/
	// Constructor - create new gene from JsonGene
	public Gene(JsonGene original){
		// initialize
		name = original.name;
		factor = new string[original.numDominant, 2];
		factorIndex = new int[original.numDominant, 2, 2];
		originalGene = original;
		int totalIndex = 0;
		// find total number of factors
		foreach(string[] element in original.factorList){
			totalIndex += element.Length;
		}
		// pick random factor & assign factor's index
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
	
	// Constructor - inherit from father Gene & mother Gene
	public Gene(Gene father, Gene mother){
		// name doesn't match
		if(father.name != mother.name){
			name = null;
			factor = null;
			factorIndex = null;
			return;
		}
		// initialize
		name = father.name;
		factor = new string[father.factor.GetLength(0), 2];
		factorIndex = new int[father.factor.GetLength(0), 2, 2];
		originalGene = father.originalGene;
		// inherit operation - get one factor from father, another factor from mother
		for(int i = 0; i < father.factor.GetLength(0); ++i){
			// get first factor from father
			if(Random.Range(0, 2) == 0){
				int randIndex = Random.Range(0, 2);
				factor[i, 0] = father.factor[i, randIndex];
				factorIndex[i, 0, 0] = father.factorIndex[i, randIndex, 0];
				factorIndex[i, 0, 1] = father.factorIndex[i, randIndex, 1];
				randIndex = Random.Range(0, 2);
				factor[i, 1] = mother.factor[i, randIndex];
				factorIndex[i, 1, 0] = mother.factorIndex[i, randIndex, 0];
				factorIndex[i, 1, 1] = mother.factorIndex[i, randIndex, 1];
			}
			// get first factor from mother
			else{
				int randIndex = Random.Range(0, 2);
				factor[i, 0] = mother.factor[i, randIndex];
				factorIndex[i, 0, 0] = mother.factorIndex[i, randIndex, 0];
				factorIndex[i, 0, 1] = mother.factorIndex[i, randIndex, 1];
				randIndex = Random.Range(0, 2);
				factor[i, 1] = father.factor[i, randIndex];
				factorIndex[i, 1, 0] = father.factorIndex[i, randIndex, 0];
				factorIndex[i, 1, 1] = father.factorIndex[i, randIndex, 1];
			}
		}
	}
	
	// get phenotype of gene from factorHashTable
	// in multi-dominant gene, result is average of each phenotype
	public T Phenotype<T>(T baseObject, System.Func<T, T, T> Add, System.Func<T, int, T> Divide){
		T result = baseObject;
		for(int i = 0; i < factor.GetLength(0); ++i){
			//check dominancy
			if(factorIndex[i, 0, 0] == factorIndex[i, 1, 0]){
				result = Add(result, (T)(originalGene.factorHashTable[factorIndex[i, 0, 0]][factorIndex[i, 0, 1]][factorIndex[i, 1, 1]]));
			}
			else if(factorIndex[i, 0, 0] > factorIndex[i, 1, 0]){
				result = Add(result, (T)(originalGene.factorHashTable[factorIndex[i, 1, 0]][factorIndex[i, 1, 1]][factorIndex[i, 1, 1]]));
			}
			else{
				result = Add(result, (T)(originalGene.factorHashTable[factorIndex[i, 0, 0]][factorIndex[i, 0, 1]][factorIndex[i, 0, 1]]));
			}
		}
		result = Divide(result, (int)(factor.GetLength(0)));
		return result;
	}
}

public class FarmFunc : MonoBehaviour {
	
	public static readonly ulong carrotSeeDistance = 200;

	// Use this for initialization
	void Start () {
	}
	// Update is called once per frame
	void Update () {
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