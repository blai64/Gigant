using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PrologueButton : MonoBehaviour {
	public Button yourBtn;

	private string currentScene;


	void Start(){
		currentScene = SceneManager.GetActiveScene ().name;
		Button btn = yourBtn.GetComponent<Button> ();
		btn.onClick.AddListener (update);
	}

	void update(){
		if (currentScene == "Prologue") {
			if (AutoFlip.instance.ControledBook.currentPage == 6) {
				SceneManager.LoadScene ("Tutorial");
			} else {
				AutoFlip.instance.FlipRightPage ();
			}

		} else if (currentScene == "Epilogue") {
			if (AutoFlip.instance.ControledBook.currentPage == 4) {
				SceneManager.LoadScene ("Splash");
			} else {
				AutoFlip.instance.FlipRightPage ();
			}

		}
	}
}