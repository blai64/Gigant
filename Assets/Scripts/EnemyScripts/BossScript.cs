using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossScript : MonoBehaviour {
	public static BossScript instance;
	public GameObject CSManager;
	private CutsceneManager csm;

	private int stage; 

	public int Health = 1000;
	private int health;
	public Image bosshealth;
	public GameObject healthFrame;

	void Awake(){
		if (instance == null)
			instance = this;
		else
			Destroy (this);
	}

	void Start() {
		csm = CSManager.GetComponent<CutsceneManager> ();
		health = Health; 
		stage = 0;
	}

	void Update() {
		bosshealth.fillAmount = (float) health / Health;
//		Debug.Log (bosshealth.fillAmount);
		changeColor ();
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.CompareTag ("Weapon") && PlayerController.instance.isAttacking) {
			GetDamaged (1);
		}
	}
		
	public void GetDamaged (int damage) {

		float rand = Random.value;
		if (rand < 0.5) {
			SoundManager.instance.PlaySound ("sword hit 1");
		} else {
			SoundManager.instance.PlaySound ("sword hit 2");
		}

		health -= damage;
		Debug.Log (bosshealth.fillAmount);

		if (bosshealth.fillAmount < 0.9f && stage == 0) {
			//bosshealth.gameObject.SetActive (false);

			StartCoroutine (csm.StartCutsceneBossP2 ());
			PlayerController.instance.isAttacking = false;
			stage++;

		}
		else if (bosshealth.fillAmount < 0.8f && stage == 1) {
			//bosshealth.gameObject.SetActive (true);
			//healthFrame.SetActive (true);
			StartCoroutine (csm.StartCutsceneBossP4 ());
			stage++;
		}
//			switch (stage) {
//			case 0:
//				bosshealth.gameObject.SetActive (false);
//				healthFrame.SetActive (false);
//				StartCoroutine (csm.StartCutsceneBossP2 ());
//				stage++;
////				health = 10;
//				PlayerController.instance.isAttacking = false;
//				break;
//			case 1: 
//				bosshealth.gameObject.SetActive (true);
//				healthFrame.SetActive (true);
//				StartCoroutine (csm.StartCutsceneBossP4 ());
//				stage++;
//				break;
//			default: 
//				Debug.Log ("Shouldnt get here");
//				break;
//
//			}
//
//		}
	}

	private void changeColor(){
		if ((float) health / Health >= 0.7) {
			bosshealth.color = new Color32 (0, 255, 0, 255);
		} else if ((float) health / Health >= 0.5) {
			bosshealth.color = new Color32 (255, 255, 0, 255);
		} else if ((float) health / Health >= 0.3) {
			bosshealth.color = new Color32 (255, 128, 0, 255);
		} else {
			bosshealth.color = new Color32 (255, 0, 0, 255);
		}

	}

}
