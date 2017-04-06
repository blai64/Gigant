using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyBehavior : BaseEnemyBehavior {

	override protected IEnumerator Activate(){
		Debug.Log ("Do Basic Enemy Activation Stuff");
		yield return new WaitForSeconds (0.5f);
		StartCoroutine (base.Activate ());
	}
}
