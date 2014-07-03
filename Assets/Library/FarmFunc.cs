using UnityEngine;
using System.Collections;

public struct Gene{
	public enum Law{BASIC};
	public enum Type{DOMINANT, RECESSIVE};
	
	public string name{
		get{
			return mName;
		}
	}
	public Law law{
		get{
			return mLaw;
		}
	}
	public Type[] type{
		get{
			return mType;
		}
	}
	
	private string mName;
	private Law mLaw;
	private Type[] mType;
	
	public Gene(string inName, Law inLaw, Type firstType, Type secondType){
		mType = new Type[2];
		mName = inName; mLaw = inLaw; mType[0] = firstType; mType[1] = secondType;
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
	
	public static Rabbit createRabbit(Rabbit father, Rabbit mother){
		Vector3 worldLeftBottom = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
		Vector3 worldRightTop = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.9f, Screen.height * 0.9f, 0));
		Vector3 tempPosition = new Vector3(Random.Range (worldLeftBottom.x, worldRightTop.x),
		                                   Random.Range (worldLeftBottom.y, worldRightTop.y), 0);
		GameObject newRabbit = (GameObject)Instantiate(scriptFarm.objRabbit, tempPosition, Quaternion.identity);
		if(father == null || mother == null){
			newRabbit.GetComponent<Rabbit>().geneList.Add (new Gene("Ear", Gene.Law.BASIC, Gene.Type.DOMINANT, Gene.Type.RECESSIVE));
		}
		else{
			int tempRandom = Random.Range(0, 2);
			Gene.Type first = (tempRandom == 0) ? father.geneList[0].type[0] : father.geneList[0].type[1];
			tempRandom = Random.Range(0, 2);
			Gene.Type second = (tempRandom == 0) ? mother.geneList[0].type[0] : mother.geneList[0].type[1];
			newRabbit.GetComponent<Rabbit>().geneList.Add (new Gene("Ear", Gene.Law.BASIC, first, second));
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