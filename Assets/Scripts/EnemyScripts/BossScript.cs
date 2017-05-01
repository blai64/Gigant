using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossScript : MonoBehaviour {
	public GameObject CSManager;
	private CutsceneManager csm;

	private int stage; 

	public int health;
	public Image bosshealth;
	public GameObject healthFrame;

	void Start(){
		csm = CSManager.GetComponent<CutsceneManager> ();
		health = 10; 
		stage = 0;
	}

	void Update(){
		bosshealth.fillAmount = health / 10.0f;
		Debug.Log (bosshealth.fillAmount);
		changeColor ();
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.CompareTag ("Weapon") && PlayerController.instance.isAttacking) {
			GetDamaged (1);
		}
	}



	public void GetDamaged (int damage){
		health -= damage;
		Debug.Log (health);

		if (health <= 0) {
			switch (stage) {
			case 0:
				bosshealth.gameObject.SetActive (false);
				healthFrame.SetActive (false);
				StartCoroutine (csm.StartCutsceneBossP2 ());
				stage++;
				health = 10;
				PlayerController.instance.isAttacking = false;
				break;
			case 1: 
				bosshealth.gameObject.SetActive (true);
				healthFrame.SetActive (true);
				StartCoroutine (csm.StartCutsceneBossP4 ());
				stage++;
				break;
			default: 
				Debug.Log ("Shouldnt get here");
				break;

			}

		}
			

	}

	private void changeColor(){
		if (health >= 7) {
			bosshealth.color = new Color32 (0, 255, 0, 255);
		} else if (health >= 5) {
			bosshealth.color = new Color32 (255, 255, 0, 255);
		} else if (health >= 3) {
			bosshealth.color = new Color32 (255, 128, 0, 255);
		} else {
			bosshealth.color = new Color32 (255, 0, 0, 255);
		}

	}

}
