using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropHandler : MonoBehaviour, IDropHandler {

	public GameObject element {
		get {
			if (transform.childCount > 0) {
				return transform.GetChild (0).gameObject;
			} else {
				return null;
			}
		}
	}


	// this is called when you drop something, and it is called before OnEndDrag in DragHandler
	public void OnDrop (PointerEventData eventData){

		if(QuestionManager.Instance.CurrentStatus != gameStatus.InGame) {
			return;
		}
		// ExecuteEvents.ExecuteHierarchy<IHasChanged>(gameObject,null,(x,y) => x.hasChanged());

		// if the element you are dropping is itself, do nothing, otherwise destroy the current child, so that you only have one element in the cell.
		if (element) {
			if (element == eventData.pointerDrag.gameObject) {
				return;
			} else {
				Destroy (element);
			}
		}
	}	
}
