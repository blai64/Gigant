using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour {

	public Transform canvas;
	private bool paused;

	void Start() {
		paused = false;
		MakeActive(paused);
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Pause();
		}
	}

	public void Pause() {

		if (!paused) {
			MakeActive(true);
		} else {
			MakeActive(false);
		}
	}

	public bool isPaused() {
		return paused;
	}

	private void MakeActive(bool active) {
		paused = active;
		canvas.gameObject.GetComponent<Canvas>().enabled = active;
		int t = active ? 0 : 1;
		Time.timeScale = t;
	}
}
