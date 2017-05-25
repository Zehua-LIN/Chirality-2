using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleHandler : MonoBehaviour {

	public Toggle toggle;
	public ToggleGroup toggleGroup;
	public Toggle[] extraQuestionToggles;
	public Canvas extraQuestionCanvas;
	public static List<Toggle> selectedToggles;
	public Sprite correct;	
	public Sprite wrong;

	public void ToggleAction(){
		if (toggle.isOn) {
			toggle.image.color = new Color (0, 0, 0, 0);
		} else {
			toggle.image.color = new Color(255,255,255,255);
		}
	}


	public void addOrRemoveSelectedToggle() {
		// getting the selected toggles list from Level 5 Question Manager
		selectedToggles = Level5QuestionManager.GetSelectedTogglesList();

		// if the list does not contain the toggle, add it to the list
		if (!selectedToggles.Contains(toggle)) {
			selectedToggles.Add(toggle);
		} else {
			selectedToggles.Remove(toggle);
		}
		// update qm select toggles
		Level5QuestionManager.UpdateSelectedTogglesList(selectedToggles);
	}

	public void ChangeColour() {

		Toggle toggle = GetComponent<Toggle>();

		extraQuestionCanvas = toggle.GetComponentInParent<Canvas>();
		extraQuestionToggles = extraQuestionCanvas.GetComponentsInChildren<Toggle>();


		// first, make the toggles not interactable anymore
		for (int j = 0; j < extraQuestionToggles.Length; j++) {
			extraQuestionToggles[j].interactable = false;
		}

		// change the colour
		if (toggle.tag == "correctToggleEQ5") {
			toggle.image.sprite = correct;
			Level5QuestionManager.addEQuestionScore ();
		} else {
			toggle.image.sprite = wrong;
		}
	    Level5QuestionManager.updateScore ();
		Level5QuestionManager.changeToEToggleSelected ();
	}

}
