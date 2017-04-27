using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeTrigger : MonoBehaviour {
	public Transform targetPositionTransform;
	private Vector3 targetPosition;

	public BoxCollider2D newBounds;

	public bool isTunnel;
	public bool isExit = false;
	public string nextLevelName = "";
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
			ChangeScene ();
		} else if (isTunnel && col.CompareTag("Player")) {
			touchingPlayer = true;
		}
	}

	void OnTriggerExit2D(Collider2D col) {
		if (isTunnel) {
			touchingPlayer = false;
		}
	}
		
	public void ChangeScene() {
		if (!isExit) {
			MosaicCameraScript.instance.SetTargetPosition (targetPosition, newBounds);
		} else {
			SceneManager.LoadScene (nextLevelName);
		}
	}
}
