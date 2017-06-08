using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.IO;
using LitJson;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// to keep track of the game status
public enum gameStatus
{ InGame, InCheck, InFunFact, InExtra }

public class QuestionManager : MonoBehaviour {

	public static QuestionManager Instance = null;

	[SerializeField] GameObject[] questionObjects;
	[SerializeField] GameObject[] questionAnswerObjects;
	[SerializeField] GameObject deck;
	[SerializeField] Canvas canvas;
	[SerializeField] Text gameTitle;
	[SerializeField] Text questionName;
	[SerializeField] Text scoreNumberLabel;
	[SerializeField] Button displayAnswerButton;
	[SerializeField] GameObject helpPanel;
	[SerializeField] GameObject funFactPanel;
	[SerializeField] GameObject exitPanel;
	[SerializeField] Button yesButton;
	[SerializeField] Button noButton;
	[SerializeField] Text funFactPanelText;
	[SerializeField] int gameLevel;
	[SerializeField] Button nextButton;

	private List<Question> questions = new List<Question>();
	private JsonData questionData;
	private int score = 0;
	private int numberOfQuestionsAnswred = 0;
	private float totalNumberOfCells = 0f;
	private bool soundEffectToggle;
	private bool leftHandMode;
	private GameObject currentQuestion;
	private GameObject currentQuestionAnswer;
	private Question currentQuestionObject;
	private gameStatus currentStatus = gameStatus.InGame;
	public gameStatus CurrentStatus {
		get {
			return currentStatus;
		}
	}
	
	private int Score {
		get{
			return score;
		}
		set{
			score = value;
			scoreNumberLabel.text = score.ToString() + "/" + totalNumberOfCells.ToString();
		}
	}

    private float strtTime;

    void Awake() {
		// singleton
		if (Instance == null) {
			Instance = this;
		}else if (Instance != this) {
			Destroy(gameObject);
		}
	}


	void Start () {
		setUpHelpPanel();
		// for testing
		//PlayerPrefs.DeleteAll();

		leftHandMode = PlayerPrefsX.GetBool("Left_Handle_Toggle",false);
		if(leftHandMode) {
			deck.transform.localPosition = new Vector2(-deck.transform.localPosition.x,0);
		}
		
		string path = readJsonData(gameLevel);	
		questionData = JsonMapper.ToObject(File.ReadAllText(path));

		loadQuestions();
		instantiateRandomQuestionToDisplay();		
		StartCoroutine(configureNextButtonColor());

        strtTime = Time.time;
    }

	void Update() {
		if(Input.GetKey(KeyCode.Escape)) {
			homeButtonPressed();
		}
	}

	// create the Question objects from the Questions.json and append them to the List<Question>
	void loadQuestions() {
		for (int i = 0; i < questionData.Count; i++){
			questions.Add(new Question((int)questionData[i]["id"],(int)questionData[i]["level"],(string)questionData[i]["code"],(string)questionData[i]["name"],(int)questionData[i]["numberOfCells"],(string)questionData[i]["facts"][Random.Range(0,10)],questionObjects[i],questionAnswerObjects[i],convertArray(questionData,i)));
		}
	}

	// pick a random question from the List<Question> and display it
	void instantiateRandomQuestionToDisplay() {
		nextButton.image.color = Color.white;
		int randomNum = Random.Range(0,questions.Count); // random a question
		currentQuestionObject = questions[randomNum]; 
		currentQuestion = Instantiate(currentQuestionObject.gameObject,canvas.transform,false);	// instantiate the prefab
		currentQuestionAnswer = Instantiate(currentQuestionObject.answerObject,canvas.transform,false);
		if(leftHandMode) {
			currentQuestion.transform.localPosition = new Vector2(-currentQuestion.transform.localPosition.x + 30,0);
			currentQuestionAnswer.transform.localPosition = new Vector2(-currentQuestionAnswer.transform.localPosition.x + 30,0);
		}
		currentQuestionAnswer.SetActive(false);
		questionName.text = currentQuestionObject.name;
		totalNumberOfCells += currentQuestionObject.numberOfCells; // record the number of cells for calculating result
		
		// change the game status and deactivate the answer button
		currentStatus = gameStatus.InGame;
		displayAnswerButton.gameObject.SetActive(false);
	}

