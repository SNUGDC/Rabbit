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

	public static void init(){
		rabbitList = new List<GameObject>();
		dummyList = new List<GameObject>();
		textList = new List<GameObject>();
		headList[0, 0] = Resources.Load<Sprite>("Rabbits/ear_none_1");
		headList[0, 1] = Resources.Load<Sprite>("Rabbits/ear_none_2");
		headList[0, 2] = Resources.Load<Sprite>("Rabbits/ear_none_3");
		headList[0, 3] = Resources.Load<Sprite>("Rabbits/ear_none_4");
		headList[0, 4] = Resources.Load<Sprite>("Rabbits/ear_none_5");
		headList[1, 0] = Resources.Load<Sprite>("Rabbits/ear_down_1");
		headList[1, 1] = Resources.Load<Sprite>("Rabbits/ear_down_2");
		headList[1, 2] = Resources.Load<Sprite>("Rabbits/ear_down_3");
		headList[1, 3] = Resources.Load<Sprite>("Rabbits/ear_down_4");
		headList[1, 4] = Resources.Load<Sprite>("Rabbits/ear_down_5");
		headList[2, 0] = Resources.Load<Sprite>("Rabbits/ear_round_1");
		headList[2, 1] = Resources.Load<Sprite>("Rabbits/ear_round_2");
		headList[2, 2] = Resources.Load<Sprite>("Rabbits/ear_round_3");
		headList[2, 3] = Resources.Load<Sprite>("Rabbits/ear_round_4");
		headList[2, 4] = Resources.Load<Sprite>("Rabbits/ear_round_5");
		bodyList[0, 0] = Resources.Load<Sprite>("Rabbits/body_spade_1");
		bodyList[0, 1] = Resources.Load<Sprite>("Rabbits/body_spade_2");
		bodyList[0, 2] = Resources.Load<Sprite>("Rabbits/body_spade_3");
		bodyList[0, 3] = Resources.Load<Sprite>("Rabbits/body_spade_4");
		bodyList[0, 4] = Resources.Load<Sprite>("Rabbits/body_spade_5");
		bodyList[1, 0] = Resources.Load<Sprite>("Rabbits/body_diamond_1");
		bodyList[1, 1] = Resources.Load<Sprite>("Rabbits/body_diamond_2");
		bodyList[1, 2] = Resources.Load<Sprite>("Rabbits/body_diamond_3");
		bodyList[1, 3] = Resources.Load<Sprite>("Rabbits/body_diamond_4");
		bodyList[1, 4] = Resources.Load<Sprite>("Rabbits/body_diamond_5");
		bodyList[2, 0] = Resources.Load<Sprite>("Rabbits/body_heart_1");
		bodyList[2, 1] = Resources.Load<Sprite>("Rabbits/body_heart_2");
		bodyList[2, 2] = Resources.Load<Sprite>("Rabbits/body_heart_3");
		bodyList[2, 3] = Resources.Load<Sprite>("Rabbits/body_heart_4");
		bodyList[2, 4] = Resources.Load<Sprite>("Rabbits/body_heart_5");
		bodyList[3, 0] = Resources.Load<Sprite>("Rabbits/body_clover_1");
		bodyList[3, 1] = Resources.Load<Sprite>("Rabbits/body_clover_2");
		bodyList[3, 2] = Resources.Load<Sprite>("Rabbits/body_clover_3");
		bodyList[3, 3] = Resources.Load<Sprite>("Rabbits/body_clover_4");
		bodyList[3, 4] = Resources.Load<Sprite>("Rabbits/body_clover_5");
		tailList[0] = Resources.Load<Sprite>("Rabbits/tail_1");
		tailList[1] = Resources.Load<Sprite>("Rabbits/tail_2");
		tailList[2] = Resources.Load<Sprite>("Rabbits/tail_3");
		tailList[3] = Resources.Load<Sprite>("Rabbits/tail_4");
		tailList[4] = Resources.Load<Sprite>("Rabbits/tail_5");
		teethList[0] = Resources.Load<Sprite>("Rabbits/teeth_none");
		teethList[1] = Resources.Load<Sprite>("Rabbits/teeth_out");
		legList[0] = Resources.Load<Sprite>("Rabbits/leg_1");
		legList[1] = Resources.Load<Sprite>("Rabbits/leg_2");
		legList[2] = Resources.Load<Sprite>("Rabbits/leg_3");
		legList[3] = Resources.Load<Sprite>("Rabbits/leg_4");
		legList[4] = null;
	}

	public static void create(GameObject father, GameObject mother){
		GameObject newRabbit = (GameObject)Instantiate(scriptFarm.objRabbit, Vector2.zero, Quaternion.identity);
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
		newDummy.transform.Find("Head").GetComponent<SpriteRenderer>().sprite = original.mSprite[0];
		newDummy.transform.Find("Head").renderer.material.color = original.mColor;
		newDummy.transform.Find("Body").GetComponent<SpriteRenderer>().sprite = original.mSprite[1];
		newDummy.transform.Find("Body").renderer.material.color = original.mColor;
		newDummy.transform.Find("Tail").GetComponent<SpriteRenderer>().sprite = original.mSprite[2];
		newDummy.transform.Find("Tail").renderer.material.color = original.mColor;
		newDummy.transform.Find("Teeth").GetComponent<SpriteRenderer>().sprite = original.mSprite[3];
		newDummy.transform.Find("Teeth").renderer.material.color = original.mColor;
		newDummy.transform.Find("Leg").GetComponent<SpriteRenderer>().sprite = original.mSprite[4];
		newDummy.transform.Find("Leg").renderer.material.color = original.mColor;
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
	
	private Sprite[] mSprite = new Sprite[5];
	private int mLife = LIFE_MAX;
	private bool mIsAdult = false;
	private Color mColor = new Color(1, 1, 1);

	void Start(){
		mSprite[0] = Resources.Load<Sprite>("Rabbits/ear_none_3");
		mSprite[1] = Resources.Load<Sprite>("Rabbits/body_none_3");
		mSprite[2] = Resources.Load<Sprite>("Rabbits/tail_3");
		mSprite[3] = Resources.Load<Sprite>("Rabbits/teeh_none");
		mSprite[4] = Resources.Load<Sprite>("Rabbits/leg_3");
		Invoke("grow", 5);
		InvokeRepeating("decLife", 0.01f, 0.01f);
	}

	void Update(){
		//Vector2 tempPos;
		transform.Find("Head").GetComponent<SpriteRenderer>().sprite = mSprite[0];
		transform.Find("Head").transform.position.z = transform.Find("Head").transform.position.y / 100 - 0.001f;
		//tempPos.z = tempPos.y / 100 - 0.001f;
		transform.Find("Body").GetComponent<SpriteRenderer>().sprite = mSprite[1];
		transform.Find("Body").GetComponent<SpriteRenderer>().sprite = mSprite[1];
		transform.Find("Tail").GetComponent<SpriteRenderer>().sprite = mSprite[2];
		transform.Find("Tail").GetComponent<SpriteRenderer>().sprite = mSprite[2];
		transform.Find("Teeth").GetComponent<SpriteRenderer>().sprite = mSprite[3];
		transform.Find("Teeth").GetComponent<SpriteRenderer>().sprite = mSprite[3];
		transform.Find("Leg").GetComponent<SpriteRenderer>().sprite = mSprite[4];
		transform.Find("Leg").GetComponent<SpriteRenderer>().sprite = mSprite[4];
	}

	void grow(){
		mIsAdult = true;
		mColor = GetComponent<Gene>().list[1].Phenotype<Color>(new Color(0, 0, 0), delegate(Color arg1, Color arg2){return arg1 + arg2;}, delegate(Color arg1, int arg2){return arg1 / arg2;});
		int patternIndex = GetComponent<Gene>().list[2].Phenotype<int>(0, delegate(int arg1, int arg2){return arg1 + arg2;}, delegate(int arg1, int arg2){return arg1 / arg2;});
		int lengthIndex = GetComponent<Gene>().list[6].Phenotype<int>(0, delegate(int arg1, int arg2){return arg1 + arg2;}, delegate(int arg1, int arg2){return arg1;});
		int earIndex = GetComponent<Gene>().list[3].Phenotype<int>(0, delegate(int arg1, int arg2){return arg1 + arg2;}, delegate(int arg1, int arg2){return arg1 / arg2;});
		int teethIndex = GetComponent<Gene>().list[5].Phenotype<int>(0, delegate(int arg1, int arg2){return arg1 + arg2;}, delegate(int arg1, int arg2){return arg1 / arg2;});
		mSprite[0] = headList[earIndex, lengthIndex];
		mSprite[1] = bodyList[patternIndex, lengthIndex];
		mSprite[2] = tailList[lengthIndex];
		mSprite[3] = teethList[teethIndex];
		mSprite[4] = legList[lengthIndex];
		transform.Find("Head").renderer.material.color = mColor;
		transform.Find("Body").renderer.material.color = mColor;
		transform.Find("Tail").renderer.material.color = mColor;
		transform.Find("Leg").renderer.material.color = mColor;
	}

	void decLife(){
		if(--mLife <= 0){
			Rabbit.remove(this.gameObject);
		}
	}
}
