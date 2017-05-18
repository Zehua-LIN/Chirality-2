using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.IO;
using LitJson;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum gameState
{ InGame, InCheck, InFunFact }

public class Level5QuestionManager : MonoBehaviour
{

	public static Level5QuestionManager Instance = null;

	[SerializeField] GameObject[] questionObjects;
	[SerializeField] GameObject[] questionextraObjects;

	[SerializeField] Canvas canvas;
	[SerializeField] Text gameTitle;
	[SerializeField] Text questionName;
	[SerializeField] Text scoreNumberLabel;
	[SerializeField] Button nextButton;
	[SerializeField] GameObject helpPanel;
	[SerializeField] int gameLevel;
	[SerializeField] GameObject AnsPanel;
	[SerializeField] GameObject exitPanel;
	[SerializeField] Button yesButton;
	[SerializeField] Button noButton;
	[SerializeField] Sprite acolourgr;
	[SerializeField] Sprite acolourr;
	[SerializeField] Sprite acolourb;

	private List<QuestionLevel5> questions = new List<QuestionLevel5>();
	private JsonData questionData;
	private int numberOfQuestionsAnswred = 0;
	private int questionNumber;	
	private int score = 0;
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
		setUpHelpPanel();

		AnsPanel.SetActive (false);

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
			//Debug.Log("Question ID: " + (int)questionData[i]["id"]);
			questions.Add(new QuestionLevel5((int)questionData[i]["id"],(int)questionData[i]["level"],(string)questionData[i]["code"],(string)questionData[i]["name"],(int)questionData[i]["numberOfAns"], questionObjects[i], questionextraObjects[i],convertArray(questionData,i)));
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

		getQuestionToggles();
		eToggleSelected = false;
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

	public void checkMainQuestionAnswer()
	{
		currentStatus = gameStatus.InCheck;

		// first, make the toggles not interactable anymore
		for (int jj = 0; jj < mainQuestionToggles.Length; jj++)
		{
			mainQuestionToggles[jj].enabled = false;
		}

		// for each toggle in the canvas
		for (int j = 0; j < mainQuestionToggles.Length; j++)
		{
			Image[] toggleImages = mainQuestionToggles[j].GetComponentsInChildren<Image>();
			Image imagecm = toggleImages [0];
			Image imageck = toggleImages [0];

			for (int i = 0; i < toggleImages.Length; i++)
			{
				if (toggleImages[i].tag.Equals("cm"))
				{
					imagecm = toggleImages[i];
				}
			}

			// check if it should be selected
			if (currentQuestionObject.answer.Contains(mainQuestionToggles[j].tag))
			{
				// check if it was selected
				if (mainQuestionSelectedToggles.Contains(mainQuestionToggles[j]))
				{
					imagecm.sprite = acolourgr;
				} else {
					for (int e = 0; e < toggleImages.Length; e++)
					{
						if (toggleImages[e].tag.Equals("bckg"))
						{
							imageck = toggleImages[e];
							imageck.enabled = false;
							mainQuestionToggles[j].isOn = true;
							imagecm.sprite = acolourb;
						}
					}
				}
			} else {
				// check if this toggle, that is not a right answer, was selected
				if (mainQuestionSelectedToggles.Contains(mainQuestionToggles[j]))
				{
					imagecm.sprite = acolourr;
				} else
				{
					mainQuestionToggles[j].gameObject.SetActive(false);
				}
			}
		}

	}

	public void nextButtonPressed() {
		if (currentStatus == gameStatus.InGame) {
			checkMainQuestionAnswer ();
			displayAnsPanel ();
		} else if (currentStatus == gameStatus.InCheck) {
			for (int j = 0; j < mainQuestionToggles.Length; j++) {
				if (currentQuestionObject.answer.Contains (mainQuestionToggles [j].tag)) {
					if (mainQuestionSelectedToggles.Contains (mainQuestionToggles [j])) {
						for (int i = 0; i < currentQuestion.transform.childCount; i++) {
							GameObject t = currentQuestion.transform.GetChild (i).GetChild (0).GetChild (0).gameObject;
							t.transform.GetComponent<Image> ().color = Color.green;
						}
					}
				} else {
					if (mainQuestionSelectedToggles.Contains (mainQuestionToggles [j])) {
						mainQuestionToggles [j].gameObject.SetActive (false);
					}
				} 
			}
			AnsPanel.SetActive(false);
			extra = Instantiate(currentQuestionObject.extraObject,canvas.transform,false);
			numberOfQuestionsAnswred += 1;	// to keep track of how many questions have been answered
			currentStatus = gameStatus.InFunFact;
		} else {
			if (eToggleSelected) {
				displayQuestion ();
			}
		}

	}


	void displayQuestion() {
		if (numberOfQuestionsAnswred < 5) {
			//Destroy (currentQuestion);
			currentQuestion.SetActive (false);
			extra.SetActive (false);
			questions.Remove (currentQuestionObject);
			instantiateRandomQuestionToDisplay ();
		} else {
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











