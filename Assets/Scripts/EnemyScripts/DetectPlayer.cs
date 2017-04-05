using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour {

	void onTriggerEnter2D (Collider2D col){
		if (col.CompareTag("Player")){
			Activate ();
		}
	}

	//Use this function to dictate the "awakening" of enemies
	void Activate(){
	
	}

}
