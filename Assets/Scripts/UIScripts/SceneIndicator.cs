using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneIndicator : MonoBehaviour {

	public GameObject player;
	public GameObject Indicator;
	private Text txt;
	private float xPosition;
	// Use this for initialization
	void Start () {
		txt = Indicator.GetComponent<Text> ();
		txt.text = "Front";

	}
	
	// Update is called once per frame
	void Update () {
		xPosition = player.transform.position.x;
		if (xPosition > 80.0f && xPosition < 150.0f) {
			txt.text = "Front";
		} else if (xPosition > -30.0f && xPosition < 30.0f) {
			txt.text = "Left";
		} else if (xPosition > 200.0f && xPosition < 265.0f) {
			txt.text = "Right";
		} else if (xPosition > 325.0f && xPosition < 380.0f) {
			txt.text = "Back";
		}
	}
}
