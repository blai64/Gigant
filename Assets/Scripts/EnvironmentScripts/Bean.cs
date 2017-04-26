using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bean : MonoBehaviour {

	private Animator anim;
	private BeanBush bb;

	void Awake () {
		anim = GetComponent<Animator> ();
		bb = transform.parent.gameObject.GetComponent<BeanBush> ();
	}

	public void Grow() {
		anim.SetTrigger ("isGrowing");
	}

	public void Disappear() {
		anim.SetTrigger ("isDisappearing");
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.CompareTag("Player")) {
			if (PlayerController.instance.beanCount + 1 <= PlayerController.instance.maxBeans) {
				PlayerController.instance.beanCount++;
				Disappear ();
				bb.PickBean ();
			}
		}
	}
}
