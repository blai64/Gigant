using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour {

	//If I put all boulders in one gameobject, it seems unnatural for
	//all boulders fall together.
	public GameObject boulder1;
	public GameObject boulder2;
	public GameObject boulder3;
	public GameObject boulder4;
	public GameObject boulder5;
	public GameObject boulder6;
	public GameObject boulder7;
	public GameObject boulder8;
	public GameObject boulder9;



	// Use this for initialization
	void Start () {
		boulder1.SetActive (false);
		boulder2.SetActive (false);
		boulder3.SetActive (false);
		boulder4.SetActive (false);
		boulder5.SetActive (false);
		boulder6.SetActive (false);
		boulder7.SetActive (false);
		boulder8.SetActive (false);
		boulder9.SetActive (false);

	}

	void OnTriggerEnter2D (Collider2D col){
		if (col.CompareTag("Player")){
			StartCoroutine (StartFalling ());
		}
	}

	IEnumerator StartFalling(){
		yield return new WaitForSeconds (0.5f);
		boulder1.SetActive (true);
		yield return new WaitForSeconds (0.5f);
		boulder2.SetActive (true);
		yield return new WaitForSeconds (0.5f);
		boulder3.SetActive (true);
		yield return new WaitForSeconds (0.5f);
		boulder4.SetActive (true);
		yield return new WaitForSeconds (0.5f);
		boulder5.SetActive (true);
		yield return new WaitForSeconds (0.5f);
		boulder6.SetActive (true);
		yield return new WaitForSeconds (0.5f);
		boulder7.SetActive (true);
		yield return new WaitForSeconds (0.5f);
		boulder8.SetActive (true);
		yield return new WaitForSeconds (0.5f);
		boulder9.SetActive (true);
	}
}
