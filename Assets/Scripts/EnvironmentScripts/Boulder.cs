using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//THIS SCRIPT IS ATTACHED TO THE BOULDER
//ONCE THE BOULDER REACHES THE "BOTTOM" WHERE IT SHOULD DISAPPEAR, IT DISAPPEARS

public class Boulder : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D col){
		if (col.gameObject.CompareTag("BoulderBottom")){
			BoulderManager.instance.reset = true;
			Debug.Log ("disappear");
		}
	}

}
