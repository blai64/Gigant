using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneIndicator : MonoBehaviour {

	public GameObject player;
	public GameObject Indicator;
	private Text txt;
	private float xPosition;

	private string currentScene;
	void Start () {
		currentScene = SceneManager.GetActiveScene ().name;
		txt = Indicator.GetComponent<Text> ();
		txt.text = currentScene + ": North";
	}

	void Update () {
		xPosition = player.transform.position.x;
		if (xPosition > 80.0f && xPosition < 150.0f) {
			txt.text = currentScene + ": North";
		} else if (xPosition > -30.0f && xPosition < 30.0f) {
			txt.text = currentScene + ": West";
		} else if (xPosition > 200.0f && xPosition < 265.0f) {
			txt.text = currentScene + ": East";
		} else if (xPosition > 325.0f && xPosition < 380.0f) {
			txt.text = currentScene + ": South";
		}
	}
}
