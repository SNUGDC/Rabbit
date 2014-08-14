using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rabbit : MonoBehaviour {
	
	/*-----readonly variables-----*/
	public static readonly uint maxLife = 2000;

	/*-----public data types-----*/
	public enum Gender {MALE, FEMALE};
	
	/*-----public static variables-----*/
	public static List<Rabbit> rabbitList = new List<Rabbit>();
	public static List<GameObject> dummyList = new List<GameObject>();
	public static Sprite[ , ] headList = new Sprite[3, 5];
	public static Sprite[ , ] bodyList = new Sprite[4, 5];
	public static Sprite[] tailList = new Sprite[5];
	public static Sprite[] teethList = new Sprite[2];
	public static Sprite[] legList = new Sprite[5];
	
	/*-----public member variables-----*/
	public bool isAdult{
		get{
			return mIsAdult;
		}
	}

	public bool inRoom{
		get{
			return mInRoom;
		}
		set{
			mInRoom = value;
		}
	}
	public ulong life{
		get{
			return mLife;
		}
		set{
			mLife = value;
		}
	}
	public int id{
		get{
			return mId;
		}
	}
	public Gender gender{
		get{
			return mGender;
		}
	}
	
	/*-----private member variables-----*/
	private bool mIsAdult = false;
	private bool mInRoom = false;
	private ulong mLife = maxLife;
	private int mId;
	private Gender mGender;
	private Color mColor = new Color(1, 1, 1);
	public Sprite[] mSprite = new Sprite[5];
	
	/*-----public static functions-----*/
	public static void init(){
		headList[0, 0] = Resources.Load<Sprite>("ear_none_1");
		headList[0, 1] = Resources.Load<Sprite>("ear_none_2");
		headList[0, 2] = Resources.Load<Sprite>("ear_none_3");
		headList[0, 3] = Resources.Load<Sprite>("ear_none_4");
		headList[0, 4] = Resources.Load<Sprite>("ear_none_5");
		headList[1, 0] = Resources.Load<Sprite>("ear_down_1");
		headList[1, 1] = Resources.Load<Sprite>("ear_down_2");
		headList[1, 2] = Resources.Load<Sprite>("ear_down_3");
		headList[1, 3] = Resources.Load<Sprite>("ear_down_4");
		headList[1, 4] = Resources.Load<Sprite>("ear_down_5");
		headList[2, 0] = Resources.Load<Sprite>("ear_round_1");
		headList[2, 1] = Resources.Load<Sprite>("ear_round_2");
		headList[2, 2] = Resources.Load<Sprite>("ear_round_3");
		headList[2, 3] = Resources.Load<Sprite>("ear_round_4");
		headList[2, 4] = Resources.Load<Sprite>("ear_round_5");
		bodyList[0, 0] = Resources.Load<Sprite>("body_spade_1");
		bodyList[0, 1] = Resources.Load<Sprite>("body_spade_2");
		bodyList[0, 2] = Resources.Load<Sprite>("body_spade_3");
		bodyList[0, 3] = Resources.Load<Sprite>("body_spade_4");
		bodyList[0, 4] = Resources.Load<Sprite>("body_spade_5");
		bodyList[1, 0] = Resources.Load<Sprite>("body_diamond_1");
		bodyList[1, 1] = Resources.Load<Sprite>("body_diamond_2");
		bodyList[1, 2] = Resources.Load<Sprite>("body_diamond_3");
		bodyList[1, 3] = Resources.Load<Sprite>("body_diamond_4");
		bodyList[1, 4] = Resources.Load<Sprite>("body_diamond_5");
		bodyList[2, 0] = Resources.Load<Sprite>("body_heart_1");
		bodyList[2, 1] = Resources.Load<Sprite>("body_heart_2");
		bodyList[2, 2] = Resources.Load<Sprite>("body_heart_3");
		bodyList[2, 3] = Resources.Load<Sprite>("body_heart_4");
		bodyList[2, 4] = Resources.Load<Sprite>("body_heart_5");
		bodyList[3, 0] = Resources.Load<Sprite>("body_clover_1");
		bodyList[3, 1] = Resources.Load<Sprite>("body_clover_2");
		bodyList[3, 2] = Resources.Load<Sprite>("body_clover_3");
		bodyList[3, 3] = Resources.Load<Sprite>("body_clover_4");
		bodyList[3, 4] = Resources.Load<Sprite>("body_clover_5");
		tailList[0] = Resources.Load<Sprite>("tail_1");
		tailList[1] = Resources.Load<Sprite>("tail_2");
		tailList[2] = Resources.Load<Sprite>("tail_3");
		tailList[3] = Resources.Load<Sprite>("tail_4");
		tailList[4] = Resources.Load<Sprite>("tail_5");
		teethList[0] = Resources.Load<Sprite>("teeth_none");
		teethList[1] = Resources.Load<Sprite>("teeth_out");
		legList[0] = Resources.Load<Sprite>("leg_1");
		legList[1] = Resources.Load<Sprite>("leg_2");
		legList[2] = Resources.Load<Sprite>("leg_3");
		legList[3] = Resources.Load<Sprite>("leg_4");
		legList[4] = null;
	}

	public static void create(GameObject father, GameObject mother){
		// select position of new rabbit
		Vector2 tempPosition = (Vector2)Camera.main.ScreenToWorldPoint(new Vector2(Screen.width * 0.5f, Screen.height * 0.5f));
		GameObject newRabbit = (GameObject)Instantiate(scriptFarm.objRabbit, tempPosition, Quaternion.identity);
		// assign genes to newRabbit
		Gene fatherGene = (father == null) ? null : father.GetComponent<Gene>();
		Gene motherGene = (mother == null) ? null : mother.GetComponent<Gene>();
		newRabbit.GetComponent<Gene>().create(fatherGene, motherGene);
		// add newRabbit to rabbitList
		rabbitList.Add(newRabbit.GetComponent<Rabbit>());
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
		dummyList.Add(newDummy);
	}
	
	/*-----public member functions-----*/
	// Use this for initialization
	void Start() {
		mId = rabbitList.Count;
		mGender = (Random.Range(0, 2) == 0) ? Gender.MALE : Gender.FEMALE;
		mSprite[0] = Resources.Load<Sprite>("ear_none_3");
		mSprite[1] = Resources.Load<Sprite>("body_none_3");
		mSprite[2] = Resources.Load<Sprite>("tail_3");
		mSprite[3] = Resources.Load<Sprite>("teeh_none");
		mSprite[4] = Resources.Load<Sprite>("leg_3");
		transform.Find("Head").GetComponent<SpriteRenderer>().sprite = mSprite[0];
		transform.Find("Body").GetComponent<SpriteRenderer>().sprite = mSprite[1];
		transform.Find("Tail").GetComponent<SpriteRenderer>().sprite = mSprite[2];
		transform.Find("Teeth").GetComponent<SpriteRenderer>().sprite = mSprite[3];
		transform.Find("Leg").GetComponent<SpriteRenderer>().sprite = mSprite[4];
		Invoke("grow", 5);
	}
	
	// Update is called once per frame
	void Update() {
		// Rabbit Jump Loop
		if(--mLife <= 0){
			rabbitList.Remove(this);
			DestroyImmediate(this.gameObject);
			return;
		}
		transform.Find("Head").GetComponent<SpriteRenderer>().sprite = mSprite[0];
		transform.Find("Body").GetComponent<SpriteRenderer>().sprite = mSprite[1];
		transform.Find("Tail").GetComponent<SpriteRenderer>().sprite = mSprite[2];
		transform.Find("Teeth").GetComponent<SpriteRenderer>().sprite = mSprite[3];
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
		transform.Find("Teeth").renderer.material.color = mColor;
		transform.Find("Leg").renderer.material.color = mColor;
	}
}
