using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FarmFunc : MonoBehaviour {
	
	public static readonly ulong carrotSeeDistance = 200;

	// Use this for initialization
	void Start () {
	}
	// Update is called once per frame
	void Update () {
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