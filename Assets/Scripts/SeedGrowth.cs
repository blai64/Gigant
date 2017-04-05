using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedGrowth : MonoBehaviour {
	bool grown;
	// Use this for initialization
	void Start () {
		grown = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.localScale.x < .5f) {
			transform.localScale += new Vector3 (.05f, .1f,0);
		} else
			grown = true;
	}
}
