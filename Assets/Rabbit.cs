using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rabbit : MonoBehaviour {
	public static List<GameObject> rabbitList;

	public static void init(){
		rabbitList = new List<GameObject>();
	}

	public static void create(GameObject father, GameObject mother){
		print("new Rabbit!");
	}

	void Start(){

	}
	void Update(){
		
	}
}