	// configure the functions attached to this button based on different game status
	public void nextButtonPressed() {
		if(currentStatus == gameStatus.InGame) {
			checkAnswer();
		}else if(currentStatus == gameStatus.InCheck) {
			displayFunFact();
		}
	}

	void checkAnswer() {
		// check for empty slots, return if there is empty one
		// for(int i = 0; i < currentQuestion.transform.childCount; i++) {
		// 	if(currentQuestion.transform.GetChild(i).childCount == 0) {
		// 		return;
		// 	}
		// }
		if(!checkForEmptyCells()) {
			return;
		}


		// change the game status
		currentStatus = gameStatus.InCheck;
		displayAnswerButton.gameObject.SetActive(true); 
		numberOfQuestionsAnswred += 1;	// to keep track of how many questions have been answered

		// loop through the slots and check answer
		for(int i = 0; i < currentQuestion.transform.childCount; i++) {
			GameObject elementInCell = currentQuestion.transform.GetChild(i).GetChild(0).gameObject;		
			if(elementInCell.tag == currentQuestionObject.answer[i]) {
				plusScore();
				elementInCell.transform.GetComponent<Image>().color = Color.green;
			}else {
				Score += 0;
				elementInCell.transform.GetComponent<Image>().color = Color.red;
			}		
		}
	}

	public bool checkForEmptyCells() {
		int count = 0;
		for(int i = 0; i < currentQuestion.transform.childCount; i++) {
			if(currentQuestion.transform.GetChild(i).childCount != 0) {
				count ++;
			}
		}
		return (count == currentQuestion.transform.childCount);
	}

	void plusScore() {
		Score++;
	}

	// helper methos to convert json array to normal List
	List<string> convertArray(JsonData ary, int index) {
		List<string> temp = new List<string>();
		for(int i = 0; i < (int)ary[index]["numberOfCells"]; i++) {
			temp.Add((string)ary[index]["answer"][i]);
		}
		return temp;
	}

	// switch to the main scene
	public void homeButtonPressed() {
		exitPanel.SetActive(true);
	}

	public void yesButtonPressed() {
		SceneManager.LoadScene("MainScene");
	}

	public void noButtonPressed() {
		exitPanel.SetActive(false);
	}

	public void toggleHelpPanel() {
		helpPanel.SetActive(!helpPanel.activeInHierarchy);
	}

	public void toggleAnswer() {
		currentQuestion.SetActive(!currentQuestion.activeInHierarchy);
		currentQuestionAnswer.SetActive(!currentQuestionAnswer.activeInHierarchy);		
	}

