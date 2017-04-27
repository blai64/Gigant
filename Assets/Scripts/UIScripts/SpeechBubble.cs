using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour {

	public GameObject CSManager;
	private CutsceneManager csm;
	private Text textBox;

	private string text = "";
	private List<string> tutorialText;
	private int index = 0;
	private bool hasStarted = false;
	private bool isPlaying = false;

	void Awake() {

		csm = CSManager.GetComponent<CutsceneManager> ();

		tutorialText = new List<string> ();
		tutorialText.Add ("Why, if it isn't <color=brown>Jack</color>!\nMy have you grown.");
		tutorialText.Add ("I remember the first time I gave you some <color=green>magic</color> <color=green>beans</color>\nfor this here cow of yers.");
		tutorialText.Add ("I guess you're here to\n<color=red>slay</color> <color=red>the</color> <color=red>giant</color>!\nYou'll need to climb to the top and reach his head.");
		tutorialText.Add ("There are some\n<color=green>magic</color> <color=green>bean</color> <color=green>bushes</color> around here.\nWatch out for <color=grey>golems</color> along the way!");

		textBox = transform.Find ("Panel/Text").GetComponent<Text> ();
	}

	void Update() {
		if (hasStarted && !isPlaying && Input.anyKeyDown) {
			if (index >= tutorialText.Count) {
				StartCoroutine (csm.EndCutscene ());
			} else {
				StartCoroutine (PlayNext ());
			}
		}
	}

	public void Play() {
		hasStarted = true;
		StartCoroutine (PlayNext ());
	}

	public IEnumerator PlayNext() {

		isPlaying = true;
		text = tutorialText [index];

		yield return StartCoroutine(AnimateText ());

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

