using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonMenu : MonoBehaviour {

	private Button btn;
	private string btnName;
	private PauseGame pg;
	public string next;

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
			btn.onClick.AddListener (delegate{ChangeScene("Prologue");});
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

		case "Next":
			btn.onClick.AddListener (delegate{ChangeScene(next);});
			break;

		default:
			btn.onClick.AddListener (delegate{ChangeScene(next);});
			break;
		}
	}

	public void Resume() {
		if (pg != null) pg.Pause ();
	}

	public void Mute() {
		// TODO
	}

	public void ChangeScene(string sceneName) {
		Resume ();
		SceneManager.LoadScene (sceneName);
	}

	public void Quit() {
		Resume ();
		Application.Quit ();
	}
}
