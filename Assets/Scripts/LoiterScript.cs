using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoiterScript : MonoBehaviour {
	public GameObject exclamationPointPrefab;

	private Vector2 pastPosition;
	private Vector2 currentPosition;

	private float limit = 4;
	private float distance;
	private float loiterTimer = 1f;
	private float destroyDelay = 6;

	private bool loiterWarning = false;
	private int exclamationCounter = 0;

	// Use this for initialization
	void Start () {
		pastPosition = PlayerController.instance.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		loiterTimer += Time.deltaTime;
		currentPosition = PlayerController.instance.transform.position;							// If player doesn't move a certain distance within a certain time,
		if (Mathf.Round(loiterTimer) % 10 == 0) {												// the loiter flag will trigger
			distance = (currentPosition - pastPosition).magnitude;
			if (distance < limit) {
				loiterWarning = true;
			}
			loiterTimer = 1f;
			pastPosition = currentPosition;
		}

		if (loiterWarning) {
			LoiterPunisher ();
		}
	}

	void LoiterPunisher(){
		GameObject exclamationPoint = Instantiate (exclamationPointPrefab);
		exclamationPoint.transform.position = new Vector2 (currentPosition.x, PlayerController.instance.transform.position.y + 6);
		destroyDelay -= Time.deltaTime;
		if (Mathf.Round (destroyDelay) % 2 == 0)
			exclamationPoint.SetActive (true);
		else
			exclamationPoint.SetActive (false);
	}
}
