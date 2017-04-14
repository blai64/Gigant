using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyBehavior : MonoBehaviour {

	[HideInInspector] public bool isActive; //flag for detecting player
	[HideInInspector] public bool isDead; //flag for defeated/dead state
	[HideInInspector] public bool isAttacking; //flag for attacking state
	[HideInInspector] public float direction;
	[HideInInspector] public bool isAttacked; //flag for attacked by Jack

	private bool canBeActivated;



	private float moveSpeed = 1.0f;

	public int health;

	private Rigidbody2D rb2d; 

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		canBeActivated = true;
		health = 3;
	}
	
	// Update is called once per frame
	virtual protected void Update () {
		//only move when not dead or attacking
		if (isActive && !isDead && !isAttacking) {
			direction = Mathf.Sign (PlayerController.instance.transform.position.x - transform.position.x);
			rb2d.velocity = new Vector2 (direction * moveSpeed, rb2d.velocity.y);
		}

	}

	void OnTriggerEnter2D (Collider2D col){
		if (col.CompareTag("Player") && !isActive){
			StartCoroutine (Activate ());
		}
		if (col.CompareTag ("Weapon") && PlayerController.instance.isAttacking) {
			Debug.Log ("Attacked!");
			GetDamaged (1);
		}

	}

	/*
	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.CompareTag ("Weapon") && isAttacked) {
			Debug.Log ("Attacked!");
			GetDamaged (1);
		}
	}*/


	//TODO: deprecated, since animation events should be able to handle all of this
	//without coroutines, just have an activate function that starts animation, setting 
	//active once animation is over.
	virtual protected IEnumerator Activate(){
		Debug.Log ("Starting base enemy activation");
		yield return new WaitForSeconds (1.0f);
		Debug.Log ("Base enemy activated - movement starts");
		isActive = true;
	}

	virtual protected void StartAttack(){
	}

	virtual protected void EndAttack(){
	}


	public void GetDamaged (int damage){
		health -= damage;

		if (health <= 0)
			Die ();
	}

	void Die(){
		Destroy (gameObject);//temporary...
		//start Death animation
		isActive = false;
		canBeActivated = false;
		StartCoroutine (DisableForTime (3.0f));
	}


	IEnumerator DisableForTime(float seconds){
		yield return new WaitForSeconds (seconds);
		canBeActivated = true;
	}
}
