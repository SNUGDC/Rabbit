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
	
	public GameObject objRabbit;

	// Use this for initialization
	void Start () {
	}
	// Update is called once per frame
	void Update () {
	}
	
	public static Rabbit selectRabbit(){
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Rabbit result = null;
		if(Physics.Raycast(ray, out hit, Mathf.Infinity)){
			if(hit.transform.tag == "rabbit"){
				result = hit.transform.GetComponent<Rabbit>();
				result.selected = true;
			}
		}
		return result;
	}
	public static Rabbit findAnotherRabbit(Rabbit target){
		Rabbit result = null;
		RaycastHit[] hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		hit = Physics.RaycastAll(ray, Mathf.Infinity);
		for(int i = 0; i < hit.Length; ++i){
			if(hit[i].transform.GetComponent<Rabbit>() != target){
				result = hit[i].transform.GetComponent<Rabbit>();
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
		newRabbit.tag = "rabbit";
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
}