using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class ClickHandler : MonoBehaviour
{
	public Button blueButton;

	void Start()
	{
		blueButton.onClick.AddListener (ClickButton1);
	}

	void ClickButton1 () {
		blueButton.onClick.RemoveAllListeners();
		blueButton.onClick.AddListener (ClickButton2);
		blueButton.image.color = Color.green;
	}

	void ClickButton2 () {
		blueButton.onClick.RemoveAllListeners();
		blueButton.onClick.AddListener (ClickButton1);
		blueButton.image.color = new Color(26,204,255,0);
	}
}
