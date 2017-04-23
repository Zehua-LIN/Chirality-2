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
    [SerializeField] GameObject[] questionAnswerObjects;
    [SerializeField] Canvas canvas;
    [SerializeField] Text gameTitle;
    [SerializeField] Text scoreNumberLabel;
    [SerializeField] GameObject helpPanel;
    [SerializeField] int gameLevel;

    private List<Level_4_Question> questions = new List<Level_4_Question>();
    private JsonData questionData;
    private int score = 0;
    private int numberOfQuestionsAnswred = 0;
    private float totalNumberOfCells = 0f;
    private bool soundEffectToggle;
    private GameObject currentQuestion;
    private GameObject currentQuestionAnswer;
    private Level_4_Question currentQuestionObject;
    private gameStatus currentStatus = gameStatus.InGame;

    private GameObject selected_answer = null;

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
            scoreNumberLabel.text = score.ToString();
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
        // for testing
        // PlayerPrefs.DeleteAll();

        helpPanel.SetActive(false);

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
            // Debug.Log("Question ID: " + (int)questionData[i]["id"]);
            questions.Add(new Level_4_Question((int)questionData[i]["id"], (int)questionData[i]["level"], (string)questionData[i]["code"], (string)questionData[i]["name"], questionObjects[i], questionAnswerObjects[i], convertArray(questionData, i)));
        }
    }

    // pick a random question from the List<Question> and display it
    void instantiateRandomQuestionToDisplay()
    {
        int randomNum = Random.Range(0, questions.Count); // random a question
        currentQuestionObject = questions[randomNum];
        currentQuestion = Instantiate(currentQuestionObject.gameObject, canvas.transform, false);	// instantiate the prefab
        currentQuestionAnswer = Instantiate(currentQuestionObject.answerObject, canvas.transform, false);
        currentQuestionAnswer.SetActive(false);

        // change the game status and deactivate the answer button
        currentStatus = gameStatus.InGame;
    }

    // configure the functions attached to this button based on different game status
    public void nextButtonPressed()
    {
        checkAnswer();

        /**
        if (currentStatus == gameStatus.InGame)
        {
            checkAnswer();
        }
         **/
    }

    public void identifySelf(GameObject caller)
    {
        selected_answer = caller;

        /**
        if (caller.name.Equals("positional"))
        {
            caller.transform.GetComponent<Image>().color = Color.green;
        }
        else
        {
            caller.transform.GetComponent<Image>().color = Color.red;
        }

        Debug.Log("Hello World");
        Debug.Log("I am a: " + caller.name);
        **/
    }

    void checkAnswer()
    {
        /**
        // check for empty slots, return if there is empty one
        for (int i = 0; i < currentQuestion.transform.childCount; i++)
        {
            if (currentQuestion.transform.GetChild(i).childCount == 0)
            {
                return;
            }
        }
         **/

        // change the game status
        currentStatus = gameStatus.InCheck;
        numberOfQuestionsAnswred += 1;	// to keep track of how many questions have been answered

        if (selected_answer.name.Equals(currentQuestionObject.name))
        {
            selected_answer.transform.GetComponent<Image>().color = Color.green;
        }
        else
        {
            selected_answer.transform.GetComponent<Image>().color = Color.red;
        }

        /**
        // loop through the slots and check answer
        for (int i = 0; i < currentQuestion.transform.childCount; i++)
        {
            GameObject elementInCell = currentQuestion.transform.GetChild(i).GetChild(0).gameObject;
            if (elementInCell.tag == currentQuestionObject.answer[i])
            {
                plusScore();
                elementInCell.transform.GetComponent<Image>().color = Color.green;
            }
            else
            {
                elementInCell.transform.GetComponent<Image>().color = Color.red;
            }
        }
         **/
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

    public void funFactPanelTouched()
    {
        if (numberOfQuestionsAnswred < 5)
        {
            Destroy(currentQuestion);
            Destroy(currentQuestionAnswer);
            questions.Remove(currentQuestionObject);
            instantiateRandomQuestionToDisplay();
        }
        else
        {
            // go to game over scene 
            PlayerPrefs.SetString("Game_Title", gameTitle.text);
            float percetange = Mathf.Round((score / totalNumberOfCells) * 100) / 100f;
            PlayerPrefs.SetInt("Score", score);
            PlayerPrefs.SetFloat("Percentage", percetange);

            switch (gameLevel)
            {
                case 1:
                    if (!PlayerPrefs.HasKey("Level_One_High_Percentage"))
                    {
                        PlayerPrefs.SetFloat("Level_One_High_Percentage", 0f);
                    }
                    break;
                case 3:
                    if (!PlayerPrefs.HasKey("Level_Three_High_Percentage"))
                    {
                        PlayerPrefs.SetFloat("Level_Three_High_Percentage", 0f);
                    }
                    break;
                default:
                    break;
            }
            SceneManager.LoadScene("Game_Over_Scene");
        }
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



}











