using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyBehavior : MonoBehaviour {
	[HideInInspector] public bool isActive; //flag for detecting player
	[HideInInspector] public bool isDead; //flag for defeated/dead state
	[HideInInspector] public bool isAttacking; //flag for attacking state

	private float moveSpeed = 0.5f;

	private Rigidbody2D rb2d; 

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (isActive && !isDead && !isAttacking) {
			float direction = Mathf.Sign (PlayerController.instance.transform.position.x - transform.position.x);
			rb2d.velocity = new Vector2 (direction * moveSpeed, rb2d.velocity.y);
		}

	}

void OnTriggerEnter2D (Collider2D col){
		if (col.CompareTag("Player") && !isActive){
			StartCoroutine (Activate ());
		}
	}

	virtual protected IEnumerator Activate(){
		Debug.Log ("Starting base enemy activation");
		yield return new WaitForSeconds (1.0f);
		Debug.Log ("Base enemy activated - movement starts");
		isActive = true;
	}
}
