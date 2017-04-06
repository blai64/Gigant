using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour {
	public Sprite seedPrefab;

	public float timeBeforeGrowth;
	public float growthDuration;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.LeftArrow))
			transform.Translate (-.1f, 0, 0);
		if (Input.GetKey (KeyCode.RightArrow))
			transform.Translate (.1f, 0, 0);
		if (Input.GetKeyDown (KeyCode.Space))
			plantSeed ();
	}


	void plantSeed(){
		GameObject seedObject = new GameObject ();
		SpriteRenderer renderer = seedObject.AddComponent<SpriteRenderer> ();
		renderer.sprite = seedPrefab;
	}
}
