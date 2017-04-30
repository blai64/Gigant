using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour {

	public GameObject CSManager;
	private CutsceneManager csm;
	private Text textBox;

	private string text = "";
	private List<string> textToShow;
	private int index = 0;
	private bool hasStarted = false;
	private bool isPlaying = false;

	private IEnumerator curRoutine;
	void Awake() {
		csm = CSManager.GetComponent<CutsceneManager> ();
		textBox = transform.Find ("Panel/Text").GetComponent<Text> ();
	}

	void Update() {
		if (hasStarted && !isPlaying && Input.anyKeyDown) {
			if (index >= textToShow.Count) {
				StartCoroutine (csm.MoveOn ());
			} else {
				StartCoroutine (PlayNext ());
			}
		}
		else if (hasStarted && isPlaying && Input.anyKeyDown){
			if (curRoutine != null) {
				StopCoroutine (curRoutine);
			}
			textBox.text = text; 
			isPlaying = false; 
			index++;
		}
	}

	public void Play(List<string> toShow) {
		textToShow = toShow;
		hasStarted = true;
		index = 0;
		StartCoroutine (PlayNext ());
	}

	public IEnumerator PlayNext() {

		isPlaying = true;
		text = textToShow [index];

		curRoutine = AnimateText ();

		yield return StartCoroutine(curRoutine);

		index++;
		isPlaying = false;
	}

	public IEnumerator AnimateText() {
		for (int i = 0; i < text.Length; i++) {
			//check for special text tag
			if (text[i] == '<'){
				int count = 2;
				while (count > 0) {
					if (text [i] == '>')
						count--;
					i++;
				}
			}
			textBox.text = text.Substring (0, i);
			yield return new WaitForSeconds (0.05f);
		}
	}
}

