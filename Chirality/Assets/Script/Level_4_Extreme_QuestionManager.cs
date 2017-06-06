using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.IO;
using LitJson;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Level_4_Extreme_QuestionManager : MonoBehaviour
{

    public static Level_4_Extreme_QuestionManager Instance = null;

    [SerializeField] GameObject[] questionObjects;
    [SerializeField] GameObject deck;
    [SerializeField] Canvas canvas;
    [SerializeField] Text gameTitle;
    [SerializeField] GameObject helpPanel;
    [SerializeField] int gameLevel;
    [SerializeField] Text timer;
    [SerializeField] GameObject exitPanel;
    [SerializeField] Button yesButton;
    [SerializeField] Button noButton;
    [SerializeField] GameObject retryPanel;
    [SerializeField] Button retryYesButton;
    [SerializeField] Button retryNoButton;
    [SerializeField] Button NextButton;
    [SerializeField] GameObject leftHandedHelpArrowLeft;
    [SerializeField] GameObject leftHandedHelpArrowRight;
    [SerializeField] Button displayAnswerButton;
    [SerializeField] Sprite[] buttonSprites;
    public GameObject backgroundMusic;
	private GameObject backgroundMusicObject = null;
	private GameObject fastBackgroundMusicObject = null;

    private List<Level_4_Question> questions = new List<Level_4_Question>();
    private JsonData questionData;
    private int numberOfQuestionsAnswred = 0;
    private bool soundEffectToggle;
    private GameObject currentQuestion;
    private GameObject currentQuestionAnswer;
    private Level_4_Question currentQuestionObject;
    private gameStatus currentStatus = gameStatus.InGame;
    private float targetTime = 11.0f;
    private GameObject selected_answer = null;
    private bool leftHandMode;

    public gameStatus CurrentStatus
    {
        get
        {
            return currentStatus;
        }
    }

    void Update()
    {
        if ((currentStatus != gameStatus.InCheck) && (!helpPanel.activeInHierarchy))
        {
            targetTime -= Time.deltaTime;

            int targetTimeInt = (int)targetTime;

            if (targetTimeInt >= 0)
                timer.text = targetTimeInt.ToString();
            else
            {
                retryPanelOpen();
            }
            

            currentQuestion.SetActive(true);
        }
        else
        {
            if (helpPanel.activeInHierarchy)
            {
                currentQuestion.SetActive(false);
            }
            
        }

        
		if(Input.GetKey(KeyCode.Escape)) {
			homeButtonPressed();
		}
	
    }

    void Awake()
    {
        // singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        setUpHelpPanel();

        // for testing
        // PlayerPrefs.DeleteAll();
        leftHandMode = PlayerPrefsX.GetBool("Left_Handle_Toggle", false);
        if (leftHandMode)
        {
            deck.transform.localPosition = new Vector2(-deck.transform.localPosition.x, 0);
            leftHandedHelpArrowLeft.SetActive(true);
            leftHandedHelpArrowRight.SetActive(false);
        }
        else
        {
            leftHandedHelpArrowLeft.SetActive(false);
            leftHandedHelpArrowRight.SetActive(true);
        }

        string path = readJsonData(gameLevel);
        questionData = JsonMapper.ToObject(File.ReadAllText(path));

        loadQuestions();
        instantiateRandomQuestionToDisplay();
        configureBackgroundMusic();
    }

    void configureBackgroundMusic() {
		if(PlayerPrefsX.GetBool("Background_Music_Toggle")) {
			backgroundMusicObject = GameObject.Find("BackgroundMusic(Clone)");
			Destroy(backgroundMusicObject);
			fastBackgroundMusicObject = Instantiate(backgroundMusic,Vector3.zero,Quaternion.identity);
		}
	}



    // create the Question objects from the Questions.json and append them to the List<Question>
    void loadQuestions()
    {
        for (int i = 0; i < questionData.Count; i++)
        {
            questions.Add(new Level_4_Question((int)questionData[i]["id"], (int)questionData[i]["level"], (string)questionData[i]["code"], (string)questionData[i]["name"], questionObjects[i]));
        }
    }

    // pick a random question from the List<Question> and display it
    void instantiateRandomQuestionToDisplay()
    {
        displayAnswerButton.gameObject.SetActive(false);
        NextButton.gameObject.SetActive(false);
        int randomNum = Random.Range(0, questions.Count); // random a question
        currentQuestionObject = questions[randomNum];
        currentQuestion = Instantiate(currentQuestionObject.gameObject, canvas.transform, false);	// instantiate the prefab
        if (leftHandMode)
        {
            currentQuestion.transform.localPosition = new Vector2(-currentQuestion.transform.localPosition.x, 0);
        }

        // change the game status and deactivate the answer button
        currentStatus = gameStatus.InGame;
    }

    // configure the functions attached to this button based on different game status
    public void nextButtonPressed()
    {
        
        if (currentStatus == gameStatus.InCheck)
        {
            if (selected_answer.transform.parent.GetComponent<Image>().sprite == buttonSprites[3])
            {
                retryPanelOpen();
            }
            else if (numberOfQuestionsAnswred < 10)
            {
                Destroy(currentQuestion);
                Destroy(currentQuestionAnswer);
                questions.Remove(currentQuestionObject);
                for (int i = 0; i < deck.transform.childCount; i++)
                {
                    GameObject elementInCell = deck.transform.GetChild(i).GetChild(0).gameObject;
                    elementInCell.transform.parent.GetComponent<Image>().sprite = buttonSprites[0];
                }

                NextButton.transform.GetComponent<Image>().color = Color.white;

                selected_answer = null;

                instantiateRandomQuestionToDisplay();
            }
            else
            {
                // go to game over scene 
                PlayerPrefs.SetString("Game_Title", gameTitle.text);
                PlayerPrefs.SetInt("Score", (int) targetTime);
                PlayerPrefs.SetFloat("Percentage", targetTime);


                if (!PlayerPrefs.HasKey("Level_4_Extreme_High_Percentage"))
                {
                    PlayerPrefs.SetFloat("Level_4_Extreme_High_Percentage", 0f);
                }
                SceneManager.LoadScene("Game_Over_Scene");
            }
        }
        else
        {
            checkAnswer();
        }
         
    }

    public void identifySelf(GameObject caller)
    {
        if (currentStatus != gameStatus.InCheck)
        {
            if (selected_answer != null)
            {
                selected_answer.transform.parent.GetComponent<Image>().sprite = buttonSprites[0];
            }
            selected_answer = caller;

            Color buttonColor = selected_answer.transform.parent.GetComponent<Image>().color;

            caller.transform.parent.GetComponent<Image>().sprite = buttonSprites[1];

            checkAnswer();
        }

        
    
    }

    public void revealAnswer()
    {
        for (int i = 0; i < deck.transform.childCount; i++)
        {
            GameObject elementInCell = deck.transform.GetChild(i).GetChild(0).gameObject;
            if (elementInCell.name.Equals(currentQuestionObject.name))
            {
                elementInCell.transform.parent.GetComponent<Image>().sprite = buttonSprites[2];
                currentStatus = gameStatus.InCheck;
                numberOfQuestionsAnswred += 1;
            }
        }
    }

    void checkAnswer()
    {

        if (selected_answer == null)
            return;

        // change the game status
        currentStatus = gameStatus.InCheck;
        displayAnswerButton.gameObject.SetActive(true);
        NextButton.gameObject.SetActive(true);
        NextButton.transform.GetComponent<Image>().color = Color.cyan;
        numberOfQuestionsAnswred += 1;	// to keep track of how many questions have been answered

        if (selected_answer.name.Equals(currentQuestionObject.name))
        {
            selected_answer.transform.parent.GetComponent<Image>().sprite = buttonSprites[2];
            targetTime = targetTime + 5.0f;
        }
        else
        {
            selected_answer.transform.parent.GetComponent<Image>().sprite = buttonSprites[3];
        }
    }

    // helper methos to convert json array to normal List
    List<string> convertArray(JsonData ary, int index)
    {
        List<string> temp = new List<string>();
        for (int i = 0; i < 6; i++)
        {
            temp.Add((string)ary[index]["answer"][i]);
        }
        return temp;
    }

    // switch to the main scene
    public void homeButtonPressed()
    {
        exitPanel.SetActive(true);
    }

    public void yesButtonPressed()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void noButtonPressed()
    {
        exitPanel.SetActive(false);
    }

    public void retryPanelOpen()
    {
        retryPanel.SetActive(true);
    }

    public void retryYesButtonPressed()
    {
        SceneManager.LoadScene("Level_Four_Scene_Extreme");
    }
    public void retryNoButtonPressed()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void toggleHelpPanel()
    {
        helpPanel.SetActive(!helpPanel.activeInHierarchy);

        
    }

    public void toggleAnswer()
    {
        currentQuestion.SetActive(!currentQuestion.activeInHierarchy);
        currentQuestionAnswer.SetActive(!currentQuestionAnswer.activeInHierarchy);
    }

    string readJsonData(int level)
    {
        string path = "";
        string fileName;

        switch (level)
        {
            case 1:
                fileName = "Level_One_Questions.json";
                break;
            case 3:
                fileName = "Level_Three_Questions.json";
                break;
            case 4:
                fileName = "Level_Four_Questions.json";
                break;
            default:
                fileName = "";
                break;
        }

        var temp = fileName.Split("."[0]);

        if (Application.platform == RuntimePlatform.Android)
        {
            string oriPath = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);
            WWW reader = new WWW(oriPath);
            while (!reader.isDone) { }

            string realPath = Application.persistentDataPath + "/" + temp[0];
            System.IO.File.WriteAllBytes(realPath, reader.bytes);
            path = realPath;
        }
        else
        {
            path = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);
        }
        return path;
    }

    void setUpHelpPanel()
    {
		if (PlayerPrefsX.GetBool("First_Time_Level_Four_Extreme", true)){
			helpPanel.SetActive(true);
			PlayerPrefsX.SetBool ("First_Time_Level_Four_Extreme", false);
		}
    }



}











