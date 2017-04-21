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
	[SerializeField] GameObject[] questionAnswerObjects;
	[SerializeField] Canvas canvas;
	[SerializeField] Text gameTitle;
	[SerializeField] Text scoreNumberLabel;
	[SerializeField] Button nextButton;
	[SerializeField] GameObject helpPanel;
	[SerializeField] int gameLevel;

	private List<QuestionLevel5> questions = new List<QuestionLevel5>();
	private JsonData questionData;

	private int score = 0;
	private GameObject currentQuestion;
	private GameObject currentQuestionAnswer;
	private QuestionLevel5 currentQuestionObject;
	private gameStatus currentStatus = gameStatus.InGame;
	public gameStatus CurrentStatus {
		get {
			return currentStatus;
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
			//Debug.Log("Question ID: " + (int)questionData[i]["id"]);
			questions.Add(new QuestionLevel5((int)questionData[i]["id"],(int)questionData[i]["level"],(string)questionData[i]["code"],(string)questionData[i]["name"],(int)questionData[i]["numberOfButtons"],questionObjects[i],questionAnswerObjects[i],convertArray(questionData,i)));
		}
	}

	// pick a random question from the List<Question> and display it
	void instantiateRandomQuestionToDisplay() {
		int randomNum = Random.Range (0, questions.Count); // random a question
		currentQuestionObject = questions [randomNum]; 
		currentQuestion = Instantiate (currentQuestionObject.gameObject, canvas.transform, false);	// instantiate the prefab
		currentQuestionAnswer = Instantiate (currentQuestionObject.answerObject, canvas.transform, false);

		// change the game status and deactivate the answer button
		currentStatus = gameStatus.InGame;
	}


	// helper methos to convert json array to normal List
	List<string> convertArray(JsonData ary, int index) {
		List<string> temp = new List<string>();
		for(int i = 0; i < (int)ary[index]["numberOfButtons"]; i++) {
			temp.Add((string)ary[index]["answer"][i]);
		}
		return temp;
	}

	// switch to the main scene
	public void homeButtonPressed()
	{
		SceneManager.LoadScene("MainScene");
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



}











