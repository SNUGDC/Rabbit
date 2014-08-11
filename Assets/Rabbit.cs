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
	public static Sprite sprMaleRabbitStand;
	public static Sprite sprFemaleRabbitStand;
	public static Sprite sprRabbitHold;
	public static Sprite sprRabbitJump;
	public static Sprite sprRabbitLand;
	public static Sprite sprSmallRabbit;
	public static Sprite sprSmallJump;
	public static Sprite sprSmallLand;
	
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
	
	/*-----public static functions-----*/
	public static void init(){
		sprMaleRabbitStand = Resources.LoadAll<Sprite>("txtrRabbit")[5];
		sprFemaleRabbitStand = Resources.LoadAll<Sprite>("txtrRabbit")[1];
		sprRabbitHold = Resources.LoadAll<Sprite> ("txtrRabbit")[2];
		sprSmallRabbit = Resources.LoadAll<Sprite>("txtrRabbit")[8];
	}

	public static void create(GameObject father, GameObject mother){
		// select position of new rabbit
		Vector3 worldLeftBottom = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
		Vector3 worldRightTop = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.9f, Screen.height * 0.9f, 0));
		Vector3 tempPosition = new Vector3(Random.Range (worldLeftBottom.x, worldRightTop.x),
		                                   Random.Range (worldLeftBottom.y, worldRightTop.y), 0);
		GameObject newRabbit = (GameObject)Instantiate(scriptFarm.objRabbit, tempPosition, Quaternion.identity);
		// assign genes to newRabbit
		Gene fatherGene = (father == null) ? null : father.GetComponent<Gene>();
		Gene motherGene = (mother == null) ? null : mother.GetComponent<Gene>();
		newRabbit.GetComponent<Gene>().create(fatherGene, motherGene);
		// add newRabbit to rabbitList
		rabbitList.Add(newRabbit.GetComponent<Rabbit>());
	}
	
	/*-----public member functions-----*/
	// Use this for initialization
	void Start() {
		mId = rabbitList.Count;
		mGender = (Random.Range(0, 2) == 0) ? Gender.MALE : Gender.FEMALE;
		GetComponent<SpriteRenderer>().sprite = sprSmallRabbit;
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
		if(GetComponent<Selectable>().selected || mInRoom){
			GetComponent<SpriteRenderer>().sprite = sprRabbitHold;
		}
		else{
			if(mIsAdult){
				GetComponent<SpriteRenderer>().sprite = (mGender == Gender.MALE) ? sprMaleRabbitStand : sprFemaleRabbitStand;
			}
			else{
				GetComponent<SpriteRenderer>().sprite = sprSmallRabbit;
			}
		}
	}

	void grow(){
		mIsAdult = true;
		mColor = GetComponent<Gene>().list[1].Phenotype<Color>(new Color(0, 0, 0), delegate(Color arg1, Color arg2){return arg1 + arg2;}, delegate(Color arg1, int arg2){return arg1 / arg2;});
		renderer.material.color = mColor;
	}
}
