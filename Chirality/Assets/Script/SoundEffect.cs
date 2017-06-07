using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour {

	[SerializeField] AudioClip soundEffectClip;
	private bool soundEffectIsOn;

	// Use this for initialization
	void Start () {
		soundEffectIsOn = PlayerPrefsX.GetBool("Sound_Effect_Toggle",false);
	}
	
	// Update is called once per frame
	void Update () {
		if(soundEffectIsOn) {
			if(Input.anyKeyDown) {
				GetComponent<AudioSource>().PlayOneShot(soundEffectClip);
			}
		}
	}
}
