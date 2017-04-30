﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedParentCaller : MonoBehaviour {

	public GameObject parent;

	private BaseAdvancedEnemyBehavior parentScript;

	void Start() {
		parentScript = parent.GetComponent<BaseAdvancedEnemyBehavior> ();
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