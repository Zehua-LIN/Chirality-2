using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleHandler : MonoBehaviour {

	Toggle toggle;

	void Start () {
		toggle = GetComponent<Toggle>();
	}

	void Update () {
		//		Debug.Log(toggle.isOn);
	}

	public void ChangeToggle ()
	{
		Debug.Log("Toggle is clicked");
	}
}