	public void funFactPanelTouched() {
		if(numberOfQuestionsAnswred < 5) {
			Destroy(currentQuestion);
			Destroy(currentQuestionAnswer);
			questions.Remove(currentQuestionObject);
			funFactPanel.SetActive(false);
			instantiateRandomQuestionToDisplay();
		}else {
            // go to game over scene 
            float timeP = Mathf.Round(Time.time - strtTime);

            PlayerPrefs.SetString("Game_Title",gameTitle.text);
			float percetange = Mathf.Round((score/totalNumberOfCells)*100) / 100f;
			PlayerPrefs.SetInt("Score",score);
			PlayerPrefs.SetFloat("Percentage",percetange);

            string day = System.DateTime.Now.ToShortDateString();
            char[] a = new char[1];
            a[0] = '/';
            string[] numbers = day.Split(a);
            string tday = numbers[1] + "/" + numbers[0] + "/" + numbers[2];

            string tscores = "";
            string sttime = "";
            string tsttime = "";
            string days = "";

            switch (gameLevel)
			{
				case 1:
					if(!PlayerPrefs.HasKey("Level_1_High_Percentage")) {
						PlayerPrefs.SetFloat("Level_1_High_Percentage",0f);
                        }

                        if (!PlayerPrefs.HasKey("Level_1_Time"))
                        {
                            PlayerPrefs.SetFloat("Level_1_Time", timeP);
                        }

                        PlayerPrefs.SetFloat("TimeP", timeP);
                        tscores = PlayerPrefs.GetString("Level_1_Percentages");
                        Debug.Log(tscores);
                        tscores += percetange + ",";
                        PlayerPrefs.SetString("Level_1_Percentages", tscores);
                        Debug.Log("aaaa");
                        Debug.Log(PlayerPrefs.GetString("Level_1_Percentages"));

                        sttime = timeP.ToString() + ",";
                        tsttime = PlayerPrefs.GetString("Level_1_Times");
                        tsttime += sttime;
                        PlayerPrefs.SetString("Level_1_Times", tsttime);

                        days = PlayerPrefs.GetString("Level_1_Days");
                        days += tday + ",";
                        PlayerPrefs.SetString("Level_1_Days", days);

                        int tpl = PlayerPrefs.GetInt("Level_1_Times_Pl");
                        tpl++;
                        PlayerPrefs.SetInt("Level_1_Times_Pl", tpl);

                        PlayerPrefs.SetInt("Level_1_Already_Played", 1);

					break;
				case 3:
					if(!PlayerPrefs.HasKey("Level_3_High_Percentage")) {
						PlayerPrefs.SetFloat("Level_3_High_Percentage",0f);
                        }

                        if (!PlayerPrefs.HasKey("Level_3_Time"))
                        {
                            PlayerPrefs.SetFloat("Level_3_Time", timeP);
                        }

                        PlayerPrefs.SetFloat("TimeP", timeP);

                        tscores = PlayerPrefs.GetString("Level_3_Percentages");
                        tscores += percetange + ",";
                        PlayerPrefs.SetString("Level_3_Percentages", tscores);

                        sttime = timeP.ToString() + ",";
                        tsttime = PlayerPrefs.GetString("Level_3_Times");
                        tsttime += sttime;
                        PlayerPrefs.SetString("Level_3_Times", tsttime);

                        days = PlayerPrefs.GetString("Level_3_Days");
                        days += tday + ",";
                        PlayerPrefs.SetString("Level_3_Days", days);

                        int tplt = PlayerPrefs.GetInt("Level_3_Times_Pl");
                        tplt++;
                        PlayerPrefs.SetInt("Level_3_Times_Pl", tplt);

                        PlayerPrefs.SetInt("Level_3_Already_Played", 1);
					break;
				default:
					break;
			}
			SceneManager.LoadScene("Game_Over_Scene");
		}
	}

	void displayFunFact() {
		funFactPanelText.text = currentQuestionObject.funFact;
		funFactPanel.SetActive(true);
		currentStatus = gameStatus.InFunFact;
	}

	string readJsonData(int level) {
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
			default:
				fileName = "";
				break;
		}

		var temp = fileName.Split("."[0]);

		if(Application.platform == RuntimePlatform.Android) {
			string oriPath = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);
			WWW reader = new WWW(oriPath);
			while(!reader.isDone) {}

			string realPath = Application.persistentDataPath + "/" + temp[0];
  			System.IO.File.WriteAllBytes(realPath, reader.bytes);
			path = realPath;
		}else {
			path = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);
		}
		return path;
	}

	IEnumerator configureNextButtonColor() {
		while(true) {
			nextButton.image.color = checkForEmptyCells() ? Color.cyan : Color.white;
			yield return new WaitForSeconds(0.1f);
		}
	}

	void setUpHelpPanel() {
		switch (gameTitle.text) {
			case "Functional Groups":
				if(PlayerPrefsX.GetBool("First_Time_Level_One",true)) {
				helpPanel.SetActive(true);
				PlayerPrefsX.SetBool("First_Time_Level_One",false);
				}
				break;
			case "Intermolecular Forces":
				if(PlayerPrefsX.GetBool("First_Time_Level_Three",true)) {
				helpPanel.SetActive(true);
				PlayerPrefsX.SetBool("First_Time_Level_Three",false);
				}
				break;
			default:
				break;
		}
	}
	
	
}











