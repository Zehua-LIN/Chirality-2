using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question {
public int ID {get; set;}
	public int Level {get; set;}
	public string code {get; set;}
	public string name {get; set;}
	public int numberOfCells {get; set;}
	public string funFact {get; set;}
	public GameObject gameObject {get; set;}
	public GameObject answerObject {get; set;}
	public List<string> answer {get; set;}

	public Question(int id, int level, string code, string name, int numOfCells,string fact, GameObject gameObj, GameObject answerObj, List<string> answer) {
		this.ID = id;
		this.Level = level;
		this.code = code;
		this.name = name;
		this.numberOfCells = numOfCells;
		this.funFact = fact;
		this.gameObject = gameObj;
		this.answerObject = answerObj;
		this.answer = answer;
	}
}
