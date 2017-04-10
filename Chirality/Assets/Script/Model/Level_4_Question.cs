using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_4_Question {
public int ID {get; set;}
	public int Level {get; set;}
	public string code {get; set;}
	public string name {get; set;}
	public GameObject gameObject {get; set;}
	public GameObject answerObject {get; set;}
	public List<string> answer {get; set;}

    public Level_4_Question(int id, int level, string code, string name, GameObject gameObj, GameObject answerObj, List<string> answer)
    {
		this.ID = id;
		this.Level = level;
		this.code = code;
		this.name = name;
		this.gameObject = gameObj;
		this.answerObject = answerObj;
		this.answer = answer;
	}
}
