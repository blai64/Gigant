using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour {

	public Transform canvas;
	private bool paused = false;

	void Start() {
		canvas.gameObject.SetActive (false);
		paused = false;
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Debug.Log ("pause");
			Pause();
		}
	}

	public void Pause() {

		if (!paused) {
			canvas.gameObject.SetActive (true);
			paused = true;
			Time.timeScale = 0;
		} else {
			canvas.gameObject.SetActive (false);
			paused = false;
			Time.timeScale = 1;

		}
	}

	public bool isPaused() {
		return paused;
	}
}
