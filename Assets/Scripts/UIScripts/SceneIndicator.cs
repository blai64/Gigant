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
		if (xPosition > 80.0f && xPosition < 155.0f) {
			txt.text = "Front";
		} else if (xPosition > -38.0f && xPosition < 35.0f) {
			txt.text = "Left";
		} else if (xPosition > 200.0f && xPosition < 275.0f) {
			txt.text = "Right";
		} else if (xPosition > 320.0f && xPosition < 400.0f) {
			txt.text = "Back";
		}
	}
}
