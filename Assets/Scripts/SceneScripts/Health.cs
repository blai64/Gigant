using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour {
	public GameObject heart1;
	public GameObject heart2;
	public GameObject heart3;

	private int hp;

	public static Health instance;
	private bool reset; //use boolean variable reset to check if health is reset.

	// Use this for initialization
	void Start () {
		reset = true;
		if (reset) {
			hp = 3;
		} else{
			hp = PlayerPrefs.GetInt ("health");
		}
}

	void Awake(){
		if (instance == null)
			instance = this;
	}

	// Update is called once per frame
	void Update () {
		//Debug.Log (timer);
		if (hp == 3) {
			heart1.SetActive (true);
			heart2.SetActive (true);
			heart3.SetActive (true);
		} else if (hp == 2) {
			heart1.SetActive (true);
			heart2.SetActive (true);
			heart3.SetActive (false);
		} else if (hp == 1) {
			heart1.SetActive (true);
			heart2.SetActive (false);
			heart3.SetActive (false);
		} else if (hp == 0) {
			heart1.SetActive (false);
			heart2.SetActive (false);
			heart3.SetActive (false);
		}


		//here can add certain condition.
		PlayerPrefs.SetInt ("health", hp);
	}
}
