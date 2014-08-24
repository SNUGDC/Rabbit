using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rabbit : MonoBehaviour {
	public static readonly int LIFE_MAX = 2000;
	public static readonly int LIFE_DECREASE = 1;
	
	public static List<GameObject> rabbitList;
	public static List<GameObject> dummyList;
	public static List<GameObject> textList;
	public static Sprite[ , ] headList = new Sprite[3, 5];
	public static Sprite[ , ] bodyList = new Sprite[4, 5];
	public static Sprite[] tailList = new Sprite[5];
	public static Sprite[] teethList = new Sprite[2];
	public static Sprite[] legList = new Sprite[5];

	public GameObject head;
	public GameObject body;
	public GameObject tail;
	public GameObject teeth;
	public GameObject eye;
	public GameObject rLeg;
	public GameObject lLeg;
	public GameObject bLeg;
	public GameObject b2Leg;

	public static void init(){
		rabbitList = new List<GameObject>();
		dummyList = new List<GameObject>();
		textList = new List<GameObject>();
	}

	public static void create(GameObject father, GameObject mother){
		GameObject newRabbit = (GameObject)Instantiate(scriptFarm.objRabbit, new Vector2(Random.Range(-100, 100), Random.Range(-50, 50)), Quaternion.identity);
		// assign genes to newRabbit
		Gene fatherGene = (father == null) ? null : father.GetComponent<Gene>();
		Gene motherGene = (mother == null) ? null : mother.GetComponent<Gene>();
		newRabbit.GetComponent<Gene>().create(fatherGene, motherGene);
		// add newRabbit to rabbitList
		rabbitList.Add(newRabbit);
	}

	public static void remove(GameObject input){
		rabbitList.Remove(input);
		Destroy(input);
	}

	public static void createDummy(Rabbit original){
		GameObject newDummy = (GameObject)Instantiate(scriptFarm.objDummy, new Vector2(700, 0), Quaternion.identity);
		newDummy.transform.Find("ear_down_2").GetComponent<SpriteRenderer>().sprite = original.head.GetComponent<SpriteRenderer>().sprite;
		newDummy.transform.Find("ear_down_2").renderer.material.color = original.mColor;
		newDummy.transform.Find("body_none_2").GetComponent<SpriteRenderer>().sprite = original.body.GetComponent<SpriteRenderer>().sprite;
		newDummy.transform.Find("body_none_2").renderer.material.color = original.mColor;
		newDummy.transform.Find("body_none_2")
				.transform.Find("tail_2").GetComponent<SpriteRenderer>().sprite = original.tail.GetComponent<SpriteRenderer>().sprite;
		newDummy.transform.Find("body_none_2")
				.transform.Find("tail_2").renderer.material.color = original.mColor;
		newDummy.transform.Find("body_none_2")
				.transform.Find("fr1_leg").GetComponent<SpriteRenderer>().sprite = original.rLeg.GetComponent<SpriteRenderer>().sprite;
		newDummy.transform.Find("body_none_2")
				.transform.Find("fr1_leg").renderer.material.color = original.mColor;
		newDummy.transform.Find("body_none_2")
				.transform.Find("fr2_leg").GetComponent<SpriteRenderer>().sprite = original.lLeg.GetComponent<SpriteRenderer>().sprite;
		newDummy.transform.Find("body_none_2")
				.transform.Find("fr2_leg").renderer.material.color = original.mColor;
		newDummy.transform.Find("body_none_2")
				.transform.Find("b_leg").GetComponent<SpriteRenderer>().sprite = original.bLeg.GetComponent<SpriteRenderer>().sprite;
		newDummy.transform.Find("body_none_2")
				.transform.Find("b_leg").renderer.material.color = original.mColor;
		newDummy.transform.Find("body_none_2")
				.transform.Find("b1_leg").GetComponent<SpriteRenderer>().sprite = original.b2Leg.GetComponent<SpriteRenderer>().sprite;
		newDummy.transform.Find("body_none_2")
				.transform.Find("b1_leg").renderer.material.color = original.mColor;
		newDummy.transform.Find("eye").renderer.material.color = original.mEyeColor;
		GameObject newText = (GameObject)Instantiate(scriptFarm.objText, new Vector2(700, 0), Quaternion.identity);
		newText.GetComponent<TextMesh>().text = "수명 : " + original.life.ToString() + " / " + Rabbit.LIFE_MAX.ToString();
		dummyList.Add(newDummy);
		textList.Add(newText);
	}

	public static void clearDummy(){
		foreach(GameObject element in dummyList){
			Destroy(element);
		}
		foreach(GameObject element in textList){
			Destroy(element);
		}
		dummyList.Clear();
		textList.Clear();
	}

	public bool isAdult{
		get{
			return mIsAdult;
		}
		set{
			mIsAdult = value;
		}
	}
	public int life{
		get{
			return mLife;
		}
		set{
			mLife = value;
		}
	}
	
	private int mLife = LIFE_MAX;
	private bool mIsAdult = false;
	private Color mColor = new Color(1, 1, 1);
	private Color mEyeColor = new Color(1, 1, 1);

	void Start(){
		Invoke("grow", 2);
		InvokeRepeating("decLife", 0.01f, 0.01f);
	}

	void Update(){
		transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y / 100);
	}

	void grow(){
		mIsAdult = true;
		mColor = GetComponent<Gene>().list[1].Phenotype<Color>(new Color(0, 0, 0), delegate(Color arg1, Color arg2){return arg1 + arg2;}, delegate(Color arg1, int arg2){return arg1 / arg2;});
		int patternIndex = GetComponent<Gene>().list[2].Phenotype<int>(0, delegate(int arg1, int arg2){return arg1 + arg2;}, delegate(int arg1, int arg2){return arg1 / arg2;});
		int lengthIndex = GetComponent<Gene>().list[6].Phenotype<int>(0, delegate(int arg1, int arg2){return arg1 + arg2;}, delegate(int arg1, int arg2){return arg1;});
		int earIndex = GetComponent<Gene>().list[3].Phenotype<int>(0, delegate(int arg1, int arg2){return arg1 + arg2;}, delegate(int arg1, int arg2){return arg1 / arg2;});
		int teethIndex = GetComponent<Gene>().list[5].Phenotype<int>(0, delegate(int arg1, int arg2){return arg1 + arg2;}, delegate(int arg1, int arg2){return arg1 / arg2;});
		head.renderer.material.color = mColor;
		head.GetComponent<SpriteRenderer>().sprite = RabbitSprite.manager.headList[earIndex][lengthIndex];
		body.GetComponent<SpriteRenderer>().sprite = RabbitSprite.manager.bodyList[patternIndex][lengthIndex];
		body.renderer.material.color = mColor;
		tail.GetComponent<SpriteRenderer>().sprite = RabbitSprite.manager.tailList[lengthIndex];
		tail.renderer.material.color = mColor;
		teeth.GetComponent<SpriteRenderer>().sprite = RabbitSprite.manager.teethList[teethIndex];
		rLeg.renderer.material.color = mColor;
		lLeg.renderer.material.color = mColor;
		bLeg.renderer.material.color = mColor;
		b2Leg.renderer.material.color = mColor;
		mEyeColor = GetComponent<Gene>().list[4].Phenotype<Color>(new Color(0, 0, 0), delegate(Color arg1, Color arg2){return arg1 + arg2;}, delegate(Color arg1, int arg2){return arg1 / arg2;});
		eye.renderer.material.color = mEyeColor;
	}

	void decLife(){
		if(--mLife <= 0){
			Rabbit.remove(this.gameObject);
			if(Gene.phenoEqual(this.GetComponent<Gene>(), scriptLevelSelect.geneList, scriptLevelSelect.levelList[scriptLevelSelect.level - 1].condition)){
				--(Camera.main.gameObject.GetComponent<scriptFarm>().mWinCount);
			}
		}
	}
}
