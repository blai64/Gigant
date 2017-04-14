using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentCaller : MonoBehaviour {

	public GameObject parent;

	private BaseEnemyBehavior parentScript;

	void Start() {
		parentScript = parent.GetComponent<BaseEnemyBehavior> ();
	}

	public void Activate(){
		parentScript.Activate ();
	}

	public void DoAttack(){
		parentScript.DoAttack ();
	}

	public void EndAttack(){
		parentScript.EndAttack ();
	}

}
