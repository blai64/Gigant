using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerStay2D(Collider2D col){
		if(col.CompareTag("Beanstalk") && 
			PlayerController.instance.isAttacking &&
			col.gameObject.GetComponent<BeanstalkScript>().FullyGrown()){						
			col.gameObject.GetComponent<BeanstalkScript> ().PlayerCutBeanstalk ();

		}
	}
}
