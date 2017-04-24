﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeTrigger : MonoBehaviour {
	public Transform targetPositionTransform;
	private Vector3 targetPosition;

	public BoxCollider2D newBounds;

	public bool isTunnel;
	private bool touchingPlayer;

	void Start(){
		targetPosition = targetPositionTransform.position;
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.UpArrow) && isTunnel && touchingPlayer) {
			ChangeScene ();
			touchingPlayer = false;
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if (!isTunnel && col.CompareTag ("Player")) {
			Debug.Log ("Changing scene normally");
			ChangeScene ();
		} else if (isTunnel) {
			touchingPlayer = true;
		}
	}

	void OnTriggerExit2D(Collider2D col){
		if (isTunnel) {
			touchingPlayer = false;
		}
	}


	/*
	void OnTriggerStay2D(Collider2D col){
		if (isTunnel && col.CompareTag("Player")  && Input.GetKeyDown(KeyCode.UpArrow)) {
			Debug.Log ("going through tunnel");
			ChangeScene ();
			//MainCamera.instance.UpdateBounds (newBounds);
		}
	}

*/
	public void ChangeScene(){
		MosaicCameraScript.instance.SetTargetPosition (targetPosition, newBounds);
		BaseEnemyBehavior[] enemies = FindObjectsOfType(typeof(BaseEnemyBehavior)) as BaseEnemyBehavior[];
		foreach (BaseEnemyBehavior enemy in enemies) {
			enemy.Reset ();
		}
	}
}
