using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderManager : MonoBehaviour {

	public static BoulderManager instance;

	public GameObject boulderPrefab;
	public Transform spawnTrans;
	public Transform disappearTrans;
	private Vector3 spawnPos;
	private Rigidbody2D rb2d;

	public bool startFalling; // use flag to check if the boulder reaches the bottom
	public bool reset;
	public float range;



	// Use this for initialization
	void Start () {
		spawnPos = spawnTrans.position;
		boulderPrefab.SetActive (false);
		startFalling = false;
		reset = true;
		rb2d = boulderPrefab.GetComponent<Rigidbody2D> ();
	}

	void Awake() {
		if (instance == null)
			instance = this;

	}


	void Update(){
		if (reset) {
			boulderPrefab.transform.position = new Vector3 (spawnPos.x + range * Random.value, spawnPos.y, spawnPos.z);
			rb2d.velocity = new Vector2 (0.0f, 0.0f);
		}
		if (startFalling) {
			reset = false;
			boulderPrefab.SetActive (true);
			Rotate (boulderPrefab);

			if (Mathf.Abs(boulderPrefab.transform.position.y - disappearTrans.transform.position.y) <= 2.0f) {
				reset = true;
			}
		}

	}

	void Rotate(GameObject obj){
		obj.transform.Rotate(Vector3.back * Time.deltaTime * 1000);
	}


	void OnTriggerEnter2D (Collider2D col){
		if (col.CompareTag("Player")){
			//StartCoroutine (StartFalling ());
			startFalling = true;
		}
	}
		
}
