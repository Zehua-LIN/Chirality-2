using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.IO;
using LitJson;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Level_4_QuestionManager : MonoBehaviour
{

    public static Level_4_QuestionManager Instance = null;

    [SerializeField] GameObject[] questionObjects;
    [SerializeField] GameObject deck;
    [SerializeField] Canvas canvas;
    [SerializeField] Text gameTitle;
    [SerializeField] Text scoreNumberLabel;
    [SerializeField] GameObject helpPanel;
    [SerializeField] int gameLevel;
    [SerializeField] Text timer;
    [SerializeField] GameObject exitPanel;
    [SerializeField] Button yesButton;
    [SerializeField] Button noButton;
    [SerializeField] Button NextButton;
    [SerializeField] GameObject leftHandedHelpArrowLeft;
    [SerializeField] GameObject leftHandedHelpArrowRight;
    [SerializeField] Sprite[] buttonSprites;

    private List<Level_4_Question> questions = new List<Level_4_Question>();
    private JsonData questionData;
    private int score = 0;
    private int numberOfQuestionsAnswred = 0;
    private bool soundEffectToggle;
    private GameObject currentQuestion;
    private GameObject currentQuestionAnswer;
    private Level_4_Question currentQuestionObject;
    private gameStatus currentStatus = gameStatus.InGame;
    private GameObject selected_answer = null;
    private bool leftHandMode;

    public gameStatus CurrentStatus
    {
        get
        {
            return currentStatus;
        }
    }

    private int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
            scoreNumberLabel.text = score.ToString() + "/" + numberOfQuestionsAnswred;
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
        int randomNum = Random.Range(0, questions.Count); // random a question
        currentQuestionObject = questions[randomNum];
        currentQuestion = Instantiate(currentQuestionObject.gameObject, canvas.transform, false);	// instantiate the prefab
        if (leftHandMode)
        {
            currentQuestion.transform.localPosition = new Vector2(-currentQuestion.transform.localPosition.x, 0);
        }

        Debug.Log("Current question name:" + currentQuestionObject.name);
        Debug.Log("Current ID: " + currentQuestionObject.ID);
        Debug.Log("Current prefab name: " + currentQuestion.name);
        

        // change the game status and deactivate the answer button
        currentStatus = gameStatus.InGame;
    }

    // configure the functions attached to this button based on different game status
    public void nextButtonPressed()
    {
        
        if (currentStatus == gameStatus.InCheck)
        {

            if (numberOfQuestionsAnswred < 10)
            {
                Destroy(currentQuestion);
                Destroy(currentQuestionAnswer);
                questions.Remove(currentQuestionObject);

                selected_answer.transform.parent.GetComponent<Image>().sprite = buttonSprites[0];
                //selected_answer.transform.GetComponent<Image>().color = Color.white;
                NextButton.transform.GetComponent<Image>().color = Color.white;

                selected_answer = null;

                instantiateRandomQuestionToDisplay();
            }
            else
            {
                // go to game over scene 
                PlayerPrefs.SetString("Game_Title", gameTitle.text);
                float percetange = score / 10f;
                PlayerPrefs.SetInt("Score", score);
                PlayerPrefs.SetFloat("Percentage", percetange);


                if (!PlayerPrefs.HasKey("Level_4_Standard_High_Percentage"))
                {
                    PlayerPrefs.SetFloat("Level_4_Standard_High_Percentage", 0f);
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
                //selected_answer.transform.parent.GetComponent<Image>().color = Color.white;
            }
            selected_answer = caller;

            Color buttonColor = selected_answer.transform.parent.GetComponent<Image>().color;
            if (buttonColor != Color.red)
            {
                caller.transform.parent.GetComponent<Image>().sprite = buttonSprites[1];
                //caller.transform.parent.GetComponent<Image>().color = Color.red;
                NextButton.transform.GetComponent<Image>().color = Color.cyan;
            }
            else
            {
                selected_answer = null;
                caller.transform.parent.GetComponent<Image>().sprite = buttonSprites[0];
                //caller.transform.parent.GetComponent<Image>().color = Color.white;
            }
        }


        
    }

    void checkAnswer()
    {

        if (selected_answer == null)
            return;

        // change the game status
        currentStatus = gameStatus.InCheck;
        numberOfQuestionsAnswred += 1;	// to keep track of how many questions have been answered

        if (selected_answer.name.Equals(currentQuestionObject.name))
        {
            //selected_answer.transform.GetComponent<Image>().color = Color.green;
            selected_answer.transform.parent.GetComponent<Image>().sprite = buttonSprites[2];
            plusScore();
        }
        else
        {
            selected_answer.transform.parent.GetComponent<Image>().sprite = buttonSprites[3];
            scoreNumberLabel.text = score.ToString() + "/" + numberOfQuestionsAnswred;
        }
        
    }

    void plusScore()
    {
        Score++;
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
        switch (gameTitle.text)
        {
            case "Level 4: Isomers":
                if (PlayerPrefsX.GetBool("First_Time_Level_Four", true))
                {
                    helpPanel.SetActive(true);
                    PlayerPrefsX.SetBool("First_Time_Level_Four", false);
                }
                break;
            default:
                break;
        }
    }



}











