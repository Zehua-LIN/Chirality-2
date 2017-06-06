using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;


public class LevelManager : MonoBehaviour {


	//[SerializeField]
	private int _state;


	public Sprite[] tileFace;
	public Sprite[] tileFaceOrange;
	public Sprite[] tileFacePurple;
	public Sprite[] tileFaceBlue;
	public Sprite[] tileName;
	public Sprite tileBack;
	public GameObject[] tiles;
	public Text matchText;
	public GameObject helpPanel;
	public GameObject exitPanel;
	public GameObject retryPanel;
	public GameObject tilesPanel;
	public bool dimPanel = false;
	public bool pauseTime = false;
	public GameObject backgroundMusic;
	private GameObject backgroundMusicObject = null;
	private GameObject fastBackgroundMusicObject = null;



	private bool _init = false;
	private int _matches = 27;

	public Text timerText;
	public Text mode;
	private float startTime;
	private float tempTime;

	// time for extreme mode
	private float timeLeft = 300.0f;
	private float trialTimeLeft = 10.0f;




	// switch to the main scene
	public void homeButtonPressed() {
		exitPanel.SetActive(true);
	}

	public void yesButtonPressed() {
		SceneManager.LoadScene("MainScene");
	}

	public void noButtonPressed() {
		exitPanel.SetActive(false);
	}

	public void yesButtonRetry() {
		SceneManager.LoadScene("Level_Two_TimeTrial_Scene");
	}

	public void noButtonRetry() {
		SceneManager.LoadScene("MainScene");
	}


	public void toggleHelpPanel() {
		pauseTime = !pauseTime;

		if (pauseTime) {
			tempTime = Time.time;
		}

		if (!pauseTime) {
			startTime += (Time.time - tempTime);
		}

		helpPanel.SetActive(!helpPanel.activeInHierarchy);
		dimPanel = !dimPanel;
		if (dimPanel) {
			tilesPanel.GetComponent<CanvasGroup> ().alpha = 0.1f;
		} else {
			tilesPanel.GetComponent<CanvasGroup> ().alpha = 1;

		}

	}

	void configureBackgroundMusic() {
		if(PlayerPrefsX.GetBool("Background_Music_Toggle")) {
			backgroundMusicObject = GameObject.Find("BackgroundMusic(Clone)");
			Destroy(backgroundMusicObject);
			fastBackgroundMusicObject = Instantiate(backgroundMusic,Vector3.zero,Quaternion.identity);
		}
	}


	void Start() {
		helpPanel.SetActive(false);
		setUpHelpPanel ();

		startTime = Time.time;

		configureBackgroundMusic();

	}


	// Update is called once per frame
	void Update () {

		if (!_init)
			initializeCards ();
		if (Input.GetMouseButtonUp (0))
			checkCards ();



		if (!pauseTime) {
			// for extreme mode 
			if (mode.text == "Extreme") {

				timeLeft -= Time.deltaTime;
				timerText.text = "Time: " + Mathf.Round (timeLeft);
				if (timeLeft < 0) {
					retryPanel.SetActive(true);
					pauseTime = !pauseTime;

				}

			} else if (mode.text == "Standard") {
				float t = Time.time - startTime;
				string minutes = ((int)t / 60).ToString ();
				string seconds = (t % 60).ToString ("00");
				timerText.text = "Time: " + minutes + ":" + seconds;


			} else if (mode.text == "Time Trial") {
				trialTimeLeft -= Time.deltaTime;
				timerText.text = "Time: " + Mathf.Round (trialTimeLeft);
				if (trialTimeLeft < 0) {
					retryPanel.SetActive(true);
					pauseTime = !pauseTime;

				}
			}
		}

		
		if(Input.GetKey(KeyCode.Escape)) {
			homeButtonPressed();
		}


	}


