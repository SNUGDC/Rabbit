using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rabbit : MonoBehaviour {
	public static List<GameObject> rabbitList;

	public static void init(){
		rabbitList = new List<GameObject>();
	}

	public static void create(GameObject father, GameObject mother){
		GameObject newRabbit = (GameObject)Instantiate(scriptFarm.objRabbit, Vector2.zero, Quaternion.identity);
		// assign genes to newRabbit
		Gene fatherGene = (father == null) ? null : father.GetComponent<Gene>();
		Gene motherGene = (mother == null) ? null : mother.GetComponent<Gene>();
		newRabbit.GetComponent<Gene>().create(fatherGene, motherGene);
		try{
			newRabbit.GetComponent<Rabbit>().mFather = father.GetComponent<Rabbit>();
		}catch{}
		try{
			newRabbit.GetComponent<Rabbit>().mMother = mother.GetComponent<Rabbit>();
		}catch{}
		// add newRabbit to rabbitList
		rabbitList.Add(newRabbit);
	}

	public Rabbit mFather = null;
	public Rabbit mMother = null;
	
	private Sprite[] mSprite = new Sprite[5];

	void Start(){
		mSprite[0] = Resources.Load<Sprite>("Rabbits/ear_none_3");
		mSprite[1] = Resources.Load<Sprite>("Rabbits/body_none_3");
		mSprite[2] = Resources.Load<Sprite>("Rabbits/tail_3");
		mSprite[3] = Resources.Load<Sprite>("Rabbits/teeh_none");
		mSprite[4] = Resources.Load<Sprite>("Rabbits/leg_3");
	}
	void Update(){
		transform.Find("Head").GetComponent<SpriteRenderer>().sprite = mSprite[0];
		transform.Find("Body").GetComponent<SpriteRenderer>().sprite = mSprite[1];
		transform.Find("Tail").GetComponent<SpriteRenderer>().sprite = mSprite[2];
		transform.Find("Teeth").GetComponent<SpriteRenderer>().sprite = mSprite[3];
		transform.Find("Leg").GetComponent<SpriteRenderer>().sprite = mSprite[4];
	}
}
