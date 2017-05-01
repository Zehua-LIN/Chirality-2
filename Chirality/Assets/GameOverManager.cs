﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour {

	[SerializeField] Text gameTitle;
	[SerializeField] Text scoreLabel;
	[SerializeField] Text percentageLabel;
	[SerializeField] Text highPercentageLabel;
	[SerializeField] Image newRecord;
	[SerializeField] Image[] medals;
	[SerializeField] GameObject infoPanel;
	[SerializeField] Canvas canvas;

	private string title = "";
	private int score = 0;
	private float percentage = 0f;
	private float highPercentage = 0f;

	void Start () {
		infoPanel.SetActive(false);
		newRecord.gameObject.SetActive(false);
		
		loadRecords();
		displayRecord();
		displayMedalAndComment();
		updateHighScore();

	}

	

	void loadRecords() {
		title = PlayerPrefs.GetString("Game_Title");
		percentage = PlayerPrefs.GetFloat("Percentage");
		score = PlayerPrefs.GetInt("Score");
		switch (title)
		{
			case "Level 1: Functional Groups":
				highPercentage = PlayerPrefs.GetFloat("Level_One_High_Percentage");
				break;
			case "Level 3: Intermolecular Forces":
				highPercentage = PlayerPrefs.GetFloat("Level_Three_High_Percentage");
				break;
			default:
				highPercentage = 0f;		
				break;
		}
	}

	void displayRecord() {
		gameTitle.text = title;
		scoreLabel.text = score.ToString();
		highPercentageLabel.text = "Your best was " + (highPercentage * 100).ToString() + "%!";
	}

	void displayMedalAndComment() {

		if (title == "Standard") {
			instantiateMedal(0);
			percentageLabel.text = "You took 2 minutes 14 seconds.";

		}

		else 
		if(percentage < 0.5f) {
			instantiateMedal(0);
			percentageLabel.text = "But your chemistry is a little rusty. You got " + (percentage * 100).ToString() + "%.";
		}else if(percentage >= 0.5f && percentage <= 0.69f) {
			instantiateMedal(1);
			percentageLabel.text = "You joined tin and copper with " + (percentage * 100).ToString() + "%.";
		}else if(percentage >= 0.7f && percentage <= 0.89f) {
			instantiateMedal(2);
			percentageLabel.text = "With that sterling effort you got " + (percentage * 100).ToString() + "%.";
		}else if(percentage >= 0.9f && percentage <= 0.99f) {
			instantiateMedal(3);
			percentageLabel.text = "Your chemistry prowess is gold standard. You got " + (percentage * 100).ToString() + "%.";
		}else {
			instantiateMedal(4);
			percentageLabel.text = "Your knowledge is obviously crystal clear. You got " + (percentage * 100).ToString() + "%.";
		}
	}

	void updateHighScore() {
		if(percentage > highPercentage) {
			newRecord.gameObject.SetActive(true);

			switch (title)
			{
				case "Level 1: Functional Groups":
					PlayerPrefs.SetFloat("Level_One_High_Percentage",percentage);
					break;
				case "Level 3: Intermolecular Forces":
					PlayerPrefs.SetFloat("Level_Three_High_Percentage",percentage);
					break;
				default:
					break;
			}
		}
	}

	public void toggleInfoPanel() {
		infoPanel.SetActive(!infoPanel.activeInHierarchy);
	}

	void instantiateMedal(int i) {
		Instantiate(medals[i],canvas.transform,false);
	}

	public void goToMenuScene() {
		SceneManager.LoadScene("MainScene");
	}

	public void replay() {
		switch (title)
		{
			case "Level 1: Functional Groups":
				SceneManager.LoadScene("Level_One_Scene");					
				break;
			case "Level 3: Intermolecular Forces":
				SceneManager.LoadScene("Level_Three_Scene");	
				break;
			default:
				break;
		}
	}
}
