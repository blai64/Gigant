using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyBehavior : BaseEnemyBehavior {

	//TODO: deprecated
	override protected IEnumerator Activate(){
		Debug.Log ("Do Basic Enemy Activation Stuff");
		while (transform.localScale.x < 0.5) {
			transform.localScale += new Vector3 (0.01f, 0.01f, 0);
			yield return 0;
		}

		StartCoroutine (base.Activate ());
	}

	virtual protected void StartAttack(){
		base.isAttacking = true;
		//start animation
	}

	virtual protected void EndAttack(){
		//do any clean up here.
		base.isAttacking = false;
	}

	//ANIMATION EVENT should call this to do damage
	public void DoAttack(){
		//attack logic, e.g. creating hitbox in front of enemy, throwing object, etc
	}


}
