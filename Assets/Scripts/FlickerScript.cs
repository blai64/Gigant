using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerScript : MonoBehaviour {
	private float destroyDelay = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		destroyDelay -= Time.deltaTime;
		if (destroyDelay < 0)
			Destroy (this.gameObject);
	}
}
