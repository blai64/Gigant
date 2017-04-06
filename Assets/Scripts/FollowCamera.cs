using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

	private GameObject cam;
	private float zPos;

	void Awake () {
		cam = GameObject.Find ("Main Camera");
		zPos = transform.position.z;
	}

	void Update () {
		transform.position = new Vector3 (cam.transform.position.x,
			cam.transform.position.y, zPos);
	}
}
