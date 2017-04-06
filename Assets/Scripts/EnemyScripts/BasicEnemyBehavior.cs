using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyBehavior : BaseEnemyBehavior {

	override protected IEnumerator Activate(){
		Debug.Log ("Do Basic Enemy Activation Stuff");
		while (transform.localScale.x < 2) {
			transform.localScale += new Vector3 (0.01f, 0.01f, 0);
			yield return 0;
		}


		StartCoroutine (base.Activate ());
	}
}