	void initializeCards() {


		var rnd = new System.Random();
		var randomNumbers = Enumerable.Range(0,54).OrderBy(x => rnd.Next()).Take(54).ToList();
			
		for (int id = 0; id < 12; id++) {
			tiles [randomNumbers[id]].GetComponent<Tile> ().cardValue = id; 
		}
		for (int id = 12; id < 24; id++) {
			tiles [randomNumbers[id]].GetComponent<Tile> ().cardValue = id-12; 
			tiles [randomNumbers[id]].GetComponent<Tile> ().nameBool = true;

		}
		for (int id = 24; id < 33; id++) {
			tiles [randomNumbers[id]].GetComponent<Tile> ().cardValue = id-24; 
		}
		for (int id = 33; id < 42; id++) {
			tiles [randomNumbers[id]].GetComponent<Tile> ().cardValue = id-33; 
			tiles [randomNumbers[id]].GetComponent<Tile> ().nameBool = true;

		}
		for (int id = 42; id < 48; id++) {
			tiles [randomNumbers[id]].GetComponent<Tile> ().cardValue = id-42; 
		}
		for (int id = 48; id < 54; id++) {
			tiles [randomNumbers[id]].GetComponent<Tile> ().cardValue = id-48; 
			tiles [randomNumbers[id]].GetComponent<Tile> ().nameBool = true;

		}






		////////////////////// Old Without Random from First Version /////////////
	

//
//		for (int id = 0; id < 12; id++) {
//			tiles [id].GetComponent<Tile> ().cardValue = id; 
//		}
//		for (int id = 12; id < 24; id++) {
//			tiles [id].GetComponent<Tile> ().cardValue = id-12; 
//			tiles [id].GetComponent<Tile> ().nameBool = true;
//
//		}
//		for (int id = 24; id < 33; id++) {
//			tiles [id].GetComponent<Tile> ().cardValue = id-24; 
//		}
//		for (int id = 33; id < 42; id++) {
//			tiles [id].GetComponent<Tile> ().cardValue = id-33; 
//			tiles [id].GetComponent<Tile> ().nameBool = true;
//
//		}
//		for (int id = 42; id < 48; id++) {
//			tiles [id].GetComponent<Tile> ().cardValue = id-42; 
//		}
//		for (int id = 48; id < 54; id++) {
//			tiles [id].GetComponent<Tile> ().cardValue = id-48; 
//			tiles [id].GetComponent<Tile> ().nameBool = true;
//
//		}




		//		for (int id = 0; id < 1; id++) {
		//			for (int i = 1; i < 14; i++) {
		//
		//				bool test = false;
		//				int choice = 0;
		//		//		while (!test) {
		//		//			choice = Random.Range (0, cards.Length);
		//		//			test = true;
		//					//test = !(cards [choice].GetComponent<Card> ().intitialzed);
		//		//		}
		//				cards [i-1].GetComponent<Card> ().cardValue = i; 
		//				cards [i-1].GetComponent<Card> ().intitialzed = true;
		//			}
		//		}
		//		for (int id = 0; id < 27; id++) {
		//			for (int i = 1; i < 14; i++) {
		//
		//				bool test = false;
		//				int choice = 0;
		//				//		while (!test) {
		//				//			choice = Random.Range (0, cards.Length);
		//				//			test = true;
		//				//test = !(cards [choice].GetComponent<Card> ().intitialzed);
		//				//		}
		//				cards [i-1].GetComponent<Card> ().cardValue = i; 
		//				cards [i-1].GetComponent<Card> ().intitialzed = true;
		//			}
		//		}

		foreach (GameObject c in tiles)
			c.GetComponent<Tile> ().setupGraphics ();

		if (!_init)
			_init = true;
	}

	public Sprite getTileBack() {
		return tileBack;
	}

	public Sprite getTileFace(int i ) {
		int color = Random.Range (0, 4);
		if (color == 1) {
			return tileFaceOrange [i];
		}
		if (color == 2) {
			return tileFacePurple [i];
		}
		if (color == 3) {
			return tileFaceBlue [i];
		}

		return tileFace [i];
	}
	public Sprite getTileName(int i ) {
		return tileName [i];
	}
	void checkCards() {
		List<int> c = new List<int> ();

		for (int i = 0; i < tiles.Length; i++) { 
			if (tiles [i].GetComponent<Tile> ().state == 0)
				c.Add (i);
		}

		if (c.Count == 2)
			cardCompariosn (c);
	}

	void cardCompariosn (List<int> c ) {
		Tile.DO_NOT = true; 

		int x = 1; 
		if (tiles [c [0]].GetComponent<Tile> ().cardValue == tiles [c [1]].GetComponent<Tile> ().cardValue  && tiles [c [0]].GetComponent<Tile> ().nameBool != tiles [c [1]].GetComponent<Tile> ().nameBool) {
			x = 2;
			_matches--;
			if (mode.text == "Time Trial") {
				trialTimeLeft += 3;
			}
//			matchText.text = "Number of Matches: " + _matches;
//			if (_matches == 0) {
//
//				_matches = 27;
//				SceneManager.LoadScene ("MainScene");
//
//			}

				

		}

		for (int i = 0; i < c.Count; i++) {
			tiles [c [i]].GetComponent<Tile> ().state = x;
			tiles [c [i]].GetComponent<Tile> ().falseCheck ();


		}

		if (_matches == 0) {
			Tile.DO_NOT = false; 
			endGame();

		}
	}

	void endGame() {
		string gameOverTitle = "Title";
		float t = 0f;
		if (mode.text == "Standard") {
			gameOverTitle = "Structure Classification: Standard";
			t = Time.time - startTime;
		}
		else if (mode.text == "Extreme") {
			gameOverTitle = "Structure Classification: Extreme";
			t = timeLeft;
		}
		else if (mode.text == "Time Trial") {
			gameOverTitle = "Structure Classification: Time Trial";
			t = trialTimeLeft;
		}
		int score = (int)t;
		PlayerPrefs.SetString("Game_Title",gameOverTitle);
		PlayerPrefs.SetInt("Score",score);
		PlayerPrefs.SetFloat ("Percentage", t);
		if (!PlayerPrefs.HasKey ("Level_2_Standard_High_Percentage")) {
			PlayerPrefs.SetFloat ("Level_2_Standard_High_Percentage", 0f);
		}

		SceneManager.LoadScene("Game_Over_Scene");


	}

	void setUpHelpPanel() {
		switch (mode.text) {
		case "Standard":
			if(PlayerPrefsX.GetBool("First_Time_Level_Two_Standard",true)) {
				toggleHelpPanel();
				PlayerPrefsX.SetBool("First_Time_Level_Two_Standard",false);
			}
			break;
		case "Time Trial":
			if(PlayerPrefsX.GetBool("First_Time_Level_Two_Trial",true)) {
				toggleHelpPanel();
				PlayerPrefsX.SetBool("First_Time_Level_Two_Trial",false);
			}
			break;
		case "Extreme":
			if(PlayerPrefsX.GetBool("First_Time_Level_Two_Extreme",true)) {
				toggleHelpPanel();
				PlayerPrefsX.SetBool("First_Time_Level_Two_Extreme",false);
			}
			break;
		default:
			break;
		}
	}

}
