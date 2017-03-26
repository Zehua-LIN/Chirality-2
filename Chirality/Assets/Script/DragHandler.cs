using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {


	Vector2 offset;
	bool firstTimeDrag = true;
	
	// this method is called when you start dragging
	public void OnBeginDrag (PointerEventData eventData){
		Debug.Log(QuestionManager.Instance.CurrentStatus);
		if(QuestionManager.Instance.CurrentStatus != gameStatus.InGame) {
			return;
		}
		
		offset = (Vector2)transform.position - eventData.position;

		if (firstTimeDrag) {
			Instantiate(transform.gameObject,transform.parent);
		}
		firstTimeDrag = false;

		transform.localScale = new Vector3(1.3f,1.3f,1.3f);

		GetComponent<CanvasGroup> ().blocksRaycasts = false;

		DropHandler[] dropables = GameObject.FindObjectsOfType<DropHandler> ();
		foreach (DropHandler drop in dropables) {
			// drop.transform.GetComponent<Image>().color = Color.red;
		}
		
	}

	



	
	// this method is called while you are dragging
	public void OnDrag (PointerEventData eventData){
		if(QuestionManager.Instance.CurrentStatus != gameStatus.InGame) {
			return;
		}
		transform.position = eventData.position + offset;
	}

	



	
	// this method is called when you release the mouse/finger
	public void OnEndDrag (PointerEventData eventData){
		if(QuestionManager.Instance.CurrentStatus != gameStatus.InGame) {
			return;
		}

		GetComponent<CanvasGroup> ().blocksRaycasts = true;

		GameObject cell = getCell (eventData);
		if (cell != null) {
			transform.localScale = Vector3.one;
			transform.SetParent (cell.transform);
			transform.position = cell.transform.position;
		} else {
			Destroy (transform.gameObject);
		}
	}


	// a helper method to check if there is a cell underneath
	private GameObject getCell(PointerEventData data) {

		List<RaycastResult> hits = new List<RaycastResult>();
		EventSystem.current.RaycastAll(data,hits);
		GameObject cell = null;
		foreach(RaycastResult result in hits) {
			if (result.gameObject.tag == "Cell") {
				cell = result.gameObject;
				break;
			}
		}
		return cell;
	}
	








}
