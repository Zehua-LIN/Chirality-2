using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionLevel5
{
	public int ID { get; set; }
	public int Level { get; set; }
	public string code { get; set; }
	public string name { get; set; }
	public int numberOfAns { get; set; }
	public GameObject gameObject { get; set; }
	public GameObject extraObject { get; set; }
	public List<string> answer { get; set; }
	public int numberCrToggles { get; set; }
	public string funFact {get; set;}


	public QuestionLevel5(int id, int level, string code, string name, int numOfAns, GameObject gameObj, GameObject extraObj, List<string> answer, int numCrToggles, string fact)
	{
		this.ID = id;
		this.Level = level;
		this.code = code;
		this.name = name;
		this.numberOfAns = numOfAns;
		this.gameObject = gameObj;
		this.extraObject = extraObj;
		this.numberCrToggles = numCrToggles;
		this.answer = answer;
		this.funFact = fact;

	}
}
