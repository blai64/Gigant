using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class State : MonoBehaviour {
	public float timer;
	public Image testbar;
	private string sceneName;

	public static State instance;

	// Use this for initialization
	void Start () {
		
		Scene currentScene = SceneManager.GetActiveScene ();
		sceneName = currentScene.name;
		if (sceneName == "Prototype_Scene1") {
			timer = 0f;
		} else{
			timer = PlayerPrefs.GetFloat ("health");
		}
		//Debug.Log ("initial value of timer is " + timer);
		//
		//timer = 0f;
	}

	void Awake(){
		if (instance == null)
			instance = this;
	}

	// Update is called once per frame
	void Update () {
		//Debug.Log (timer);

		timer = timer + 0.001f;
		testbar.fillAmount = timer;

		//here can add certain condition.
		//PlayerPrefs.SetFloat ("health", timer);

		// Four parameters here: next scene name, fade out & fade in time,
		// and fade color.
		//if (timer >= 0.2 && sceneName != "zao2") {
		//	AutoFade.LoadScene("zao2" ,2,2,Color.yellow);
		//}
		
	}
}
