using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// to keep track of the game status
public enum gameStatus
{ InGame, InHelp, InCheck, InAnswer }

public class QuestionManager : MonoBehaviour {

	public static QuestionManager Instance = null;

	private List<Question> questions = new List<Question>();
	private JsonData questionData;
	[SerializeField] GameObject[] questionObjects;
	[SerializeField] Canvas canvas;
	[SerializeField] Text questionName;
	[SerializeField] Text scoreNumberLabel;
	[SerializeField] Button displayAnswerButton;
	[SerializeField] GameObject helpPanel;

	private int score = 0;
	private GameObject currentQuestion;
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
			scoreNumberLabel.text = score.ToString();
		}
	}

	void Awake() {
		// singleton
		if (Instance == null) {
			Instance = this;
		}else if (Instance != this) {
			Destroy(gameObject);
		}
	}


	void Start () {
		helpPanel.SetActive(false);
		displayAnswerButton.gameObject.SetActive(false);

		// Debug.Log(Application.persistentDataPath);
		// Debug.Log(Application.dataPath);
		questionData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/JsonDatabase/Questions.json"));
		loadQuestions();
		
		randomQuestionToDisplay();		
	}

	// create the Question objects from the Questions.json and append them to the List<Question>
	void loadQuestions() {
		for (int i = 0; i < questionData.Count; i++){
			questions.Add(new Question((int)questionData[i]["id"],(int)questionData[i]["level"],(string)questionData[i]["code"],(string)questionData[i]["name"],(int)questionData[i]["numberOfCells"],questionObjects[i],convertArray(questionData,i)));
		}
	}

	// pick a random question from the List<Question> and display it
	void randomQuestionToDisplay() {
		int randomNum = Random.Range(0,questions.Count);
		currentQuestion = Instantiate(questions[randomNum].gameObj,canvas.transform,false);
		currentQuestionObject = questions[randomNum]; 
		questionName.text = questions[randomNum].name;
		
		// change the game status and deactivate the answer button
		currentStatus = gameStatus.InGame;
		displayAnswerButton.gameObject.SetActive(false);
	}

	// configure the functions attached to this button based on different game status
	public void nextButtonPressed() {
		if(currentStatus == gameStatus.InGame) {
			checkAnswer();
		}else if(currentStatus == gameStatus.InCheck) {
			Destroy(currentQuestion);
			questions.Remove(currentQuestionObject);
			randomQuestionToDisplay();
		}else if(currentStatus == gameStatus.InHelp) {

		}else if (currentStatus == gameStatus.InAnswer) {

		}
	}




	void checkAnswer() {
		// check for empty slots, return if there is empty one
		for(int i = 0; i < currentQuestion.transform.childCount; i++) {
			if(currentQuestion.transform.GetChild(i).childCount == 0) {
				return;
			}
		}

		// change the game status
		currentStatus = gameStatus.InCheck;
		displayAnswerButton.gameObject.SetActive(true);

		// loop through the slots and check answer
		for(int i = 0; i < currentQuestion.transform.childCount; i++) {
			GameObject elementInCell = currentQuestion.transform.GetChild(i).GetChild(0).gameObject;		
			if(elementInCell.tag == currentQuestionObject.answer[i]) {
				plusScore();
				elementInCell.transform.GetComponent<Image>().color = Color.green;
			}else {
				elementInCell.transform.GetComponent<Image>().color = Color.red;
			}		
		}
	}

	void plusScore() {
		Score++;
	}

	// helper methos to convert json array to normal array
	List<string> convertArray(JsonData ary, int index) {
		List<string> temp = new List<string>();
		for(int i = 0; i < (int)ary[index]["numberOfCells"]; i++) {
			temp.Add((string)ary[index]["answer"][i]);
		}
		return temp;
	}

	// switch to the main scene
	public void homeButtonPressed() {
		SceneManager.LoadScene("MainScene");
	}

	public void toggleHelpPanel() {
		helpPanel.SetActive(!helpPanel.activeInHierarchy);
	}
	
}











