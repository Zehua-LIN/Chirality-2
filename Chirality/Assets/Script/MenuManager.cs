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

        setTimesAndScoresForUse();
        setLastFiftyGames();
        loadUserSetting();
		loadMedals();
		convertTextColor();
	}

	void Update() {
		if(Input.GetKey(KeyCode.Escape)) {
			Application.Quit();
		}
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
		//scrollAnimation.Stop();
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
		if(PlayerPrefs.HasKey("Level_4_Extreme_High_Percentage")) {
			PlayerPrefs.DeleteKey("Level_4_Extreme_High_Percentage");
			if(levelFourExtremeButton.transform.childCount > 1) {
				Destroy(levelFourExtremeButton.transform.GetChild(1).gameObject);
			}
		}

        for (int i = 1; i < 7; i++)
        {
            if (i != 2 && i != 4)
            {
                PlayerPrefs.SetString("Level_" + i + "_Times", "");
                PlayerPrefs.SetString("Level_" + i + "_Percentages", "");
                PlayerPrefs.SetString("Level_" + i + "_Days", "");
                PlayerPrefs.SetInt("Level_" + i + "_Already_Played", 0);
                PlayerPrefs.DeleteKey("Level_" + i + "_Time");
                PlayerPrefs.SetInt("Level_" + i + "_Times_Pl", 0);
            }
            else
            {
                PlayerPrefs.SetString("Level_" + i + "_Times_Stdd", "");
                PlayerPrefs.SetString("Level_" + i + "_Percentages_Stdd", "");
                PlayerPrefs.SetString("Level_" + i + "_Days_Stdd", "");
                PlayerPrefs.SetInt("Level_" + i + "_Already_Played_Stdd", 0);
                PlayerPrefs.DeleteKey("Level_" + i + "_Time_Stdd");
                PlayerPrefs.SetInt("Level_" + i + "_Times_Pl_Stdd", 0);


                PlayerPrefs.SetString("Level_" + i + "_Times_Ext", "");
                PlayerPrefs.SetString("Level_" + i + "_Percentages_Ext", "");
                PlayerPrefs.SetString("Level_" + i + "_Days_Ext", "");
                PlayerPrefs.SetInt("Level_" + i + "_Already_Played_Ext", 0);
                PlayerPrefs.DeleteKey("Level_" + i + "_Time_Ext");
                PlayerPrefs.SetInt("Level_" + i + "_Times_Pl_Ext", 0);

                if (i == 2)
                {

                    PlayerPrefs.SetString("Level_" + i + "_Times_Time_Trial", "");
                    PlayerPrefs.SetString("Level_" + i + "_Percentages_Time_Trial", "");
                    PlayerPrefs.SetString("Level_" + i + "_Days_Time_Trial", "");
                    PlayerPrefs.SetInt("Level_" + i + "_Already_Played_Time_Trial", 0);
                    PlayerPrefs.DeleteKey("Level_" + i + "_Time_Time_Trial");
                    PlayerPrefs.SetInt("Level_" + i + "_Times_Pl_Time_Trial", 0);
                }

            }
            PlayerPrefs.SetFloat("Time_P", 0f);
        }

    }

	void loadMedals() {
		int level4High = -1;
		int level2High = -1;
		string level4Name = "";
		string level2Name = "";

		// lvl 4 sub medals
		if(PlayerPrefs.HasKey("Level_4_Standard_High_Percentage")) {
			float highest = PlayerPrefs.GetFloat("Level_4_Standard_High_Percentage");
			int temp = getMedal(highest);
			if(temp > level4High) {
				level4High = temp;
				level4Name = "Standard";
				PlayerPrefs.SetFloat("Level_4_High_Percentage",highest);
			}			
			int medalNumber = getMedal(highest);
			Image medal = Instantiate(medals[medalNumber],levelFourStandardButton.transform,false);
			medal.rectTransform.sizeDelta = new Vector2(65,65);
			medal.transform.localPosition = new Vector2(200,0);	
		}
		if(PlayerPrefs.HasKey("Level_4_Extreme_High_Percentage")) {
			float highest = PlayerPrefs.GetFloat("Level_4_Extreme_High_Percentage");
			int temp = getMedalForLevel4Extreme(highest);
			if(temp > level4High) {
				level4High = temp;
				level4Name = "Extreme";
				PlayerPrefs.SetFloat("Level_4_High_Percentage",highest);
			}				
			int medalNumber = getMedalForLevel4Extreme(highest);
			Image medal = Instantiate(medals[medalNumber],levelFourExtremeButton.transform,false);
			medal.rectTransform.sizeDelta = new Vector2(65,65);
			medal.transform.localPosition = new Vector2(200,0);	
		}

		// lvl2 sub medals
		if(PlayerPrefs.HasKey("Level_2_Standard_High_Percentage")) {
			float highest = PlayerPrefs.GetFloat("Level_2_Standard_High_Percentage");
			int temp = getMedalForLevel2Standard(highest);
			if(temp > level2High) {
				level2High = temp;
				level2Name = "Standard";
				PlayerPrefs.SetFloat("Level_2_High_Percentage",highest);
			}			
			int medalNumber = getMedalForLevel2Standard(highest);
			Image medal = Instantiate(medals[medalNumber],levelTwoStandardButton.transform,false);
			medal.rectTransform.sizeDelta = new Vector2(65,65);
			medal.transform.localPosition = new Vector2(100,0);	
		}
		if(PlayerPrefs.HasKey("Level_2_Trial_High_Percentage")) {
			float highest = PlayerPrefs.GetFloat("Level_2_Trial_High_Percentage");
			int temp = getMedalForLevel2TimeTrial(highest);
			if(temp > level2High) {
				level2High = temp;
				level2Name = "Time";
				PlayerPrefs.SetFloat("Level_2_High_Percentage",highest);
			}				
			int medalNumber = getMedalForLevel2TimeTrial(highest);
			Image medal = Instantiate(medals[medalNumber],levelTwoTimeButton.transform,false);
			medal.rectTransform.sizeDelta = new Vector2(65,65);
			medal.transform.localPosition = new Vector2(100,0);	
		}
		if(PlayerPrefs.HasKey("Level_2_Extreme_High_Percentage")) {
			float highest = PlayerPrefs.GetFloat("Level_2_Extreme_High_Percentage");
			int temp = getMedalForLevel2Extreme(highest);
			if(temp > level2High) {
				level2High = temp;
				level2Name = "Extreme";
				PlayerPrefs.SetFloat("Level_2_High_Percentage",highest);
			}				
			int medalNumber = getMedalForLevel2Extreme(highest);
			Image medal = Instantiate(medals[medalNumber],levelTwoExtremeButton.transform,false);
			medal.rectTransform.sizeDelta = new Vector2(65,65);
			medal.transform.localPosition = new Vector2(100,0);	
		}


		for(int i = 1; i <= 6; i++) {
			if(PlayerPrefs.HasKey("Level_" + i + "_High_Percentage")) {
				int medalNumber = -1;
				float highest = PlayerPrefs.GetFloat("Level_" + i + "_High_Percentage");	
				if(i == 2) {
					switch(level2Name) {
						case "Standard":
							medalNumber = getMedalForLevel2Standard(highest);
							break;
						case "Time":
							medalNumber = getMedalForLevel2TimeTrial(highest);
							break;
						case "Extreme":
							medalNumber = getMedalForLevel2Extreme(highest);
							break;
					}
				}else if(i == 4) {
					switch(level4Name) {
						case "Standard":
							medalNumber = getMedal(highest);
							break;			
						case "Extreme":
							medalNumber = getMedalForLevel4Extreme(highest);
							break;
					}
				}else {
					medalNumber = getMedal(highest);
				}
				
				Image medal = Instantiate(medals[medalNumber],levelButtons[i-1].transform,false);
				medal.rectTransform.sizeDelta = new Vector2(65,65);
				medal.transform.localPosition = new Vector2(200,0);				
			}					
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

    public void goToTimesAndScores()
    {
        SceneManager.LoadScene("Time_And_Score");
    }

    void setTimesAndScoresForUse()
    {
        string times = "";
        string scores = "";

        for (int i = 1; i < 7; i++)
        {
            if (i != 2 && i != 4)
            {
                if (!PlayerPrefs.HasKey("Level_" + i + "_Times"))
                {
                    PlayerPrefs.SetString("Level_" + i + "_Times", "");
                }

                if (!PlayerPrefs.HasKey("Level_" + i + "_Percentages"))
                {
                    PlayerPrefs.SetString("Level_" + i + "_Percentages", "");
                }

                if (!PlayerPrefs.HasKey("Level_" + i + "_Days"))
                {
                    PlayerPrefs.SetString("Level_" + i + "_Days", "");
                }

                if (!PlayerPrefs.HasKey("Level_" + i + "_Already_Played"))
                {
                    PlayerPrefs.SetInt("Level_" + i + "_Already_Played", 0);
                }

                if (!PlayerPrefs.HasKey("Level_" + i + "_Times_Pl"))
                {
                    PlayerPrefs.SetInt("Level_" + i + "_Times_Pl", 0);
                }
            }
            else
            {
                if (!PlayerPrefs.HasKey("Level_" + i + "_Times_Stdd"))
                {
                    PlayerPrefs.SetString("Level_" + i + "_Times_Stdd", "");
                }

                if (!PlayerPrefs.HasKey("Level_" + i + "_Percentages_Stdd"))
                {
                    PlayerPrefs.SetString("Level_" + i + "_Percentages_Stdd", "");
                }

                if (!PlayerPrefs.HasKey("Level_" + i + "_Days_Stdd"))
                {
                    PlayerPrefs.SetString("Level_" + i + "_Days_Stdd", "");
                }

                if (!PlayerPrefs.HasKey("Level_" + i + "_Already_Played_Stdd"))
                {
                    PlayerPrefs.SetInt("Level_" + i + "_Already_Played_Stdd", 0);
                }

                if (!PlayerPrefs.HasKey("Level_" + i + "_Times_Pl_Stdd"))
                {
                    PlayerPrefs.SetInt("Level_" + i + "_Times_Pl_Stdd", 0);
                }

                if (!PlayerPrefs.HasKey("Level_" + i + "_Times_Ext"))
                {
                    PlayerPrefs.SetString("Level_" + i + "_Times_Ext", "");
                }

                if (!PlayerPrefs.HasKey("Level_" + i + "_Percentages_Ext"))
                {
                    PlayerPrefs.SetString("Level_" + i + "_Percentages_Ext", "");
                }

                if (!PlayerPrefs.HasKey("Level_" + i + "_Days_Ext"))
                {
                    PlayerPrefs.SetString("Level_" + i + "_Days_Ext", "");
                }

                if (!PlayerPrefs.HasKey("Level_" + i + "_Already_Played_Ext"))
                {
                    PlayerPrefs.SetInt("Level_" + i + "_Already_Played_Ext", 0);
                }

                if (!PlayerPrefs.HasKey("Level_" + i + "_Times_Pl_Ext"))
                {
                    PlayerPrefs.SetInt("Level_" + i + "_Times_Pl_Ext", 0);
                }

                if (i == 2)
                {
                    if (!PlayerPrefs.HasKey("Level_" + i + "_Times_Time_Trial"))
                    {
                        PlayerPrefs.SetString("Level_" + i + "_Times_Time_Trial", "");
                    }

                    if (!PlayerPrefs.HasKey("Level_" + i + "_Percentages_Time_Trial"))
                    {
                        PlayerPrefs.SetString("Level_" + i + "_Percentages_Time_Trial", "");
                    }

                    if (!PlayerPrefs.HasKey("Level_" + i + "_Days_Time_Trial"))
                    {
                        PlayerPrefs.SetString("Level_" + i + "_Days_Time_Trial", "");
                    }

                    if (!PlayerPrefs.HasKey("Level_" + i + "_Already_Played_Time_Trial"))
                    {
                        PlayerPrefs.SetInt("Level_" + i + "_Already_Played_Time_Trial", 0);
                    }

                    if (!PlayerPrefs.HasKey("Level_" + i + "_Times_Pl_Time_Trial"))
                    {
                        PlayerPrefs.SetInt("Level_" + i + "_Times_Pl_Time_Trial", 0);
                    }
                }

                if (!PlayerPrefs.HasKey("Time_P"))
                {
                    PlayerPrefs.SetFloat("Time_P", 0f);
                }

            }
        }
    }

    void setLastFiftyGames()
    {
        for (int i = 1; i < 7; i++)
        {
            string scores = "";
            string times = "";
            string days = "";

            if (i != 2 && i != 4)
            {
                if (PlayerPrefs.GetInt("Level_" + i + "_Times_Pl") > 50)
                {

                    scores = PlayerPrefs.GetString("Level_" + i + "_Percentages");
                    times = PlayerPrefs.GetString("Level_" + i + "_Times");
                    days = PlayerPrefs.GetString("Level_" + i + "_Days");

                    if (PlayerPrefs.GetInt("Level_" + i + "_Times_Pl") > 50)
                    {
                        updateTimesScoresAndDays(i, scores, times, days, "n");
                    }

                }
            }
            else
            {
                scores = PlayerPrefs.GetString("Level_" + i + "_Percentages_Stdd");
                times = PlayerPrefs.GetString("Level_" + i + "_Times_Stdd");
                days = PlayerPrefs.GetString("Level_" + i + "_Days_Stdd");

                if (PlayerPrefs.GetInt("Level_" + i + "_Times_Pl_Stdd") > 50)
                {
                    updateTimesScoresAndDays(i, scores, times, days, "stdd");
                }

                scores = PlayerPrefs.GetString("Level_" + i + "_Percentages_Ext");
                times = PlayerPrefs.GetString("Level_" + i + "_Times_Ext");
                days = PlayerPrefs.GetString("Level_" + i + "_Days_Ext");

                if (PlayerPrefs.GetInt("Level_" + i + "_Times_Pl_Ext") > 50)
                {
                    updateTimesScoresAndDays(i, scores, times, days, "ext");
                }

                if (i == 2)
                {
                    scores = PlayerPrefs.GetString("Level_" + i + "_Percentages_Time_Trial");
                    times = PlayerPrefs.GetString("Level_" + i + "_Times_Time_Trial");
                    days = PlayerPrefs.GetString("Level_" + i + "_Days_Time_Trial");

                    if (PlayerPrefs.GetInt("Level_" + i + "_Times_Pl_Time_Trial") > 50)
                    {
                        updateTimesScoresAndDays(i, scores, times, days, "trial");
                    }
                }

            }
        }
    }

    void updateTimesScoresAndDays(int i, string scores, string times, string days, string s)
    {
        char[] c = new char[1];
        c[0] = ',';
        string[] arrayScores = scores.Split(c);
        string[] arrayTimes = times.Split(c);
        string[] arrayDays = days.Split(c);

        if (arrayTimes.Length > 51)
        {
            Debug.Log("updating times st");
            string[] arraynw = new string[51];
            string sttms = "";

            for (int ii = 0; ii < 51; ii++)
            {
                arraynw[50 - ii] = arrayTimes[arrayTimes.Length - 1 - ii];
            }
            arrayTimes = new string[51];

            for (int iii = 0; iii < 51 - 1; iii++)
            {
                arrayTimes[iii] = arraynw[iii];
                sttms += arraynw[iii] + ",";
            }


            if (i != 2 && i != 4)
            {
                PlayerPrefs.SetString("Level_" + i + "_Times", sttms);
            }
            else
            {
                if (s == "stdd")
                {
                    PlayerPrefs.SetString("Level_" + i + "_Times_Stdd", sttms);
                }
                else if (s == "ext")
                {
                    PlayerPrefs.SetString("Level_" + i + "_Times_Ext", sttms);
                }
                else if (s == "trial")
                {
                    PlayerPrefs.SetString("Level_" + i + "_Times_Time_Trial", sttms);
                }
            }

        }

        if (arrayScores.Length > 51)
        {
            Debug.Log("updating scores st");
            string[] arraynw = new string[51];
            string sttms = "";

            for (int ii = 0; ii < 51; ii++)
            {
                arraynw[50 - ii] = arrayScores[arrayScores.Length - 1 - ii];
            }
            arrayScores = new string[51];

            for (int iii = 0; iii < 51 - 1; iii++)
            {
                arrayScores[iii] = arraynw[iii];
                sttms += arraynw[iii] + ",";
            }

            if (i != 2 && i != 4)
            {
                PlayerPrefs.SetString("Level_" + i + "_Percentages", sttms);
            }
            else
            {
                if (s == "stdd")
                {
                    PlayerPrefs.SetString("Level_" + i + "_Percentages_Stdd", sttms);
                }
                else if (s == "ext")
                {
                    PlayerPrefs.SetString("Level_" + i + "_Percentages_Ext", sttms);
                }

                else if (s == "trial")
                {
                    PlayerPrefs.SetString("Level_" + i + "_Percentages_Time_Trial", sttms);
                }
            }

        }


        if (arrayDays.Length > 51)
        {
            Debug.Log("updating days st");
            string[] arraynw = new string[51];
            string stdays = "";

            for (int ii = 0; ii < 51; ii++)
            {
                arraynw[50 - ii] = arrayDays[arrayDays.Length - 1 - ii];
            }
            arrayDays = new string[51];

            for (int iii = 0; iii < 50; iii++)
            {
                arrayDays[iii] = arraynw[iii];
                stdays += arraynw[iii] + ",";
            }


            if (i != 2 && i != 4)
            {
                PlayerPrefs.SetString("Level_" + i + "_Days", stdays);
            }
            else
            {
                if (s == "stdd")
                {
                    PlayerPrefs.SetString("Level_" + i + "_Days_Stdd", stdays);
                }
                else if (s == "ext")
                {
                    PlayerPrefs.SetString("Level_" + i + "_Days_Ext", stdays);
                }

                else if (s == "trial")
                {
                    PlayerPrefs.SetString("Level_" + i + "_Days_Time_Trial", stdays);
                }
            }
        }
    }

}

