using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class scriptFarm : MonoBehaviour {
	public enum State{START, SISTER, MAIN, MONEY, MONEY_CONFIRM, COUNT, DICT, WIN_TEMP, WIN, LOSE};

	private static readonly int MONEY_START = 10000;
	private static readonly int COST_RABBIT = 200;
	private static readonly int COST_MAINTENANCE = 200;
	private static readonly int MENU_DISTANCE = 50;
	private static readonly int TEXT_MARGIN = 50;
	private static readonly string[] winScript = new string[]{"내 토끼는 스테이지를 뚫는 \n토끼다!", "깼다.....스테이지\n...넘어간다....스테이지...", "스테이지 클리어!", "요동친다 토끼!\n불타오를만큼 교미!\n새긴다 스테이지의 비트!", "아름다운 토끼에요.", "퍼펙트하다.", "굿!", "토끼가 살아있네.", "환상의 나라 토끼랜드로~", "이열~!"};
	private static readonly string[] loseScript = new string[]{"너는 이제껏 실패한 토끼의\n수를 기억하나", "훗, 너는 하루하루 토끼 교미나\n시키는 기계일 뿐이지", "이것이 너의 한계인가...", "후...\n그는 좋은 토끼였어", "실패 다메요!!", "하아... 토끼! 토끼를 만들자", "토끼의 포스가 함께하길...", "넌 토끼에게 모욕감을 줬어", "설마...\n처음부터 다음 게임에 모든 걸\n걸었나!", "이렇게 된 거\n다음 기회를 노린다!"};
	private static readonly string[] touchScript = new string[]{"불결해...!", "스읍...떽!", "토끼에 집중해.", "나쁜손!", "라면 먹고 갈래?", "아이 참!", "고만 좀 눌러라.", "밥은 먹고 다니냐", "래빗! 뢔빗! 래뷧! 뢔뷧!", "나 뭐 달라진거 없어?"};

	public static GameObject objRabbit;
	public static GameObject objDummy;
	public static GameObject objText;
	public static List<GameObject> roomList;
	public static Diamond fieldArea;

	public int mWinCount;
	private int mScriptIndex;
	private int mMoney;
	private State mCurState;
	private Camera mCurCam;
	private GameObject mSelObj;
	private int mDictIndex;

	void Start(){
		mWinCount = 0;
		mScriptIndex = 0;
		objRabbit = Resources.Load<GameObject>("prefabRabbit");
		objDummy = Resources.Load<GameObject>("prefabDummy");
		objText = Resources.Load<GameObject>("prefabText");
		roomList = new List<GameObject>();
		//Cursor.SetCursor(Resources.Load<Texture2D>("cursor_grab1"), new Vector2(5, 5), CursorMode.Auto);
		mMoney = MONEY_START;
		mCurCam = Camera.main;
		mCurState = State.START;
		mDictIndex = 0;
		// make field area from experience
		fieldArea = new Diamond(new Vector2(0, -11), 215, 108);
		Rabbit.init();
		string targetText = "";
		for(int i = 0; i < scriptLevelSelect.levelList[scriptLevelSelect.level - 1].targetText.Length; ++i){
			targetText += scriptLevelSelect.levelList[scriptLevelSelect.level - 1].targetText[i] + "\n";
		}
		GameObject.Find("TargetText").GetComponent<TextMesh>().text = targetText;
		GameObject.Find("MessageBox").transform.Find("Text").GetComponent<TextMesh>().text = scriptLevelSelect.levelList[scriptLevelSelect.level - 1].script[mScriptIndex];
		foreach(string[][] element in scriptLevelSelect.levelList[scriptLevelSelect.level - 1].start){
			Rabbit.create(null, null);
			for(int i = 0; i < element[0].Length; ++i){
				for(int j = 0; j < element[i + 1].Length; j += 2){
					Rabbit.rabbitList[Rabbit.rabbitList.Count - 1].GetComponent<Gene>().setField(element[0][i],
																								 j / 2,
																								 element[i + 1][j],
																								 element[i + 1][j + 1]);
				}
			}
		}
		InvokeRepeating("decMoney", 10, 10);
		GameObject sisObj = (GameObject)Instantiate(Resources.Load("prefabSister" + scriptCloth.clothList[scriptCloth.clothIndex]), new Vector3(-110, -20, -1), Quaternion.identity);
		sisObj.transform.parent = GameObject.Find("Sister").transform;
		sisObj.name = "SisterBody";
		sisObj.GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime;
		Time.timeScale = 0;
	}
	void Update(){
		roomList.RemoveAll(x => x == null);
		if(mCurState != State.LOSE && (Rabbit.rabbitList.Count == 0 || mMoney < 0)){
			mCurState = State.LOSE;
			GameObject.Find("Sister").transform.position = new Vector3(-110, -20, -3);
			GameObject.Find("Sister").transform.Find("SisterBody").GetComponent<Animator>().enabled = true;
			GameObject.Find("Sister").GetComponent<BoxCollider2D>().center = new Vector2(0, 0);
			GameObject.Find("Sister").GetComponent<BoxCollider2D>().size = new Vector2(180.75f, 298.75f);
			GameObject.Find("MessageBox").GetComponent<SpriteRenderer>().enabled = true;
			GameObject.Find("MessageBox").transform.Find("Text").GetComponent<MeshRenderer>().enabled = true;
			GameObject.Find("MessageBackground").GetComponent<SpriteRenderer>().enabled = true;
			GameObject.Find("MessageBox").transform.Find("Text").GetComponent<TextMesh>().text = "실패!\n" + loseScript[Random.Range(0, loseScript.Length)];
		}
		// for inputs
		if(Input.GetMouseButton(0)){
			//Cursor.SetCursor(Resources.Load<Texture2D>("cursor_grab2"), new Vector2(0, 5), CursorMode.Auto);
		}
		else{
			//Cursor.SetCursor(Resources.Load<Texture2D>("cursor_grab1"), new Vector2(5, 5), CursorMode.Auto);
		}
		if(Input.GetMouseButtonDown(0)){
			Vector2 ray = mCurCam.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);
			if(hit.collider != null){
				mSelObj = hit.collider.gameObject;
				switch(mCurState){
					case State.MAIN :
						switch(mSelObj.tag){
							case "Rabbit" :
								if(Rabbit.rabbitList.Contains(mSelObj)){
									mSelObj.GetComponent<Draggable>().select = true;
									mSelObj.transform.Find("rabbitBody").GetComponent<Animator>().SetBool("Catch", true);
								}
								break;
						}
						break;
					case State.MONEY :
						break;
					case State.COUNT :
						break;
				}
			}
			else if(mCurState != State.MONEY_CONFIRM && !(mSelObj != null && mCurState == State.MONEY && mSelObj.tag == "Dummy")){
				mSelObj = null;
			}
		}
		if(Input.GetMouseButtonUp(0)){
			Vector2 ray = mCurCam.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);
			GameObject mUpObj = (hit.collider == null) ? null : hit.collider.gameObject;
			if(mUpObj != null){
				switch(mCurState){
					case State.START :
						if(mUpObj.tag == "Sister"){
							if(++mScriptIndex >= scriptLevelSelect.levelList[scriptLevelSelect.level - 1].script.Length){
								GameObject.Find("Sister").transform.position = new Vector2(-10, -32);
								GameObject.Find("Sister").transform.Find("SisterBody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("sister_normal_down");
								GameObject.Find("Sister").transform.Find("SisterBody").GetComponent<Animator>().enabled = false;
								GameObject.Find("Sister").GetComponent<BoxCollider2D>().center = new Vector2(-102, -90);
								GameObject.Find("Sister").GetComponent<BoxCollider2D>().size = new Vector2(70, 80);
								GameObject.Find("MessageBox").GetComponent<SpriteRenderer>().enabled = false;
								GameObject.Find("MessageBox").transform.Find("Text").GetComponent<MeshRenderer>().enabled = false;
								GameObject.Find("MessageBackground").GetComponent<SpriteRenderer>().enabled = false;
								//GameObject.Find("MessageBox").transform.Find("Text").GetComponent<TextMesh>().text = "!!!!!";
								mCurState = State.MAIN;
								foreach(GameObject element in Rabbit.rabbitList){
									checkCondition(element);
								}
								Time.timeScale = 1;
							}
							else{
								GameObject.Find("MessageBox").transform.Find("Text").GetComponent<TextMesh>().text = scriptLevelSelect.levelList[scriptLevelSelect.level - 1].script[mScriptIndex];
							}
						}
							break;
					case State.SISTER :
						if(mUpObj.tag == "Sister"){
							GameObject.Find("Sister").transform.position = new Vector2(-10, -32);
							GameObject.Find("Sister").transform.Find("SisterBody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("sister_normal_down");
							GameObject.Find("Sister").transform.Find("SisterBody").GetComponent<Animator>().enabled = false;
							GameObject.Find("Sister").GetComponent<BoxCollider2D>().center = new Vector2(-102, -90);
							GameObject.Find("Sister").GetComponent<BoxCollider2D>().size = new Vector2(70, 80);
							GameObject.Find("MessageBox").GetComponent<SpriteRenderer>().enabled = false;
							GameObject.Find("MessageBox").transform.Find("Text").GetComponent<MeshRenderer>().enabled = false;
							GameObject.Find("MessageBackground").GetComponent<SpriteRenderer>().enabled = false;
							mCurState = State.MAIN;
							Time.timeScale = 1;
						}
						break;
					case State.MAIN :
						if(mSelObj != null && mSelObj.tag == "Rabbit"){
							mSelObj.GetComponent<Draggable>().select = false;
							bool isHouse = false;
							RaycastHit2D[] hitArr = Physics2D.RaycastAll(ray, Vector2.zero);
							foreach(RaycastHit2D element in hitArr){
								if(element.collider.gameObject.name == "House"){
									isHouse = true;
									break;
								}
							}
							if(isHouse){
								if(roomList.Count < 2 && mSelObj.GetComponent<Rabbit>().isAdult){
									roomList.Add(mSelObj);
									mSelObj.transform.position = new Vector2(0, 1000);
								}
								else{
									mSelObj.transform.position = new Vector2(30, 30);
								}
							}
							else if(!fieldArea.Contains(mSelObj.transform.position)){
								mSelObj.GetComponent<Rabbit>().remove();
							}
							mSelObj.transform.Find("rabbitBody").GetComponent<Animator>().SetBool("Catch", false);
						}
						switch(mUpObj.tag){
							case "Sister" :
								mCurState = State.SISTER;
								GameObject.Find("Sister").transform.position = new Vector3(-110, -20, -3);
								GameObject.Find("Sister").transform.Find("SisterBody").GetComponent<Animator>().enabled = true;
								GameObject.Find("Sister").GetComponent<BoxCollider2D>().center = new Vector2(0, 0);
								GameObject.Find("Sister").GetComponent<BoxCollider2D>().size = new Vector2(180.75f, 298.75f);
								GameObject.Find("MessageBox").GetComponent<SpriteRenderer>().enabled = true;
								GameObject.Find("MessageBox").transform.Find("Text").GetComponent<MeshRenderer>().enabled = true;
								GameObject.Find("MessageBackground").GetComponent<SpriteRenderer>().enabled = true;
								GameObject.Find("MessageBox").transform.Find("Text").GetComponent<TextMesh>().text = touchScript[Random.Range(0, touchScript.Length)];
								Time.timeScale = 0;
								break;
							case "GUI" :
								switch(mUpObj.name){
									case "MoneyButton" :
										mCurState = State.MONEY;
										foreach(GameObject element in Rabbit.rabbitList){
											Rabbit.createDummy(element.GetComponent<Rabbit>());
										}
										for(int i = 0; i < Rabbit.dummyList.Count; ++i){
											Rabbit.dummyList[i].transform.position = new Vector2(520, 120 - i * MENU_DISTANCE );
											Rabbit.textList[i].transform.position = new Vector2(520 + TEXT_MARGIN, 130 - i * MENU_DISTANCE);
										}
										Time.timeScale = 0;
										mCurCam = GameObject.Find("List Camera").GetComponent<Camera>();
										mCurCam.gameObject.transform.position = new Vector3(700, 0, -10);
										mCurCam.enabled = true;
										break;
									case "CountButton" :
										foreach(GameObject element in Rabbit.rabbitList){
											Rabbit.createDummy(element.GetComponent<Rabbit>());
										}
										for(int i = 0; i < Rabbit.dummyList.Count; ++i){
											Rabbit.dummyList[i].transform.position = new Vector2(1220, 120 - i * MENU_DISTANCE );
											Rabbit.textList[i].transform.position = new Vector2(1220 + TEXT_MARGIN, 130 - i * MENU_DISTANCE);
										}
										mCurState = State.COUNT;
										mCurCam = GameObject.Find("List Camera").GetComponent<Camera>();
										mCurCam.gameObject.transform.position = new Vector3(1400, 0, -10);
										mCurCam.enabled = true;
										Time.timeScale = 0;
										break;
									case "Dictionary" :
										mCurState = State.DICT;
										mCurCam = GameObject.Find("List Camera").GetComponent<Camera>();
										mCurCam.gameObject.transform.position = new Vector3(2100, 0, -10);
										mCurCam.enabled = true;
										Time.timeScale = 0;
										break;
								}
								break;
						}
						break;
					case State.MONEY :
						if(mSelObj != null && mSelObj.tag == "Dummy" && mUpObj.tag == "Dummy"){
							mCurState = State.MONEY_CONFIRM;
							break;
						}
						switch(mUpObj.tag){
							case "GUI" :
								switch(mUpObj.name){
									case "ExitButton" :
										mCurState = State.MAIN;
										mCurCam.enabled = false;
										mCurCam = Camera.main;
										Time.timeScale = 1;
										Rabbit.clearDummy();
										break;
								}
								break;
							case "Dummy" :
								break;
						}
						break;
					case State.COUNT :
						switch(mUpObj.tag){
							case "GUI" :
								switch(mUpObj.name){
									case "ExitButton" :
										mCurState = State.MAIN;
										mCurCam.enabled = false;
										mCurCam = Camera.main;
										Time.timeScale = 1;
										Rabbit.clearDummy();
										break;
								}
								break;
							case "Dummy" :
								mCurState = State.MAIN;
								GameObject.Find("Light").GetComponent<SpriteRenderer>().enabled = true;
								Vector3 newPos = Rabbit.rabbitList[Rabbit.dummyList.IndexOf(mUpObj)].transform.position;
								newPos.z = 0.2f;
								GameObject.Find("Light").transform.position = newPos;
								Invoke("disableLight", 3);
								mCurCam.enabled = false;
								mCurCam = Camera.main;
								Time.timeScale = 1;
								Rabbit.clearDummy();
								break;
						}
						break;
					case State.DICT :
						switch(mUpObj.name){
							case "ExitButton" :
								mCurState = State.MAIN;
								mCurCam.enabled = false;
								mCurCam = Camera.main;
								Time.timeScale = 1;
								break;
							case "DictLeft" :
								mDictIndex = (8 + mDictIndex - 1) % 8;
								GameObject.Find("DictImage").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("dict" + (mDictIndex + 1).ToString());
								break;
							case "DictRight" :
								mDictIndex = (mDictIndex + 1) % 8;
								GameObject.Find("DictImage").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("dict" + (mDictIndex + 1).ToString());
								break;
						}
						break;
					case State.WIN :
						if(mUpObj.tag == "Sister"){
							scriptLevelSelect.clearList[scriptLevelSelect.level - 1] = true;
							Application.LoadLevel("sceneLevelSelect");
						}
						break;
					case State.LOSE :
						if(mUpObj.tag == "Sister"){
							Application.LoadLevel("sceneLevelSelect");
						}
						break;
				}
				if(mCurState != State.MONEY_CONFIRM && !(mCurState == State.MONEY_CONFIRM && mUpObj.tag == "Dummy")){
					mSelObj = null;
				}
			}
		}
		// updating texts
		GameObject.Find("MoneyButton").transform.
				   Find("Text").GetComponent<TextMesh>().text = mMoney.ToString() + "(-"
				   											  + (Rabbit.rabbitList.Count * COST_MAINTENANCE).ToString() + ")";
		GameObject.Find("CountButton").transform.
				   Find("Text").GetComponent<TextMesh>().text = Rabbit.rabbitList.Count.ToString() + "마리";
	}

	void OnGUI(){
		if(mCurState == State.MONEY_CONFIRM){
			if(GUI.Button(new Rect(0, Screen.height * 0.5f, Screen.width * 0.1f, Screen.height * 0.1f), "SELL!!")){
				mMoney += COST_RABBIT;
				int index = Rabbit.dummyList.IndexOf(mSelObj);
				Destroy(Rabbit.rabbitList[index]);
				Destroy(Rabbit.dummyList[index]);
				Destroy(Rabbit.textList[index]);
				Rabbit.rabbitList.RemoveAt(index);
				Rabbit.dummyList.RemoveAt(index);
				Rabbit.textList.RemoveAt(index);
				for(int i = index; i < Rabbit.dummyList.Count; ++i){
					Vector2 cusPos = Rabbit.dummyList[i].transform.position;
					Rabbit.dummyList[i].transform.position = new Vector2(cusPos.x, cusPos.y + MENU_DISTANCE);
					Rabbit.textList[i].transform.position = new Vector2(cusPos.x + TEXT_MARGIN, cusPos.y + MENU_DISTANCE);
				}
				mCurState = State.MONEY;
			}
			if(GUI.Button(new Rect(0, Screen.height * 0.6f, Screen.width * 0.1f, Screen.height * 0.1f), "NOOO!!")){
				mCurState = State.MONEY;
			}
		}
		if(roomList.Count >= 2){
			if(GUI.Button(new Rect(Screen.width * 0.9f, Screen.height * 0.6f, Screen.width * 0.1f, Screen.height * 0.1f), "!WOW!")){
				foreach(GameObject element in roomList){
					element.transform.position = new Vector2(Random.Range(-100, 100), Random.Range(-50, 50));
				}
				Rabbit.create(roomList[1], roomList[0]);
				roomList.Clear();
			}
		}
	}

	public bool checkCondition(GameObject input){
		if(Gene.phenoEqual(input.GetComponent<Gene>(), scriptLevelSelect.geneList, scriptLevelSelect.levelList[scriptLevelSelect.level - 1].condition)){
			++mWinCount;
		}
		if(mWinCount >= scriptLevelSelect.levelList[scriptLevelSelect.level - 1].count){
			mCurState = State.WIN_TEMP;
			Invoke("winGame", 2);
			return true;
		}
		return false;
	}

	void winGame(){
		GameObject.Find("Sister").transform.position = new Vector3(-110, -20, -3);
		GameObject.Find("Sister").transform.Find("SisterBody").GetComponent<Animator>().enabled = true;
		GameObject.Find("Sister").GetComponent<BoxCollider2D>().center = new Vector2(0, 0);
		GameObject.Find("Sister").GetComponent<BoxCollider2D>().size = new Vector2(180.75f, 298.75f);
		GameObject.Find("MessageBox").GetComponent<SpriteRenderer>().enabled = true;
		GameObject.Find("MessageBox").transform.Find("Text").GetComponent<MeshRenderer>().enabled = true;
		GameObject.Find("MessageBackground").GetComponent<SpriteRenderer>().enabled = true;
		GameObject.Find("MessageBox").transform.Find("Text").GetComponent<TextMesh>().text = "성공!\n" + winScript[Random.Range(0, winScript.Length)];
		mCurState = State.WIN;
	}

	void decMoney(){
		mMoney -= Rabbit.rabbitList.Count * COST_MAINTENANCE;
	}

	void disableLight(){
		GameObject.Find("Light").GetComponent<SpriteRenderer>().enabled = false;
	}

// for field area test
/*
	void OnDrawGizmos(){
		Gizmos.DrawLine(fieldArea.top(), fieldArea.right());
		Gizmos.DrawLine(fieldArea.right(), fieldArea.bottom());
		Gizmos.DrawLine(fieldArea.bottom(), fieldArea.left());
		Gizmos.DrawLine(fieldArea.left(), fieldArea.top());
		Gizmos.DrawLine(fieldArea.left(), fieldArea.right());
		Gizmos.DrawLine(fieldArea.top(), fieldArea.bottom());
	}
*/
}