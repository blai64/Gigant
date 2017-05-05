using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderManager : MonoBehaviour {

	public GameObject boulderPrefab;
	public Transform[] debrisSpawns;
	public GameObject debrisPrefab;

	public GameObject exclamationPointPrefab;
	public Transform spawnTrans;
	public Transform disappearTrans;
	public float rotateSpeed = 500f;
	public float scaleMin = 0.8f;
	public float scaleMax = 1.0f;
	private float debrisDelta = 1.0f;
	private float newRotate;

	private Vector3 spawnPos;
	private Rigidbody2D rb2d;

	public bool startFalling; // use flag to check if the boulder reaches the bottom
	public bool reset;
	private bool resetting;
	public float range;
	public float speed;

	void Start () {
		spawnPos = spawnTrans.position;
		boulderPrefab.SetActive (false);
		startFalling = false;
		reset = true;
		rb2d = boulderPrefab.GetComponent<Rigidbody2D> ();
		newRotate = Random.Range (-rotateSpeed, rotateSpeed);
	}




	void Update(){
		boulderPrefab.SetActive (startFalling);

		if (reset) {
			boulderPrefab.transform.position = new Vector3 (spawnPos.x + range * Random.Range(-1f, 1f),
				spawnPos.y, spawnPos.z);
			float newScale = Random.Range (scaleMin, scaleMax);
			boulderPrefab.transform.localScale = new Vector3 (newScale, newScale, newScale);
			newRotate = Random.Range (-rotateSpeed, rotateSpeed);
			rb2d.velocity = new Vector2 (0.0f, 0.0f);

		}

		if (startFalling) {
			if (reset) {
				if (PlayerController.instance.transform.position.y + 3 < spawnPos.y && 
					disappearTrans.position.y <= PlayerController.instance.transform.position.y) {
					if (debrisPrefab != null) {
						foreach (Transform debrisSpawn in debrisSpawns) {
							for (int i = 0; i < 3; i++) {
								float offsetX = Random.Range (-debrisDelta, debrisDelta);
								float offsetY = Random.Range (-debrisDelta, debrisDelta);

								GameObject debris = Instantiate (debrisPrefab);
								debris.transform.position = new Vector3 (boulderPrefab.transform.position.x + offsetX,
									debrisSpawn.position.y + offsetY,
									spawnPos.z);

								debris.GetComponent<Rigidbody2D>().gravityScale += Random.Range (-debrisDelta, debrisDelta);

								StartCoroutine (DestroyDebris (debris));
								
							}

						} 

					} else {
						GameObject warning = Instantiate (exclamationPointPrefab);
						warning.transform.position = new Vector3 (boulderPrefab.transform.position.x,
							PlayerController.instance.transform.position.y + 3,
							spawnPos.z);
					}

				}
			}
			reset = false;
			boulderPrefab.SetActive (true);
			rb2d.velocity = new Vector2 (0.0f, -speed);
			//make the boulders fall in constant speed


			Rotate (boulderPrefab);

			if (Mathf.Abs (boulderPrefab.transform.position.y - disappearTrans.transform.position.y) <= 2.0f) {
				reset = true;
				//resetting = true;
				//StartCoroutine (DelayFall ());

			}
		}
	}

	void Rotate(GameObject obj) {
		obj.transform.Rotate(Vector3.back * Time.deltaTime * newRotate);
	}

	void OnTriggerEnter2D (Collider2D col){
		if (col.CompareTag("Player")) {
			startFalling = true;
		}
	}

//	void OnTriggerExit2D (Collider2D col) {
//		if (col.CompareTag("Player")) {
//			startFalling = false;
//		}
//	}

	IEnumerator DestroyDebris(GameObject debris){
		yield return new WaitForSeconds (3.0f);
		Destroy (debris);
	}

	IEnumerator DelayFall(){
		yield return new WaitForSeconds (Random.Range (0f, 1f));
		reset = true;
		resetting = false;
	}
		
}
