using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerscenechange : MonoBehaviour {
	public static triggerscenechange instance;

	public GameObject testcube;
	public GameObject camPos1;

	public bool isColliding;

	// Use this for initialization
	void Start () {
		isColliding = false;
	}

	void Awake(){
		if (instance == null)
			instance = this;
	}

	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.name == "collider") {
			isColliding = true;
			Debug.Log (isColliding);
		}
	}

	void Update(){
		if (isColliding) {
			Vector3 pos = camPos1.transform.position;
			pos.z = testcube.transform.position.z;
			testcube.transform.position = pos;
			//testcube.transform.position = new Vector3 (camPos1.transform.position.x, (float)camPos1.transform.position.y, (float)testcube.transform.position);

		}
	}

}
