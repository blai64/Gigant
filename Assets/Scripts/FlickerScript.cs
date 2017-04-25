using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerScript : MonoBehaviour {
	private float destroyDelay = 6;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		destroyDelay -= Time.deltaTime;
		print (Mathf.Round (destroyDelay));
		if (Mathf.Round (destroyDelay) % 2 == 0)
			this.gameObject.SetActive (true);
		else
			this.gameObject.SetActive (false);
	}
}
