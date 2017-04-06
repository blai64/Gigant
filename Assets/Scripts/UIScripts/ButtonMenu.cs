using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonMenu : MonoBehaviour {

	private Button btn;
	private string btnName;
	private PauseGame pg;

	void Awake() {
		
		btn = GetComponent<Button> ();
		btnName = btn.GetComponentInChildren<Text>().text;

		GameObject UI = GameObject.Find ("UI Controller");
		if (UI != null) {
			pg = UI.GetComponent<PauseGame> ();
			pg.Pause ();
		}

		switch (btnName) {

		case "Play":
			btn.onClick.AddListener (delegate{ChangeScene("Michelle Scene");});
			break;

		case "Main Menu":
			btn.onClick.AddListener (delegate{ChangeScene("Splash");});
			break;

		case "Instructions":
			btn.onClick.AddListener (delegate{ChangeScene("Instructions");});
			break;

		case "Credits":
			btn.onClick.AddListener (delegate{ChangeScene("Credits");});
			break;

		case "Quit":
			btn.onClick.AddListener (Quit);
			break;

		case "Resume":
			btn.onClick.AddListener (Resume);
			break;

		case "Mute":
			btn.onClick.AddListener (Mute);
			break;

		default:
			break;
		}
	}

	void Resume() {
		pg.Pause ();
	}

	void Mute() {
		// TODO
	}

	void ChangeScene(string sceneName) {
		if (pg != null) Resume ();
		SceneManager.LoadScene (sceneName);
	}

	void Quit() {
		if (pg != null) Resume ();
		Application.Quit ();
	}
}
