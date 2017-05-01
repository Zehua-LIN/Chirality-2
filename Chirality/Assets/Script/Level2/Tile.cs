using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Tile : MonoBehaviour {


	public static bool DO_NOT = false;

	//[SerializeField]
	private int _state;
	//[SerializeField]
	private int _cardValue;
	//[SerializeField]
	private bool _intitialized = false;
	//[SerializeField]
	private bool _name = false;




	private Sprite _cardBack;
	private Sprite _cardFace;

	private GameObject manager;

	void Start() {
		_state = 0;
		manager = GameObject.FindGameObjectWithTag ("Manager");
	}


	public void setupGraphics() {
		_cardBack = manager.GetComponent<LevelManager> ().getTileBack ();
		if (nameBool==true)
			_cardFace = manager.GetComponent<LevelManager> ().getTileName (_cardValue);
		else
			_cardFace = manager.GetComponent<LevelManager> ().getTileFace (_cardValue);

		flipCard ();
	}

	public void flipCard() {

		if (_state == 0)
			_state = 1;
		else if (_state == 1)
			_state = 0;

		if (state == 0 && !DO_NOT) {
			Image image;
			//GetComponent<Image> ().sprite = _cardBack;
			image = GetComponent<Image> ();

			Color c = image.color;
			c.a = 0.2f;
			image.color = c;



		} else if (state == 1 && !DO_NOT) {
			GetComponent<Image> ().sprite = _cardFace;

			Image image = GetComponent<Image> ();

			Color c = image.color;
			c.a = 1;
			image.color = c;
		}
		//GetComponent<Image> ().color.a = 0;

	}

	public int cardValue {
		get { return _cardValue;}
		set { _cardValue = value;}
	}

	public int state {
		get { return _state; }
		set { _state = value; }
	}

	public bool intitialzed {
		get { return _intitialized; }
		set { _intitialized = value; }
	}

	public bool nameBool {
		get { return _name; }
		set { _name = value; }
	}

	public void falseCheck() {
		StartCoroutine (pause ());
	}


	IEnumerator pause() {
		yield return new WaitForSeconds (0);
		//if (_state == 0)
		//	GetComponent<Image> ().sprite = _cardBack;
		 if (_state == 1) {
			//GetComponent<Image> ().sprite = _cardFace;
			Image image = GetComponent<Image> ();

			Color c = image.color;
			c.a = 1;
			image.color = c;
		}
		else if (_state == 2) {
			Image image = GetComponent<Image>();

			Color c = image.color;
			c.a = 0.1f;
			image.color = c;
		}
		DO_NOT = false;
	}



}
