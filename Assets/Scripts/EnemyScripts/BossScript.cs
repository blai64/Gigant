using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour {
	public GameObject CSManager;
	private CutsceneManager csm;

	private int stage; 

	public int health;


	void Start(){
		csm = CSManager.GetComponent<CutsceneManager> ();
		health = 10; 
		stage = 0;
	}

	public void GetDamaged (int damage){
		health -= damage;
		Debug.Log (health);

		if (health <= 0) {
			switch (stage) {
			case 0:
				StartCoroutine (csm.StartCutsceneBossP2 ());
				stage++;
				health = 10;
				break;
			case 1: 
				StartCoroutine (csm.StartCutsceneBossP4 ());
				break;
			default: 
				Debug.Log ("Shouldnt get here");
				break;

			}

		}
			

	}

}
