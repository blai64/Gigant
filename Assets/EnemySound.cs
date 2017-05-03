using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySound : MonoBehaviour {

	private SoundManager sm;

	void Start () {
		sm = SoundManager.instance;
	}

	public void FootStep() {
		sm.PlaySound ("enemy step");
	}

	public void Attack() {
		sm.PlaySound ("enemy attack");
	}
}
