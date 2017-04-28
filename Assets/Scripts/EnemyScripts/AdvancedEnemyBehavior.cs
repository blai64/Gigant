using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedEnemyBehavior : BaseAdvancedEnemyBehavior {
	private int attackCounter;
	private int attackThreshold = 250;
	private float attackDistance = 5f;																					// Alex) changed attackDistance from 6 to 5  4/23
	private float distanceBetweenPlayer;




	public GameObject EnemyHitbox;


	override protected void Update(){
		base.Update ();
		distanceBetweenPlayer = Mathf.Abs (transform.position.x - PlayerController.instance.transform.position.x);			
		if (base.isActive && !base.isAttacking) {															
			attackCounter += 1;																							


			if (attackCounter > attackThreshold && distanceBetweenPlayer < attackDistance) {								// Alex) Made it so enemy only attacks if he has an 
				base.isAttacking = true;																					//       attack charged up and is in range to hit target
				base.anim.SetTrigger ("isAttacking");																		//       4/23
				attackCounter = 0;
			}
		}
	}




	/* void OnTriggerStay2D(Collider2D col){																					// Alex) Enemy knocks down beanstalks 4/23
		if (col.CompareTag ("Beanstalk") && attackCounter > attackThreshold && 
			col.gameObject.GetComponent<BeanstalkScript>().FullyGrown() ) {
			base.isAttacking = true;																					
			base.anim.SetTrigger ("isAttacking");																		
			attackCounter = 0;
			col.gameObject.GetComponent<BeanstalkScript> ().EnemyCutBeanstalk (col.gameObject);
		}
	} /*
		
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
		//GameObject temp = Instantiate (EnemyHitbox, transform.position, transform.rotation);

		//Vector3 oldPos = temp.transform.position;
		//Vector3 newPos = oldPos + (base.direction * attackDistance * Vector3.right);

		while (t < 1.0) {
			t += Time.deltaTime / 0.5f;

			//temp.transform.position = Vector3.Lerp (oldPos, newPos, t);
			yield return 0;
		}

		//Destroy (temp);
		EndAttack ();
	}
}
