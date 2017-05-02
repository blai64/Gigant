using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class SpeechBubble : MonoBehaviour {

	public GameObject CSManager;
	private CutsceneManager csm;
	private Text textBox;

	private string text = "";
	private List<string> textToShow;
	private int index = 0;
	private bool hasStarted = false;
	private bool isPlaying = false;

	private bool isHermit = true;

	private IEnumerator curRoutine;

	void Awake() {
		isHermit = (transform.parent.name.Contains("Hermit"));
		Debug.Log (isHermit);
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
		else if (hasStarted && isPlaying && Input.anyKeyDown) {
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

		if (isHermit) {
			float rand = Random.value;
			if (rand < 0.25) {
				SoundManager.instance.PlaySound ("talking 1");
			} else if (rand < 0.50) {
				SoundManager.instance.PlaySound ("talking 2");
			} else if (rand < 0.75) {
				SoundManager.instance.PlaySound ("talking 3");
			} else {
				SoundManager.instance.PlaySound ("talking 3");
			}
		}

		isPlaying = true;
		text = textToShow [index];

		curRoutine = AnimateText ();

		yield return StartCoroutine(curRoutine);

		index++;
		isPlaying = false;
	}

	public IEnumerator AnimateText() {
		string temp; 
		string remainingText; 
		string regex = "(\\<.*?\\>)";
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

			remainingText = "";
			temp = text.Substring (i, text.Length - i);
			remainingText = Regex.Replace (temp, regex, "");

			textBox.text = text.Substring (0, i) + "<color=#00000000>" + remainingText + "</color>";
			yield return new WaitForSeconds (0.05f);
		}
	}
}

