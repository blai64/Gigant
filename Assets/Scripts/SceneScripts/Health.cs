using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour {
	public static Health instance;

	public GameObject life1;
	public GameObject life2;
	public GameObject life3;

	public int hp;
	private bool reset; //use boolean variable reset to check if health is reset.

	// Use this for initialization
	void Start () {
		if (SceneManager.GetActiveScene ().name == "Zao_tutorial")
			reset = true;
		//if (reset) {
		//	hp = 3;
		//} 
		//else{
			hp = PlayerPrefs.GetInt ("health");
		//}
	}

	void Awake(){
		if (instance == null)
			instance = this;
	}

	// Update is called once per frame
	void Update () {
		
		//Debug.Log (timer);
		if (hp == 3) {
			life1.SetActive (true);
			life2.SetActive (true);
			life3.SetActive (true);
		} else if (hp == 2) {
			life1.SetActive (true);
			life2.SetActive (true);
			life3.SetActive (false);
		} else if (hp == 1) {
			life1.SetActive (true);
			life2.SetActive (false);
			life3.SetActive (false);
		} else if (hp == 0) {
			life1.SetActive (false);
			life2.SetActive (false);
			life3.SetActive (false);
		}


		//here can add certain condition.
		PlayerPrefs.SetInt ("health", hp);
	}
}
