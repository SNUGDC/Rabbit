using UnityEngine;
using LitJson;
using System.Collections;
using System.Collections.Generic;

public struct GeneNode{
	/*-----public member variable-----*/
	public string name; // name of gene
	public string[ , ] factor; // list of name of factors
							   // 1st index : numDominant
							   // 2nd index : one of two factors in gene (gene always have two factors)
	public int[ , , ] factorIndex; // list of index of factors in JsonGene
									// 1st index : numDominant
									// 2nd index : one of two factors in gene
									// 3rd index : one of two indexes of factorList of JsonGene
	public JsonGene originalGene; // reference data

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

public class Gene : MonoBehaviour {
	/*-----public static variable-----*/

	/*-----public member function-----*/
	public List<GeneNode> list{
		get{
			return mList;
		}
	}

	private List<GeneNode> mList = new List<GeneNode>();

	void Start(){
	}

	void Update(){

	}

	public void create(Gene father, Gene mother){
		if(father == null || mother == null){
			foreach(JsonGene element in JsonGene.list){
				GeneNode newNode = new GeneNode();
				// initialize
				newNode.name = element.name;
				newNode.factor = new string[element.numDominant, 2];
				newNode.factorIndex = new int[element.numDominant, 2, 2];
				newNode.originalGene = element;
				int totalIndex = 0;
				// find total number of factors
				foreach(string[] sElement in element.factorList){
					totalIndex += sElement.Length;
				}
				// pick random factor & assign factor's index
				for(int i = 0; i < element.numDominant; ++i){
					for(int j = 0; j < 2; ++j){
						int count = 0, tempIndex = Random.Range(0, totalIndex);
						while(tempIndex >= element.factorList[count].Length){
							tempIndex -= element.factorList[count++].Length;
						}
						newNode.factor[i, j] = element.factorList[count][tempIndex];
						newNode.factorIndex[i, j, 0] = count;
						newNode.factorIndex[i, j, 1] = tempIndex;
					}
				}
				mList.Add(newNode);
			}
		}
		else{
			for(int i = 0; i < JsonGene.list.Count; ++i){
				// name doesn't match
				GeneNode newNode = new GeneNode();
				if(father.mList[i].name != mother.mList[i].name){
					newNode.name = null;
					newNode.factor = null;
					newNode.factorIndex = null;
					mList.Add(newNode);
					continue;
				}
				// initialize
				newNode.name = JsonGene.list[i].name;
				newNode.factor = new string[JsonGene.list[i].numDominant, 2];
				newNode.factorIndex = new int[JsonGene.list[i].numDominant, 2, 2];
				newNode.originalGene = JsonGene.list[i];
				// inherit operation - get one factor from father, another factor from mother
				for(int j = 0; j < JsonGene.list[i].numDominant; ++j){
					// get first factor from father
					if(Random.Range(0, 2) == 0){
						int randIndex = Random.Range(0, 2);
						newNode.factor[j, 0] = father.mList[i].factor[j, randIndex];
						newNode.factorIndex[j, 0, 0] = father.mList[i].factorIndex[j, randIndex, 0];
						newNode.factorIndex[j, 0, 1] = father.mList[i].factorIndex[j, randIndex, 1];
						randIndex = Random.Range(0, 2);
						newNode.factor[j, 1] = mother.mList[i].factor[j, randIndex];
						newNode.factorIndex[j, 1, 0] = mother.mList[i].factorIndex[j, randIndex, 0];
						newNode.factorIndex[j, 1, 1] = mother.mList[i].factorIndex[j, randIndex, 1];
					}
					// get first factor from mother
					else{
						int randIndex = Random.Range(0, 2);
						newNode.factor[j, 0] = mother.mList[i].factor[j, randIndex];
						newNode.factorIndex[j, 0, 0] = mother.mList[i].factorIndex[j, randIndex, 0];
						newNode.factorIndex[j, 0, 1] = mother.mList[i].factorIndex[j, randIndex, 1];
						randIndex = Random.Range(0, 2);
						newNode.factor[j, 1] = father.mList[i].factor[j, randIndex];
						newNode.factorIndex[j, 1, 0] = father.mList[i].factorIndex[j, randIndex, 0];
						newNode.factorIndex[j, 1, 1] = father.mList[i].factorIndex[j, randIndex, 1];
					}
				}
				mList.Add(newNode);
			}
		}
	}
}
