using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

	[SerializeField] GameObject settingPanel;
	[SerializeField] GameObject infoPanel;
	[SerializeField] GameObject backgroundMusic;	// this is the prefab
	[SerializeField] Toggle backgroundMusicToggle;
	[SerializeField] Toggle soundEffectToggle;
	[SerializeField] Toggle leftHandModeToggle;

	[SerializeField] Button levelTwoButton;
	[SerializeField] GameObject levelTwoPanel;

	[SerializeField] Animator scrollAnimation;

	private GameObject backgroundMusicObject = null;	// this is the actual audio object in game


	void Start() {
		settingPanel.SetActive(false);
		hideLevelTwoPanel();
		
		loadUserSetting();
		
		
		

		
	}

	void loadUserSetting() {
		// read user setting
		backgroundMusicToggle.isOn = PlayerPrefsX.GetBool("Background_Music_Toggle",true);
		soundEffectToggle.isOn = PlayerPrefsX.GetBool("Sound_Effect_Toggle",true);
		leftHandModeToggle.isOn = PlayerPrefsX.GetBool("Left_Handle_Toggle",false);


		configureMusic();
	}

	public void loadGame(string level) {
		SceneManager.LoadScene(level);
		// configureMusic();
	}

	public void saveUserSetting(Toggle toggle) {
		PlayerPrefsX.SetBool(toggle.gameObject.name,toggle.isOn);
		configureMusic();
	}

	public void toggleSetting() {
		settingPanel.SetActive(!settingPanel.activeInHierarchy);
	}

	public void toggleInfoPanel() {
		infoPanel.SetActive(!infoPanel.activeInHierarchy);
	}

	public void displayLevelTwoPanel() {
		levelTwoButton.gameObject.SetActive(false);
		levelTwoPanel.SetActive(true);
	}

	public void hideLevelTwoPanel() {
		levelTwoButton.gameObject.SetActive(true);
		levelTwoPanel.SetActive(false);
	}

	public void stopScrolling() {
		scrollAnimation.Stop();
	}

	void configureMusic() {

		// assign the audio object to local
		if(backgroundMusicObject == null && GameObject.Find("BackgroundMusic(Clone)")) {
				backgroundMusicObject = GameObject.Find("BackgroundMusic(Clone)");
		}
		
		if(backgroundMusicToggle.isOn && !GameObject.Find("BackgroundMusic(Clone)")) {
			// if toggle is on and there is no existing one, instantiate one from the prefab
			backgroundMusicObject = Instantiate(backgroundMusic,Vector3.zero,Quaternion.identity);
			DontDestroyOnLoad(backgroundMusicObject);
		}else if(!backgroundMusicToggle.isOn) {
			// mute background music 
			if(backgroundMusicObject != null) {
				Destroy(backgroundMusicObject);
			}
		}

		if(soundEffectToggle.isOn) {
			

		}else {
			// mute sound effect 

		}
	}
}

