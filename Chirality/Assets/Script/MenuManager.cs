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
	[SerializeField] Button[] levelButtons;
	[SerializeField] Button levelFourStandardButton;
	[SerializeField] Button levelFourExtremeButton;
	[SerializeField] Button levelTwoStandardButton;
	[SerializeField] Button levelTwoTimeButton;
	[SerializeField] Button levelTwoExtremeButton;

	[SerializeField] Image[] medals;
	[SerializeField] Sprite menuButtonSelected;
	[SerializeField] Sprite menuButtonUnselected;

	private GameObject backgroundMusicObject = null;	// this is the actual audio object in game
	private GameObject soundEffectObject = null;
	private Color textColorSelected;
	private Color textColorUnselected;

	void Start() {
		settingPanel.SetActive(false);
		
		loadUserSetting();
		loadMedals();
		convertTextColor();
	}

	void loadUserSetting() {
		// read user setting
		backgroundMusicToggle.isOn = PlayerPrefsX.GetBool("Background_Music_Toggle",true);
		soundEffectToggle.isOn = PlayerPrefsX.GetBool("Sound_Effect_Toggle",false);
		leftHandModeToggle.isOn = PlayerPrefsX.GetBool("Left_Handle_Toggle",false);

		configureBackgroundMusic();
		configureSoundEffect();
	}

	public void loadGame(string level) {
        Debug.Log("Level num is: " + level);
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
		hideLevelFourPanel();
		levelTwoSubMenu.SetBool("isHidden",false);
		levelButtons[1].GetComponent<Image>().sprite = menuButtonSelected;		
		levelButtons[1].GetComponentInChildren<Text>().color = textColorSelected;
	}

	public void hideLevelTwoPanel() {
		levelTwoSubMenu.SetBool("isHidden",true);
		levelButtons[1].GetComponent<Image>().sprite = menuButtonUnselected;
		levelButtons[1].GetComponentInChildren<Text>().color = textColorUnselected;
	}

	public void displayLevelFourPanel() {
		hideLevelTwoPanel();
		levelFourSubMenu.SetBool("isHidden",false);
		levelButtons[3].GetComponent<Image>().sprite = menuButtonSelected;		
		levelButtons[3].GetComponentInChildren<Text>().color = textColorSelected;
	}

	public void hideLevelFourPanel() {
		levelFourSubMenu.SetBool("isHidden",true);
		levelButtons[3].GetComponent<Image>().sprite = menuButtonUnselected;
		levelButtons[3].GetComponentInChildren<Text>().color = textColorUnselected;
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

	public void resetScore() {
		for(int i = 1; i <= 6; i++) {
			PlayerPrefs.DeleteKey("Level_" + i + "_High_Percentage");
			if(levelButtons[i-1].transform.childCount > 1) {
				Destroy(levelButtons[i-1].transform.GetChild(1).gameObject);
			}
		}

		// level 2
		if(PlayerPrefs.HasKey("Level_2_Standard_High_Percentage")) {
			PlayerPrefs.DeleteKey("Level_2_Standard_High_Percentage");
			if(levelTwoStandardButton.transform.childCount > 1) {
				Destroy(levelTwoStandardButton.transform.GetChild(1).gameObject);
			}
		}
		if(PlayerPrefs.HasKey("Level_2_Trial_High_Percentage")) {
			PlayerPrefs.DeleteKey("Level_2_Trial_High_Percentage");
			if(levelTwoTimeButton.transform.childCount > 1) {
				Destroy(levelTwoTimeButton.transform.GetChild(1).gameObject);
			}
		}
		if(PlayerPrefs.HasKey("Level_2_Extreme_High_Percentage")) {
			PlayerPrefs.DeleteKey("Level_2_Extreme_High_Percentage");
			if(levelTwoExtremeButton.transform.childCount > 1) {
				Destroy(levelTwoExtremeButton.transform.GetChild(1).gameObject);
			}
		}

		// level 4
		if(PlayerPrefs.HasKey("Level_4_Standard_High_Percentage")) {
			PlayerPrefs.DeleteKey("Level_4_Standard_High_Percentage");
			if(levelFourStandardButton.transform.childCount > 1) {
				Destroy(levelFourStandardButton.transform.GetChild(1).gameObject);
			}
		}
		if(PlayerPrefs.HasKey("Level_4_Standard_Extreme_Percentage")) {
			PlayerPrefs.DeleteKey("Level_4_Standard_Extreme_Percentage");
			if(levelFourExtremeButton.transform.childCount > 1) {
				Destroy(levelFourExtremeButton.transform.GetChild(1).gameObject);
			}
		}
		
		
	}

	void loadMedals() {
		for(int i = 1; i <= 6; i++) {
			if(PlayerPrefs.HasKey("Level_" + i + "_High_Percentage")) {
				float highest = PlayerPrefs.GetFloat("Level_" + i + "_High_Percentage");				
				int medalNumber = getMedal(highest);
				Image medal = Instantiate(medals[medalNumber],levelButtons[i-1].transform,false);
				medal.rectTransform.sizeDelta = new Vector2(70,70);
				medal.transform.localPosition = new Vector2(200,0);				
			}					
		}		

		// lvl 4 sub medals
		if(PlayerPrefs.HasKey("Level_4_Standard_High_Percentage")) {
			float highest = PlayerPrefs.GetFloat("Level_4_Standard_High_Percentage");				
			int medalNumber = getMedal(highest);
			Image medal = Instantiate(medals[medalNumber],levelFourStandardButton.transform,false);
			medal.rectTransform.sizeDelta = new Vector2(70,70);
			medal.transform.localPosition = new Vector2(200,0);	
		}
		if(PlayerPrefs.HasKey("Level_4_Standard_Extreme_Percentage")) {
			float highest = PlayerPrefs.GetFloat("Level_4_Extreme_High_Percentage");				
			int medalNumber = getMedalForLevel4Extreme(highest);
			Image medal = Instantiate(medals[medalNumber],levelFourExtremeButton.transform,false);
			medal.rectTransform.sizeDelta = new Vector2(70,70);
			medal.transform.localPosition = new Vector2(200,0);	
		}

		// lvl2 sub medals
		if(PlayerPrefs.HasKey("Level_2_Standard_High_Percentage")) {
			float highest = PlayerPrefs.GetFloat("Level_2_Standard_High_Percentage");				
			int medalNumber = getMedalForLevel2Standard(highest);
			Image medal = Instantiate(medals[medalNumber],levelTwoStandardButton.transform,false);
			medal.rectTransform.sizeDelta = new Vector2(70,70);
			medal.transform.localPosition = new Vector2(100,0);	
		}
		if(PlayerPrefs.HasKey("Level_2_Trial_High_Percentage")) {
			float highest = PlayerPrefs.GetFloat("Level_2_Trial_High_Percentage");				
			int medalNumber = getMedalForLevel2TimeTrial(highest);
			Image medal = Instantiate(medals[medalNumber],levelTwoTimeButton.transform,false);
			medal.rectTransform.sizeDelta = new Vector2(70,70);
			medal.transform.localPosition = new Vector2(100,0);	
		}
		if(PlayerPrefs.HasKey("Level_2_Extreme_High_Percentage")) {
			float highest = PlayerPrefs.GetFloat("Level_2_Extreme_High_Percentage");				
			int medalNumber = getMedalForLevel2Extreme(highest);
			Image medal = Instantiate(medals[medalNumber],levelTwoExtremeButton.transform,false);
			medal.rectTransform.sizeDelta = new Vector2(70,70);
			medal.transform.localPosition = new Vector2(100,0);	
		}
	}

	int getMedal(float score) {
		if(score < 0.5f) {
			return 0;
		}else if(score >= 0.5f && score <= 0.69f) {
			return 1;
		}else if(score >= 0.7f && score <= 0.89f) {
			return 2;
		}else if(score >= 0.9f && score <= 0.99f) {
			return 3;
		}else {
			return 4;
		}
	}

	int getMedalForLevel4Extreme(float score) {
		if(score >= 40) {
			return 4;
		}else if(score >= 30) {
			return 3;
		}else if(score >= 20) {
			return 2;
		}else if (score >= 10) {
			return 1;
		}else {
			return 0;
		}
	}

	int getMedalForLevel2Standard(float score) {
		if (score <= 80) {
			return 4;
		}
		else if (score <= 120) {
			return 3;
		}
		else if (score <= 180) {
			return 2;
		}
		else if (score <= 240) {
			return 1;
		}
		else{
			return 0;
		}
	}

	int getMedalForLevel2Extreme(float score) {
		if (score <= 70) {
			return 0;
		}
		else if (score <= 120) {
			return 1;
		}
		else if (score <= 180) {
			return 2;
		}
		else if (score < 240) {
			return 3;
		}
		else {
			return 4;
		}
	}

	int getMedalForLevel2TimeTrial(float score) {
		if (score <= 20) {
			return 0;
		}
		else if (score <= 35) {
			return 1;
		}
		else if (score <= 50) {
			return 2;
		}
		else if (score < 65) {
			return 3;
		}
		else {
			return 4;
		}
	}

	void convertTextColor() {
		textColorUnselected = Color.white;
		textColorSelected = new Color();
		ColorUtility.TryParseHtmlString("#072B3BFF",out textColorSelected);
	}
}

