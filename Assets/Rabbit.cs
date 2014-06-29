using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rabbit : MonoBehaviour {
	
	
	public enum Gender{MALE, FEMALE};
	
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
	private Vector3 mMovingDir;
	private Gender mGender;
	private List<Gene> mGeneList = new List<Gene>();
	
	// Use this for initialization
	void Start () {
		mRabbitId = ++rabbitCounter;
		if (Random.Range(0, 2) == 0) {
			mGender = Gender.MALE;
			GetComponent<SpriteRenderer> ().sprite = sprMaleRabbitStand;
		}
		else{
			mGender = Gender.FEMALE;
			GetComponent<SpriteRenderer> ().sprite = sprFemaleRabbitStand;
		}
		InvokeRepeating ("RabbitJump", 0.4f, 0.4f);
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
	
	void RabbitJump(){
		if (!selected) {
			switch(mJumpCounter){
				case 4 :
					mMovingDir = new Vector3 (Random.Range (-10, 10), Random.Range (-10, 10), 0);
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
	}
	
	void OnMouseDrag(){
		if (selected) {
			Vector3 temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			transform.position = new Vector3(temp.x, temp.y, 0);
		}
	}
}
