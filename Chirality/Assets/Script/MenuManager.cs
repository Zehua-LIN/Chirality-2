using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

	[SerializeField] private Animator levelOneButton;
	[SerializeField] private Animator levelTwoButton;
	[SerializeField] private Animator levelThreeButton;
	[SerializeField] private Animator levelTwoStandardButton;
	[SerializeField] private Animator levelTwoTimeTrialButton;
	[SerializeField] private Animator levelTwoExtremeButton;
	[SerializeField] private Animator settingPanel;

	private bool isLevelTwoClicked = false;
	private bool isSettingClicked = false;

	public void StartGame(string level) {
	
		SceneManager.LoadScene(level);
	}

	public void openSetting() {
		if (isLevelTwoClicked) {
			isSettingClicked = true;
			settingPanel.SetBool ("isHidden", false);
			levelOneButton.SetBool ("isHidden", true);
			levelThreeButton.SetBool ("isHidden", true);
			configureLevelTwoSubButton (true);
		} else {
			isSettingClicked = true;
			settingPanel.SetBool ("isHidden", false);
			configureLevelButton (true);
		}
	}

	public void resumeMainMenu() {
		if (isLevelTwoClicked) {
			isLevelTwoClicked = false;
			isSettingClicked = false;
			settingPanel.SetBool ("isHidden", true);
			levelOneButton.SetBool ("isHidden", false);
			levelTwoButton.SetBool ("isClicked", false);
			levelThreeButton.SetBool ("isHidden", false);
			configureLevelTwoSubButton (true);
		} else {
			isSettingClicked = false;
			settingPanel.SetBool ("isHidden", true);
			configureLevelButton (false);
		}
	}

	public void openLevelTwoButton() {
		isLevelTwoClicked = true;
		levelTwoButton.SetBool ("isClicked", true);
		configureLevelTwoSubButton (false);
	}

	public void configureLevelTwoSubButton(bool hidden) {
		levelTwoStandardButton.SetBool ("isHidden", hidden);
		levelTwoTimeTrialButton.SetBool ("isHidden", hidden);
		levelTwoExtremeButton.SetBool ("isHidden", hidden);
	}

	public void configureLevelButton(bool hidden) {
		levelOneButton.SetBool ("isHidden", hidden);
		levelTwoButton.SetBool ("isHidden", hidden);
		levelThreeButton.SetBool ("isHidden", hidden);
	}

}

