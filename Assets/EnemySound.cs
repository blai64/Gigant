using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySound : MonoBehaviour {

	private SoundManager sm;
	private SpriteRenderer sr;

	void Start () {
		sm = SoundManager.instance;
		sr = GetComponent<SpriteRenderer> ();
	}

	public void FootStep() {
		if (sr.isVisible) {
			sm.PlaySound ("enemy step");
		}
	}

	public void Attack() {
		if (sr.isVisible) {
			sm.PlaySound ("enemy attack");
		}
	}
}
