using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour {

	public Text txt;

	private bool isFlipping;

	// Use this for initialization
	void Start () {
//		isFlipping = AutoFlip.instance.isFlipping;
	}
	
	// Update is called once per frame
	void Update () {
		isFlipping = AutoFlip.instance.isFlipping;
		if (isFlipping) {
			txt.gameObject.SetActive (false);
		} else {
			txt.gameObject.SetActive (true);
		}
	}
}
