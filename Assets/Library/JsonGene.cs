using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public struct JsonGene{
	public static List<JsonGene> list = new List<JsonGene>();

	public string name; // name of gene
	public string[][] factorList; // list of factors
								  // 1st index : dominancy(priority of factor)
	public uint numDominant; // number of multi-dominant genes
	public object[][][] factorHashTable; // table of combination of factors
										 // 1st index : dominancy
										 // 2nd, 3rd index : index of factors

	public static void init(){
		// initialize
		list.Clear();
		System.IO.StringReader inFile = new System.IO.StringReader (Resources.Load<TextAsset> ("GeneFile").text);
		string read = null, json = null;
		// read each JsonGene from GeneFile
		while(inFile.Peek() >= 0){
			do{
				read = inFile.ReadLine();
				json += read + "\n";
			}while(read != "}");
			list.Add(JsonMapper.ToObject<JsonGene>(json));
			json = null;
		}
		inFile.Close ();
		// set factorHashTable of each JsonGene
		foreach(JsonGene element in list){
			switch(element.name){
				// pattern -> second index in rabbit's bodyList
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
				// pattern -> second index in rabbit's bodyList
				case "pattern" :
					foreach(object[][] arElement in element.factorHashTable){
						foreach(object[] arArElement in arElement){
							for(int i = 0; i < arElement.Length; ++i){
								switch(arArElement[i].ToString()){
									case "♠" :
										arArElement[i] = 0;
										break;
									case "◆" :
										arArElement[i] = 1;
										break;
									case "♥" :
										arArElement[i] = 2;
										break;
									case "♣" :
										arArElement[i] = 3;
										break;
								}
							}
						}
					}
					break;
				case "ear" :
					foreach(object[][] arElement in element.factorHashTable){
						foreach(object[] arArElement in arElement){
							for(int i = 0; i < arElement.Length; ++i){
								switch(arArElement[i].ToString()){
									case "none" :
										arArElement[i] = 0;
										break;
									case "down" :
										arArElement[i] = 1;
										break;
									case "round" :
										arArElement[i] = 2;
										break;
								}
							}
						}
					}
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
									case "Red" :
										arArElement[i] = new Color(1.0f, 0.0f, 0.0f);
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
					foreach(object[][] arElement in element.factorHashTable){
						foreach(object[] arArElement in arElement){
							for(int i = 0; i < arElement.Length; ++i){
								switch(arArElement[i].ToString()){
									case "0" :
										arArElement[i] = 0;
										break;
									case "1" :
										arArElement[i] = 1;
										break;
								}
							}
						}
					}
					break;
				case "length" :
					foreach(object[][] arElement in element.factorHashTable){
						foreach(object[] arArElement in arElement){
							for(int i = 0; i < arElement.Length; ++i){
								switch(arArElement[i].ToString()){
									case "0" :
										arArElement[i] = 0;
										break;
									case "1" :
										arArElement[i] = 1;
										break;
									case "2" :
										arArElement[i] = 2;
										break;
								}
							}
						}
					}
					break;
				default :
					break;
			}
		}
	}
}
