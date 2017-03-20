using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question {
public int ID {get; set;}
	public int Level {get; set;}
	public string code {get; set;}
	public string name {get; set;}
	public int numberOfCells {get; set;}
	public GameObject gameObj {get; set;}
	public List<string> answer {get; set;}

	public Question(int id, int level, string code, string name, int numOfCells,GameObject obj,List<string> answer) {
		this.ID = id;
		this.Level = level;
		this.code = code;
		this.name = name;
		this.numberOfCells = numOfCells;
		this.gameObj = obj;
		this.answer = answer;
	}
}
