using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyBehavior : BaseEnemyBehavior {
	private int attackCounter;
	private int attackThreshold = 250;
	private float attackDistance = 5f;

	public GameObject EnemyHitbox;

	override protected void Update(){
		base.Update ();
		if (base.isActive && !base.isDead && !base.isAttacking) {
			attackCounter += 1;


			if (attackCounter > attackThreshold) {
				base.isAttacking = true;
				base.anim.SetTrigger ("isAttacking");
				attackCounter = 0;
			}

		}


	}
		
	/*
	virtual protected void StartAttack(){
		base.isAttacking = true;
		
		DoAttack();
	}

	//ANIMATION EVENT should call this when 	the animation is over
	virtual protected void EndAttack(){
		//do any clean up here.
		base.isAttacking = false;
	}
*/
	//ANIMATION EVENT should call this to do damage
	override public void DoAttack(){
		//attack logic, e.g. creating hitbox in front of enemy, throwing object, etc
		StartCoroutine(BasicAttack());
	}

	IEnumerator BasicAttack(){
		float t = 0f; 
		GameObject temp = Instantiate (EnemyHitbox, transform.position, transform.rotation);

		Vector3 oldPos = temp.transform.position;
		Vector3 newPos = oldPos + (base.direction * attackDistance * Vector3.right);

		while (t < 1.0) {
			t += Time.deltaTime / 0.5f;

			temp.transform.position = Vector3.Lerp (oldPos, newPos, t);
			yield return 0;
		}

		Destroy (temp);
		EndAttack ();
	}


}
