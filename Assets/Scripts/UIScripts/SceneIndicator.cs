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
	public Image face;
	public Sprite front;
	public Sprite left;
	public Sprite right;
	public Sprite back;

	private string currentScene;
	void Start () {
		currentScene = SceneManager.GetActiveScene ().name;
		txt = Indicator.GetComponent<Text> ();
		txt.text = "current";
		//face.GetComponent<SpriteRenderer> ().sprite = front;
		face.sprite = front;
	}

	void Update () {
		xPosition = player.transform.position.x;

		if (currentScene == "Tutorial") {
			if (xPosition > 80.0f && xPosition < 160.0f) {
				face.sprite = front;
//				txt.text = currentScene + ": North";
			} else if (xPosition > -35.0f && xPosition < 38.0f) {
				face.sprite = right;
//				txt.text = currentScene + ": West";
			} else if (xPosition > 200.0f && xPosition < 275.0f) {
				face.sprite = left;
//				txt.text = currentScene + ": East";
			} else if (xPosition > 320.0f && xPosition < 395.0f) {
				face.sprite = back;
//				txt.text = currentScene + ": South";
			}
		} else {
			if (xPosition > 80.0f && xPosition < 150.0f) {
				face.sprite = front;
//				txt.text = currentScene + ": North";
			} else if (xPosition > -30.0f && xPosition < 30.0f) {
				face.sprite = right;
//				txt.text = currentScene + ": West";
			} else if (xPosition > 200.0f && xPosition < 265.0f) {
				face.sprite = left;
//				txt.text = currentScene + ": East";
			} else if (xPosition > 325.0f && xPosition < 380.0f) {
				face.sprite = back;
//				txt.text = currentScene + ": South";
			}
		}

	}
}
