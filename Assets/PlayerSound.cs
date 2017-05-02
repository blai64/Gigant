using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour {

	private SoundManager sm;

	void Start () {
		sm = SoundManager.instance;
	}

	public void Footstep1() {
		sm.PlaySound("footstep 1");
	}

	public void FootStep2() {
		sm.PlaySound("footstep 2");
	}
}
