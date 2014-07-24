using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rabbit : MonoBehaviour {
	
	/*-----public data types-----*/
	public enum Gender {MALE, FEMALE};
	
	/*-----public static variables-----*/
	public static List<Rabbit> rabbitList = new List<Rabbit>();
	public static readonly ulong startHunger = 200; // starting point of hungry state
	public static readonly ulong maxHunger = 1000; // hunger for starving to death
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
	public bool selected{
		get{
			return mSelected;
		}
		set{
			mSelected = value;
		}
	}
	public ulong hunger{
		get{
			return mHunger;
		}
		set{
			mHunger = value;
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
	public List<Gene> geneList{
		get{
			return mGeneList;
		}
	}
	
	/*-----private member variables-----*/
	private bool mIsAdult = false;
	private bool mSelected = false;
	private uint mFrameCounter = 0; // for Jump Loop
	private ulong mHunger = 0;
	private ulong mLife;
	private int mId;
	private uint mJumpPeriod = 15; // for Jump Loop
	private Vector3 mMovingDir; // for Jump Loop
	private Gender mGender;
	private Color mColor = new Color(1, 1, 1);
	private List<Gene> mGeneList = new List<Gene>();
	
	/*-----public static functions-----*/
	public static void init(){
		sprMaleRabbitStand = Resources.LoadAll<Sprite>("txtrRabbit")[5];
		sprFemaleRabbitStand = Resources.LoadAll<Sprite>("txtrRabbit")[1];
		sprRabbitHold = Resources.LoadAll<Sprite> ("txtrRabbit")[2];
		sprRabbitJump = Resources.LoadAll<Sprite>("txtrRabbit")[3];
		sprRabbitLand = Resources.LoadAll<Sprite>("txtrRabbit")[4];
		sprSmallRabbit = Resources.LoadAll<Sprite>("txtrRabbit")[8];
		sprSmallJump = Resources.LoadAll<Sprite>("txtrRabbit")[6];
		sprSmallLand = Resources.LoadAll<Sprite>("txtrRabbit")[7];
	}

	public static void create(Rabbit father, Rabbit mother){
		// select position of new rabbit
		Vector3 worldLeftBottom = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
		Vector3 worldRightTop = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.9f, Screen.height * 0.9f, 0));
		Vector3 tempPosition = new Vector3(Random.Range (worldLeftBottom.x, worldRightTop.x),
		                                   Random.Range (worldLeftBottom.y, worldRightTop.y), 0);
		GameObject newRabbit = (GameObject)Instantiate(scriptFarm.objRabbit, tempPosition, Quaternion.identity);
		// assign genes to newRabbit
		if(father == null || mother == null){
			foreach(JsonGene element in Gene.jsonGeneList){
				newRabbit.GetComponent<Rabbit>().geneList.Add (new Gene(element));
			}
		}
		else{
			for(int i = 0; i < father.geneList.Count; ++i){
				newRabbit.GetComponent<Rabbit>().geneList.Add(new Gene(father.geneList[i], mother.geneList[i]));
			}
		}
		// add newRabbit to rabbitList
		rabbitList.Add(newRabbit.GetComponent<Rabbit>());
	}

	public static void delete(Rabbit target){
		rabbitList.Remove(target);
		DestroyImmediate(target.gameObject);
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
		if(++mHunger > maxHunger){
			// rabbit starved to death
			Rabbit.delete(this);
			return;
		}
		if(mHunger > startHunger){
			// rabbit is hungry
			renderer.material.color = new Color(0.192f, 0.376f, 0.2f);
			mJumpPeriod = 5;
		}
		// Rabbit Jump Loop
		if (!mSelected) {
			if(mFrameCounter == 0){
				if(!mIsAdult){
					GetComponent<SpriteRenderer>().sprite = sprSmallRabbit;
				}
				else{
					GetComponent<SpriteRenderer>().sprite = (mGender == Gender.MALE) ? sprMaleRabbitStand : sprFemaleRabbitStand;
				}
			}
			else if(mFrameCounter == mJumpPeriod * 4){
				// find Carrot
				Carrot nearCarrot = FarmFunc.findNearCarrot(transform.position.x, transform.position.y);
				// set new MovingDir
				if(nearCarrot && mHunger > startHunger){
					mMovingDir = new Vector3 (nearCarrot.gameObject.transform.position.x - transform.position.x,
					                          nearCarrot.gameObject.transform.position.y - transform.position.y);
					mMovingDir.Normalize();
					mMovingDir *= 10;
				}
				else{
					mMovingDir = new Vector3 (Random.Range (-10, 10), Random.Range (-10, 10), 0);
				}
				// boundary check
				Vector3 maxLeftBottom = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
				Vector3 maxRightTop = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.9f, Screen.height * 0.9f, 0));
				if(mMovingDir.x + transform.position.x < maxLeftBottom.x || mMovingDir.x + transform.position.x > maxRightTop.x){
					mMovingDir.x *= -1;
				}
				if(mMovingDir.y + transform.position.y < maxLeftBottom.y || mMovingDir.y + transform.position.y > maxRightTop.y){
					mMovingDir.y *= -1;
				}
				// move position
				transform.position += new Vector3(mMovingDir.x / 2, mMovingDir.y / 2, 0);
				// change sprite
				GetComponent<SpriteRenderer>().sprite = (mIsAdult) ? sprRabbitJump : sprSmallJump;
			}
			else if(mFrameCounter == mJumpPeriod * 5){
				transform.position += new Vector3(mMovingDir.x / 2, mMovingDir.y / 2, 0);
				GetComponent<SpriteRenderer>().sprite = (mIsAdult) ? sprRabbitLand : sprSmallLand;
			}
			mFrameCounter = (mFrameCounter + 1) % (mJumpPeriod * 6);
		}
		else{
			GetComponent<SpriteRenderer>().sprite = sprRabbitHold;
			mFrameCounter = 0;
		}
	}
	
	void OnMouseDrag(){
		if (selected) {
			Vector2 temp = Input.mousePosition;
			// limit draggable area
			temp.x = Mathf.Max(temp.x, 0);
			temp.x = Mathf.Min(temp.x, scriptFarm.sWidth * 0.9f);
			temp.y = Mathf.Max(temp.y, 0);
			temp.y = Mathf.Min(temp.y, scriptFarm.sHeight * 0.9f);
			transform.position = (Vector2)Camera.main.ScreenToWorldPoint(temp);
		}
	}

	void grow(){
		mIsAdult = true;
		mColor = mGeneList[1].Phenotype<Color>(new Color(0, 0, 0), delegate(Color arg1, Color arg2){return arg1 + arg2;}, delegate(Color arg1, int arg2){return arg1 / arg2;});
		renderer.material.color = mColor;
	}
}
