using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeTrigger : MonoBehaviour {
	public Transform targetPositionTransform;
	private Vector3 targetPosition;

	public BoxCollider2D newBounds;


	void Start(){
		targetPosition = targetPositionTransform.position;
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.CompareTag("Player")) {
			MosaicCameraScript.instance.SetTargetPosition (targetPosition, newBounds);
			BaseEnemyBehavior[] enemies = FindObjectsOfType(typeof(BaseEnemyBehavior)) as BaseEnemyBehavior[];
			foreach (BaseEnemyBehavior enemy in enemies) {
				enemy.Reset ();
			}
			//MainCamera.instance.UpdateBounds (newBounds);
		}
	}
		

}
