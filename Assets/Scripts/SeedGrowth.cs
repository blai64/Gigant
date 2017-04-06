using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedGrowth : MonoBehaviour {
	
	bool grown;

	void Start () {
		grown = false;
	}

	void Update () {
		if (transform.localScale.x < .5f) {
			transform.localScale += new Vector3 (.05f, .1f,0);
		} else
			grown = true;
	}
}
