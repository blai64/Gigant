using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour {
	private bool hitEnemy = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerStay2D(Collider2D col){
		/*if(col.CompareTag("Beanstalk") && 
			PlayerController.instance.isAttacking &&
			col.gameObject.GetComponent<BeanstalkScript>().FullyGrown()){						
			col.gameObject.GetComponent<BeanstalkScript> ().PlayerCutBeanstalk ();
		}*/
		if (col.CompareTag ("Enemy") && !hitEnemy &&
		   PlayerController.instance.isAttacking &&
			col.gameObject.GetComponent<BaseEnemyBehavior>().health > 0) {
			hitEnemy = true;
			col.gameObject.GetComponent<BaseEnemyBehavior> ().GetDamaged (1);
			col.gameObject.GetComponent<BaseEnemyBehavior> ().RedFlash ();
		}
		else if(col.CompareTag ("AdvancedEnemy") && !hitEnemy &&
			PlayerController.instance.isAttacking &&
			col.gameObject.GetComponent<BaseAdvancedEnemyBehavior>().health > 0) {
			hitEnemy = true;
			col.gameObject.GetComponent<BaseAdvancedEnemyBehavior> ().GetDamaged (1);
			col.gameObject.GetComponent<BaseAdvancedEnemyBehavior> ().RedFlash ();
		}
	}

	void OnTriggerExit2D(Collider2D col){
		if (col.CompareTag ("Enemy") && hitEnemy) {
			hitEnemy = false;
			col.gameObject.GetComponent<BaseEnemyBehavior> ().RevertFromRed ();
		}
		else if (col.CompareTag ("AdvancedEnemy") && hitEnemy) {
			hitEnemy = false;
			col.gameObject.GetComponent<BaseAdvancedEnemyBehavior> ().RevertFromRed ();
		}
	}
}
