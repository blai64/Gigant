﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour {

	private bool initialized;

	//THIS IS SUPER TEMPORARY, NO IDEA HOW ELSE TO DO IT
	public GameObject speechBubble;

	void OnTriggerEnter2D (Collider2D col){
		if (col.CompareTag("Player") && !initialized){
			initialized = true; 
			StartCoroutine (StartCutscene ());
		}
	}

	IEnumerator StartCutscene(){
		StartCoroutine (CameraManager.instance.Zoom (true));
		yield return StartCoroutine (CameraManager.instance.MoveCinematic (true));

		//Do animations
		yield return new WaitForSeconds (1.0f);

		speechBubble.SetActive (true);
		yield return 0;

		yield return StartCoroutine (speechBubble.GetComponent<SpeechBubble> ().AnimateText ());
		yield return new WaitForSeconds (1.0f);

		speechBubble.SetActive (false);
		yield return 0;

		StartCoroutine (CameraManager.instance.Zoom (false));
		yield return StartCoroutine (CameraManager.instance.MoveCinematic (false));

	}
}
