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
		tutorialText.Add ("I remember the first time I gave you some <color=green>magic beans</color>\nfor this here cow of yers.");
		tutorialText.Add ("I hear you've come to <color=red>slay the giant</color>!\nYou'll need to climb to the top and reach his head.");

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
		yield return new WaitForSeconds (1.0f);

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

