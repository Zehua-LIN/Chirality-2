using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

	[SerializeField] GameObject settingPanel;
	[SerializeField] GameObject infoPanel;
	[SerializeField] GameObject backgroundMusic;	// this is the prefab
	[SerializeField] GameObject soundEffect;
	[SerializeField] Toggle backgroundMusicToggle;
	[SerializeField] Toggle soundEffectToggle;
	[SerializeField] Toggle leftHandModeToggle;
	[SerializeField] GameObject levelTwoPanel;
	[SerializeField] Animator scrollAnimation;
	[SerializeField] Animator levelTwoSubMenu;
	[SerializeField] Animator levelFourSubMenu;

	private GameObject backgroundMusicObject = null;	// this is the actual audio object in game
	private GameObject soundEffectObject = null;

	void Start() {
		settingPanel.SetActive(false);
		
		loadUserSetting();
			
	}


	void loadUserSetting() {
		// read user setting
		backgroundMusicToggle.isOn = PlayerPrefsX.GetBool("Background_Music_Toggle",true);
		soundEffectToggle.isOn = PlayerPrefsX.GetBool("Sound_Effect_Toggle",true);
		leftHandModeToggle.isOn = PlayerPrefsX.GetBool("Left_Handle_Toggle",false);

		configureBackgroundMusic();
		configureSoundEffect();
	}

	public void loadGame(string level) {
		SceneManager.LoadScene(level);
	}

	public void saveUserSetting(Toggle toggle) {
		PlayerPrefsX.SetBool(toggle.gameObject.name,toggle.isOn);
		configureBackgroundMusic();
		configureSoundEffect();
	}

	public void toggleSetting() {
		settingPanel.SetActive(!settingPanel.activeInHierarchy);
	}

	public void toggleInfoPanel() {
		infoPanel.SetActive(!infoPanel.activeInHierarchy);
	}

	public void displayLevelTwoPanel() {
		// if(!levelFourSubMenu.GetBool("isHidden")) {
		// 	hideLevelFourPanel();
		// }
		hideLevelFourPanel();
		levelTwoSubMenu.SetBool("isHidden",false);
	}

	public void hideLevelTwoPanel() {
		levelTwoSubMenu.SetBool("isHidden",true);
	}

	public void displayLevelFourPanel() {
		// if(!levelTwoSubMenu.GetBool("isHidden")) {
		// 	hideLevelTwoPanel();
		// }
		hideLevelTwoPanel();
		levelFourSubMenu.SetBool("isHidden",false);
	}

	public void hideLevelFourPanel() {
		levelFourSubMenu.SetBool("isHidden",true);
	}

	public void stopScrolling() {
		scrollAnimation.Stop();
	}

	void configureBackgroundMusic() {
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
	}

	void configureSoundEffect() {
		if(soundEffectObject == null && GameObject.Find("SoundEffect(Clone)")) {
			soundEffectObject = GameObject.Find("SoundEffect(Clone)");
		}

		if(soundEffectToggle.isOn && !GameObject.Find("SoundEffect(Clone)")) {
			soundEffectObject = Instantiate(soundEffect,Vector3.zero,Quaternion.identity);
			DontDestroyOnLoad(soundEffectObject);
		}else if(!soundEffectToggle.isOn) {
			if(soundEffectObject != null) {
				Destroy(soundEffectObject);
			}
		}
	}
}

