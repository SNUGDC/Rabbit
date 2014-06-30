using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(BoxCollider2D))]

public class Rabbit : MonoBehaviour {
	
	
	public enum Gender{MALE, FEMALE};
	
	public static readonly ulong startHunger = 30;
	public static readonly ulong maxHunger = 50;
	public static ulong rabbitCounter = 0;
	public static Sprite sprMaleRabbitStand = Resources.LoadAll<Sprite>("txtrRabbit")[5];
	public static Sprite sprFemaleRabbitStand = Resources.LoadAll<Sprite>("txtrRabbit")[1];
	public static Sprite sprRabbitHold = Resources.LoadAll<Sprite> ("txtrRabbit")[2];
	public static Sprite sprRabbitJump = Resources.LoadAll<Sprite>("txtrRabbit")[3];
	public static Sprite sprRabbitLand = Resources.LoadAll<Sprite>("txtrRabbit")[4];
	
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
	public ulong rabbitId{
		get{
			return mRabbitId;
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
	
	private bool mSelected = false;
	private bool mSelectBuffer = false;
	private int mJumpCounter = 0;
	private ulong mHunger;
	private ulong mRabbitId;
	private float mJumpRate = 0.4f;
	private Vector3 mMovingDir;
	private Gender mGender;
	private List<Gene> mGeneList = new List<Gene>();
	
	// Use this for initialization
	IEnumerator Start () {
		//renderer.material.shader = Shader.Find("Diffuse");
		mRabbitId = ++rabbitCounter;
		if (Random.Range(0, 2) == 0) {
			mGender = Gender.MALE;
			GetComponent<SpriteRenderer> ().sprite = sprMaleRabbitStand;
		}
		else{
			mGender = Gender.FEMALE;
			GetComponent<SpriteRenderer> ().sprite = sprFemaleRabbitStand;
		}
		mHunger = 0;
		yield return StartCoroutine("RabbitJump");
		
	}
	
	// Update is called once per frame
	void Update () {
		if (mSelected != mSelectBuffer) {
			if(mSelected){
				GetComponent<SpriteRenderer>().sprite = sprRabbitHold;
			}
			else{
				if (mGender == Gender.MALE) {
					GetComponent<SpriteRenderer> ().sprite = sprMaleRabbitStand;
				}
				else{
					GetComponent<SpriteRenderer> ().sprite = sprFemaleRabbitStand;
				}
			}
			mSelectBuffer = selected;
		}
	}
	
	IEnumerator RabbitJump(){
		while(mHunger <= maxHunger){
			if(mHunger > startHunger){
				renderer.material.color = new Color(0.192f, 0.376f, 0.2f);
				mJumpRate = 0.1f;
			}			
			if (!selected) {
				switch(mJumpCounter){
				case 4 :
					Carrot nearCarrot = FarmFunc.findCarrot(transform.position.x, transform.position.y);
					if(nearCarrot && mHunger > startHunger){
						mMovingDir = new Vector3 (nearCarrot.gameObject.transform.position.x - transform.position.x,
						                          nearCarrot.gameObject.transform.position.y - transform.position.y);
						mMovingDir.Normalize();
						mMovingDir = mMovingDir * 10;
					}
					else{
						mMovingDir = new Vector3 (Random.Range (-10, 10), Random.Range (-10, 10), 0);
					}
					Vector3 maxLeftBottom = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
					Vector3 maxRightTop = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.9f, Screen.height * 0.9f, 0));
					if(mMovingDir.x + transform.position.x < maxLeftBottom.x || mMovingDir.x + transform.position.x > maxRightTop.x){
						mMovingDir.x *= -1;
					}
					if(mMovingDir.y + transform.position.y < maxLeftBottom.y || mMovingDir.y + transform.position.y > maxRightTop.y){
						mMovingDir.y *= -1;
					}
					transform.position = new Vector3(transform.position.x + mMovingDir.x / 2,
					                                 transform.position.y +  mMovingDir.y / 2,
					                                 0);
					GetComponent<SpriteRenderer> ().sprite = sprRabbitJump;
					break;
				case 5 :
					transform.position = new Vector3(transform.position.x + mMovingDir.x / 2,
					                                 transform.position.y +  mMovingDir.y / 2,
					                                 0);
					GetComponent<SpriteRenderer> ().sprite = sprRabbitLand;
					break;
				default :
					if (mGender == Gender.MALE) {
						GetComponent<SpriteRenderer> ().sprite = sprMaleRabbitStand;
					}
					else {
						GetComponent<SpriteRenderer> ().sprite = sprFemaleRabbitStand;
					}
					break;
				}
				mJumpCounter = (mJumpCounter + 1) % 6;
			}
			else{
				mJumpCounter = 0;
			}
			yield return new WaitForSeconds(mJumpRate);
		}
		scriptFarm.rabbitList.Remove(this);
		Destroy(gameObject);
		yield break;
	}
	
	void OnMouseDrag(){
		if (selected) {
			Vector3 temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			transform.position = new Vector3(temp.x, temp.y, 0);
		}
	}
	
	void OnTriggerStay2D(Collider2D collider){
		if(collider.gameObject.tag == "carrot" && !mSelected && mHunger > startHunger){
			scriptFarm.carrotList.Remove((collider.gameObject.GetComponent<Carrot>()));
			Destroy(collider.gameObject);
			renderer.material.color = new Color(1.0f, 1.0f, 1.0f);
			mHunger = 0;
			mJumpRate = 0.4f;
		}
	}
}
