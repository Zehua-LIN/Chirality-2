using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.IO;
using LitJson;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Level5QuestionManager : MonoBehaviour
{

	public static Level5QuestionManager Instance = null;

	[SerializeField] GameObject[] questionObjects;
	[SerializeField] GameObject[] questionextraObjects;

	[SerializeField] Canvas canvas;
	[SerializeField] Text gameTitle;
	[SerializeField] Text questionName;
	[SerializeField] Button nextButton;
	[SerializeField] GameObject helpPanel;
	[SerializeField] int gameLevel;
	[SerializeField] GameObject AnsPanel;
	[SerializeField] GameObject exitPanel;
	[SerializeField] Text funFactPanelText;
	[SerializeField] GameObject funFactPanel;
	[SerializeField] Button yesButton;
	[SerializeField] Button noButton;
	[SerializeField] Sprite acolourgr;
	[SerializeField] Sprite acolourr;
	[SerializeField] Sprite acolourb;

	private List<QuestionLevel5> questions = new List<QuestionLevel5>();
	private JsonData questionData;
	private int numberOfQuestionsAnswred = 0;
	private int questionNumber;	
	private static int score = 0;
	private static int mainQuestionScore = 0;
	private static Text scoreNumberLabel;
	private static float qstnRoundTtlPossiblePts = 0f;
	private static float qtnRoundScore = 0f;
	private GameObject currentQuestion;
	private GameObject extra;
	private QuestionLevel5 currentQuestionObject;
	private gameStatus currentStatus = gameStatus.InGame;
	public gameStatus CurrentStatus {
		get {
			return currentStatus;
		}
	}
	private List<Toggle> mainQuestionTogglesList = new List<Toggle>();
	private Toggle[] mainQuestionToggles = null;
	private static List<Toggle> mainQuestionSelectedToggles = new List<Toggle>();
	private string extraQuestionAnswer;
	private static bool eToggleSelected = false;

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
		//PlayerPrefs.DeleteAll();


		setUpHelpPanel();

		AnsPanel.SetActive (false);

		string path = readJsonData(gameLevel);	
		questionData = JsonMapper.ToObject(File.ReadAllText(path));

		score = 0;
		qstnRoundTtlPossiblePts = 0;

		loadQuestions();
		instantiateRandomQuestionToDisplay();	


	}


	// create the Question objects from the Questions.json and append them to the List<Question>
	void loadQuestions()
	{
		for (int i = 0; i < questionData.Count; i++)
		{
			//Debug.Log("Question ID: " + (int)questionData[i]["id"]);
			questions.Add(new QuestionLevel5((int)questionData[i]["id"],(int)questionData[i]["level"],(string)questionData[i]["code"],(string)questionData[i]["name"],(int)questionData[i]["numberOfAns"], questionObjects[i], questionextraObjects[i],convertArray(questionData,i), (int)questionData[i]["numberCrToggles"], (string)questionData[i]["facts"][Random.Range(0,10)]));
		}
	}

	// pick a random question from the List<Question> and display it
	void instantiateRandomQuestionToDisplay() {
		int randomNum = Random.Range (0, questions.Count); // random a question
		currentQuestionObject = questions [randomNum]; 
		currentQuestion = Instantiate (currentQuestionObject.gameObject, canvas.transform, false);	// instantiate the prefab
		questionName.text = currentQuestionObject.name;

		// change the game status and deactivate the answer button
		currentStatus = gameStatus.InGame;

		mainQuestionSelectedToggles = new List<Toggle>();
		mainQuestionScore = 0;
		calculateQtnTotalPossiblePts();

		getQuestionToggles();
		eToggleSelected = false;

		// getting the score number label
		Text[] textelements = canvas.GetComponentsInChildren<Text>();
		for (int i = 0; i < textelements.Length; i++)
		{
			if (textelements[i].tag.Equals("scorenumberlabel"))
			{
				scoreNumberLabel = textelements[i];
			}
		}
		updateScore();
	}



	public void getQuestionToggles()
	{
		// get all the toggles in the main canvas and store them in an array
		mainQuestionToggles = canvas.GetComponentsInChildren<Toggle>();
		// toggles array to list
		for (int a = 0; a < mainQuestionToggles.Length; a++)
		{
			mainQuestionTogglesList.Add(mainQuestionToggles[a]);
		}
	}

	// helper methos to convert json array to normal List
	List<string> convertArray(JsonData ary, int index) {
		List<string> temp = new List<string>();
		for(int i = 0; i < (int)ary[index]["numberOfAns"]; i++) {
			temp.Add((string)ary[index]["answer"][i]);
		}
		return temp;
	}

	public static List<Toggle> GetSelectedTogglesList()
	{
		return mainQuestionSelectedToggles;
	}

	public static void UpdateSelectedTogglesList(List<Toggle> list)
	{
		mainQuestionSelectedToggles = list;
		Toggle[] aa = mainQuestionSelectedToggles.ToArray();
		int nn = aa.Length;
	}

	public void checkMainQuestionAnswer() {
		currentStatus = gameStatus.InCheck;

		// first, make the toggles not interactable anymore
		for (int jj = 0; jj < mainQuestionToggles.Length; jj++) {
			mainQuestionToggles[jj].enabled = false;
		}

		// if no toggle should be selected and none was selected
		if (currentQuestionObject.numberCrToggles == 0)
		{
			Toggle[] arrayMainQuestionSelectedToggles = mainQuestionSelectedToggles.ToArray();
			if (arrayMainQuestionSelectedToggles.Length == 0)
			{
				mainQuestionScore += 2;
			}
		}

		// for each toggle in the canvas
		for (int j = 0; j < mainQuestionToggles.Length; j++) {
			Image[] toggleImages = mainQuestionToggles[j].GetComponentsInChildren<Image>();
			Image imagecm = toggleImages[0];

			for (int i = 0; i < toggleImages.Length; i++) {
				if (toggleImages[i].tag.Equals("cm")) {
					imagecm = toggleImages[i];
				}
			}

			// check if it should be selected
			if (currentQuestionObject.answer.Contains(mainQuestionToggles[j].tag)) {
				// check if it was selected
				if (mainQuestionSelectedToggles.Contains(mainQuestionToggles[j])) {
					imagecm.sprite = acolourgr;
					// correct answer gives 2 points to the main question score - not to the overall score
					mainQuestionScore += 2;
				} else {
					for (int e = 0; e < toggleImages.Length; e++) {
						if (toggleImages[e].tag.Equals("bckg")) {
							imagecm = toggleImages[e];
						}
					}
					imagecm.sprite = acolourb;
					// missing answer removes 1 point from the main question score - not from the overall score
					mainQuestionScore--;
				}

			}
			else {
				// check if this toggle, that is not a right answer, was selected
				if (mainQuestionSelectedToggles.Contains(mainQuestionToggles[j])) {
					imagecm.sprite = acolourr;
					// incorrect answer removes 1 point from the main question score - not from the overall score
					mainQuestionScore--;
				} else {
					mainQuestionToggles[j].gameObject.SetActive(false);
				}
			}
		}

	}

	public void nextButtonPressed() {
		if (currentStatus == gameStatus.InGame) {
			checkMainQuestionAnswer ();
			displayAnsPanel ();
			addMainQuestionScore ();
			updateScore ();
		} else if (currentStatus == gameStatus.InCheck) {
			for (int j = 0; j < mainQuestionToggles.Length; j++) {
				Image[] toggleImages = mainQuestionToggles [j].GetComponentsInChildren<Image> ();
				Image imagecm = toggleImages [0];
				if (currentQuestionObject.answer.Contains (mainQuestionToggles [j].tag)) {
					if (mainQuestionSelectedToggles.Contains (mainQuestionToggles [j])) {
						imagecm.sprite = acolourgr;
					} else if (imagecm.sprite == acolourb) {
						imagecm.sprite = acolourgr;
					} 
				} else {
					if (mainQuestionSelectedToggles.Contains (mainQuestionToggles [j])) {
						mainQuestionToggles [j].gameObject.SetActive (false);
					}
				} 
			}
			AnsPanel.SetActive (false);
			extra = Instantiate (currentQuestionObject.extraObject, canvas.transform, false);
			numberOfQuestionsAnswred += 1;	// to keep track of how many questions have been answered
			currentStatus = gameStatus.InExtra;
		} else if (currentStatus == gameStatus.InExtra) {
			if (eToggleSelected) {
				displyFunFact ();
			}
		} else {
			displayQuestion ();

		}

	}

	void displyFunFact() {
		funFactPanelText.text = currentQuestionObject.funFact;
		funFactPanel.SetActive(true);
		currentStatus = gameStatus.InFunFact;
	}




	public void displayQuestion() {
		if (numberOfQuestionsAnswred < 5) {
			Destroy (currentQuestion);
			currentQuestion.SetActive (false);
			Destroy (extra);
			extra.SetActive (false);
			funFactPanel.SetActive(false);
			questions.Remove (currentQuestionObject);
			instantiateRandomQuestionToDisplay ();
		} else {
			roundScore();
			PlayerPrefs.SetFloat("Percentage", qtnRoundScore);
			if (!PlayerPrefs.HasKey("Level_5_High_Percentage"))
			{
				PlayerPrefs.SetFloat("Level_5_High_Percentage", 0f);
			}
			// go to game over scene 
			PlayerPrefs.SetString ("Game_Title", gameTitle.text);
			SceneManager.LoadScene("Game_Over_Scene");
		}
	}

	void displayAnsPanel() {
		AnsPanel.SetActive(!AnsPanel.activeInHierarchy);
	}

	public static void changeToEToggleSelected()
	{
		eToggleSelected = true;
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

	// if the score for the main question is negative or 0, add no points to the overall score
	// points from the main question are added to the overall score after a main question is played - if the main question score is greater than zero 
	public void addMainQuestionScore()
	{
		if (mainQuestionScore > 0)
		{
			score += mainQuestionScore;
		}
	}

	public static void addEQuestionScore()
	{
		score++;
	}

	public static void updateScore()
	{
		scoreNumberLabel.text = score.ToString();
	}

	public void calculateQtnTotalPossiblePts()
	{
		int totalPossiblePtsForQuestion = 0;

		// the total possible points for a question would be:
		// 1. if the correct answer for the main question is to select no toggles:
		// selecting no toggle in the main question would add 2 points to the overall score, plus the extra question answered correct
		// 2. if not case 1: 
		// all correct toggles are selected, no incorrect toggle is selected 
		// in case 2, each correct toggle would add 2 points to the overall score, plus the extra question answered is correct

		if (currentQuestionObject.numberCrToggles == 0)
		{
			totalPossiblePtsForQuestion = 3; // 2 points for seleceting no toggles in the main question, plus the extra question correct
		}
		else
		{
			totalPossiblePtsForQuestion = (currentQuestionObject.numberCrToggles * 2) + 1; // all correct toggles in the main question(2 points each), plus the extra question correct
		}

		// add the total possible points for this question to the total possible possible for this question round
		qstnRoundTtlPossiblePts += totalPossiblePtsForQuestion;
	}

	public static void roundScore()
	{
		qtnRoundScore = Mathf.Round((score / qstnRoundTtlPossiblePts) * 100) / 100f;
	}

	string readJsonData(int level) {
		string path = "";
		string fileName;

		if (level == 5) {
			fileName = "Level_Five_Questions.json";
		} else {
			fileName = "";
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

	void setUpHelpPanel() {
		switch (gameTitle.text) {
		case "Chiral Carbons":
			if(PlayerPrefsX.GetBool("First_Time_Level_Five",true)) {
				helpPanel.SetActive(true);
				PlayerPrefsX.SetBool("First_Time_Level_Five",false);
			}
			break;
		default:
			break;
		}
	}
}











