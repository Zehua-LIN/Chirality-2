using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;


public class ExtremeManager : MonoBehaviour {


	[SerializeField]
	private int _state;


	public Sprite[] tileFace;
	public Sprite[] tileFaceOrange;
	public Sprite[] tileFacePurple;
	public Sprite[] tileFaceBlue;
	public Sprite[] tileName;
	public Sprite tileBack;
	public GameObject[] tiles;
	public Text matchText;

	private bool _init = false;
	private int _matches = 27;

	public Text timerText;
	private float startTime;





	// switch to the main scene
	public void homeButtonPressed() {
		SceneManager.LoadScene("MainScene");
	}



	void Start() {
		startTime = Time.time;

	}


	// Update is called once per frame
	void Update () {

		float t = Time.time - startTime;
		string minutes = ((int)t / 60).ToString ();
		string seconds = (t % 60).ToString ("f0");
		timerText.text = "Time: " +  minutes + ":" + seconds;

		if (!_init)
			initializeCards ();
		if (Input.GetMouseButtonUp (0))
			checkCards ();
	}


	void initializeCards() {


		var rnd = new System.Random();
		var randomNumbers = Enumerable.Range(0,54).OrderBy(x => rnd.Next()).Take(54).ToList();

		//Debug.Log(randomNumbers.Count);
		for ( int i = 0; i<randomNumbers.Count; i++) {
			Debug.Log(randomNumbers[i]);

		}

			
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
//			matchText.text = "Number of Matches: " + _matches;
			if (_matches == 0)
				SceneManager.LoadScene ("MainScene");
		}

		for (int i = 0; i < c.Count; i++) {
			tiles [c [i]].GetComponent<Tile> ().state = x;
			tiles [c [i]].GetComponent<Tile> ().falseCheck ();


		}
	}
	

}
